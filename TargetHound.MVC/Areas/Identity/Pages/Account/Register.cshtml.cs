namespace TargetHound.MVC.Areas.Identity.Pages.Account
{
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
    using TargetHound.DataModels;
    using TargetHound.Services.Interfaces;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly TargetHoundContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
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
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this.logger = logger;
            this.emailSender = emailSender;
            this.clientService = clientService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = this.Input.Username, Email = this.Input.Email };

                if (this.roleManager.Roles.Count() <= 0)
                {
                    await this.CreateRoles();
                }

                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.User);

                ICollection<ApplicationUser> siteAdmins = await this.userManager.GetUsersInRoleAsync(SiteIdentityRoles.SiteAdmin);
                if (siteAdmins.Count == 0)
                {
                    await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.SiteAdmin);
                }

                if (this.Input.Company != "None" && !string.IsNullOrWhiteSpace(this.Input.Company))
                {
                    if (!this.dbContext.Clients.Any(x => x.Name.ToLower() == this.Input.Company))
                    {
                        await this.clientService.CreateClientAsync(this.Input.Company, user.Id);
                        await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.ClientAdmin);
                    }

                    user.ClientId = this.dbContext.Clients.FirstOrDefault(x => x.Name == this.Input.Company)?.Id;
                }

                if (result.Succeeded)
                {
                    return await this.Login(user, result, returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        // TODO: extract user roles into enum for code quality
        private async Task<IActionResult> Login(ApplicationUser user, IdentityResult result, string returnUrl)
        {
            this.logger.LogInformation("User created a new account with password.");

            var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await this.emailSender
                .SendEmailAsync(
                this.Input.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (this.userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
            }
            else
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
                return this.LocalRedirect(returnUrl);
            }
        }

        private async Task CreateRoles()
        {
            var siteRoles = typeof(SiteIdentityRoles).GetFields();

            foreach (FieldInfo role in siteRoles)
            {
                bool roleExist = await this.roleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    await this.roleManager.CreateAsync(new ApplicationRole(role.Name));
                }
            }
        }

        public class InputModel
        {
            [Required]
            [MinLength(4)]
            [MaxLength(40)]
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
    }
}
