namespace TargetHound.MVC.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using TargetHound.Data;
    using TargetHound.Models;
    using TargetHound.Services.Interfaces;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly TargetHoundContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IClientService clientService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            TargetHoundContext dbContext,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IClientService clientService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.dbContext = dbContext;
            _logger = logger;
            _emailSender = emailSender;
            this.clientService = clientService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [MinLength(4)]
            [MaxLength(15)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Company name")]
            public string Company { get; set; } = "None";
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email };

                if (_roleManager.Roles.Count() <= 0)
                {
                    await this.CreateRoles();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, SiteIdentityRoles.User);

                ICollection<ApplicationUser> admins = await _userManager.GetUsersInRoleAsync(SiteIdentityRoles.SiteAdmin);
                if(admins.Count == 0)
                {
                    await _userManager.AddToRoleAsync(user, SiteIdentityRoles.SiteAdmin);
                }

                if (Input.Company != "None" && !string.IsNullOrWhiteSpace(Input.Company))
                {
                    if (!this.dbContext.Clients.Any(x => x.Name.ToLower() == Input.Company))
                    {
                        await this.clientService.CreateClientAsync(Input.Company, user.Id);
                        await _userManager.AddToRoleAsync(user, SiteIdentityRoles.ClientAdmin);
                    }
                }

                if (result.Succeeded)
                {
                    return await this.Login(user, result, returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        // TODO: extract user roles into enum for code quality
        private async Task<IActionResult> Login(ApplicationUser user, IdentityResult result, string returnUrl)
        {
            _logger.LogInformation("User created a new account with password.");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }

        private async Task CreateRoles()
        {
            var siteRoles = typeof(SiteIdentityRoles).GetFields();

            foreach (FieldInfo role in siteRoles)
            {
                bool roleExist = await _roleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new ApplicationRole(role.Name));
                }
            }
        }
    }
}
