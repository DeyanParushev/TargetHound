namespace TargetHound.MVC.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using TargetHound.DataModels;
    using TargetHound.MVC.Areas.Identity;
    using TargetHound.MVC.Areas.Identity.Pages.Account;
    using TargetHound.MVC.Models;
    using TargetHound.MVC.Settings;
    using TargetHound.Services.Interfaces;
    using TargetHound.Services.Messages;
    using TargetHound.SharedViewModels.InputModels;
    using TargetHound.SharedViewModels.ViewModels;

    [RequireHttps]
    public class ClientsController : Controller
    {
        private readonly IClientService clientService;
        private readonly IEmailSender emailSender;
        private readonly IClientInvitationsService clientInvitationsService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ClientsController(
            IClientService clientService,
            IEmailSender emailSender,
            IClientInvitationsService clientInvitationsService,
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.clientService = clientService;
            this.emailSender = emailSender;
            this.clientInvitationsService = clientInvitationsService;
            this.userService = userService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var userId = this.userManager.GetUserId(this.User);
            var clientsInfo =
                await this.clientService.GetAllClientsByAdminIdAsync<ClientViewModel>(userId);

            return this.View(clientsInfo);
        }

        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> Edit(string clientId)
        {
            try
            {
                var userId = this.userManager.GetUserId(this.User);
                var userIsClientAdmin = await this.clientService.IsUserClientAdminAsync(userId, clientId);

                var clientInfo =
                     await this.clientService.GetClientInfoByAdminIdAsync<ClientEditInputModel>(clientId, userId);

                if (!userIsClientAdmin)
                {
                    this.ModelState.AddModelError(string.Empty, ClientControllerErrorMessages.ClientAdminError);
                    return this.View(nameof(ErrorViewModel));
                }

                return this.View(clientInfo);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }
        }

        [HttpPost]
        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> Edit(string clientId, string name)
        {
            try
            {
                var userId = this.userManager.GetUserId(this.User);
                var userIsClientAdmin = await this.clientService.IsUserClientAdminAsync(userId, clientId);

                if (!userIsClientAdmin)
                {
                    this.ModelState.AddModelError(string.Empty, ClientControllerErrorMessages.ClientAdminError);
                    return this.View(nameof(ErrorViewModel));
                }

                var changeIsSuccess = await this.clientService.ChangeClientNameAsync(clientId, name);

                if (!changeIsSuccess)
                {
                    this.ModelState.AddModelError(string.Empty, ClientControllerErrorMessages.ErrorSavingData);
                    return this.View(nameof(ErrorViewModel));
                }

                return this.RedirectToAction(nameof(this.All));
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }
        }

        [Authorize]
        public IActionResult Create()
        {
            var client = new ClientCreateInputModel();
            return this.View(client);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ClientCreateInputModel model)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                var clientId = await this.clientService.CreateClientAsync(model.Name, user.Id);
                await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.ClientAdmin);
                var userIsAsigned = await this.clientService.AsignAdminAsync(clientId, user.Id);

                if (!userIsAsigned)
                {
                    this.ModelState.AddModelError(string.Empty, ClientControllerErrorMessages.ErrorChangingAdmin);
                    return this.View(nameof(ErrorViewModel));
                }

                return this.Redirect(nameof(this.All));
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }
        }

        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> ChangeAdmin(string clientId)
        {
            var clientUsers = await this.clientService.GetClientUsersAsync<UserViewModel>(clientId);
            var viewModel = new ClientEditViewModel
            {
                Id = clientId,
                Users = clientUsers,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> AsignAdmin(string userId, string clientId)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                var isAdmin = await this.clientService.IsUserClientAdminAsync(currentUser.Id, clientId);

                if (!isAdmin)
                {
                    this.ModelState.AddModelError(string.Empty, ClientControllerErrorMessages.ClientAdminError);
                    return this.View(nameof(ErrorViewModel));
                }

                await this.userManager.RemoveFromRoleAsync(currentUser, SiteIdentityRoles.ClientAdmin);
                bool adminIsChanged = await this.clientService.ChangeClientAdminAsync(clientId, userId);
                ApplicationUser newAdmin = await this.userManager.FindByIdAsync(userId);

                if (newAdmin == null)
                {
                    return this.View(nameof(ErrorViewModel));
                }

                await this.userManager.AddToRoleAsync(newAdmin, SiteIdentityRoles.ClientAdmin);

                if (!adminIsChanged)
                {
                    return this.View(nameof(ErrorViewModel));
                }

                await this.signInManager.SignOutAsync();
                return this.Redirect(nameof(LoginModel));
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }
        }

        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> AddUser(string clientId)
        {
            var addUserModel = new AddUserModel
            {
                Id = clientId,
            };
            return this.View(addUserModel);
        }

        [HttpPost]
        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> AddUser(string clientId, string email)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                var receiver = await this.userManager.FindByEmailAsync(email);
                var clientName = await this.clientService.GetClientNameByIdAsync(clientId);
                var linkToJoin = this.Url.Action(
                    "Join", "Clients", new { clientId = clientId }, this.Request.Scheme);
                var receiverEmail = email;

                if (receiver == null)
                {
                    await this.userService.SendClientInvitationAsync(user.Email, user.UserName, receiverEmail, receiverEmail, clientId, linkToJoin);
                }
                else
                {
                    await this.clientService.AsignUserToClientAsync(receiver.Id, clientId);
                    await this.userService.SendClientInvitationAsync(user.Email, user.UserName, receiver.Email, receiver.UserName, clientId, linkToJoin);
                }

                await this.clientInvitationsService.AddClientInvitationAssync(clientId, receiverEmail);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }

            return this.Redirect(nameof(this.All));
        }

        [HttpPost]
        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> Delete(string clientId)
        {
            string userId = this.userManager.GetUserId(this.User);
            try
            {
                await this.clientService.SetClientToNullAsync(userId, clientId);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(nameof(ErrorViewModel));
            }

            return this.Redirect(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> InactiveClients()
        {
            string userId = this.userManager.GetUserId(this.User);
            var inactiveClients = await this.clientService.GetInactiveClientsAsync<ClientViewModel>(userId);
            return this.View(inactiveClients);
        }

        [Authorize]
        public async Task<IActionResult> Activate(string clientId)
        {
            string userId = this.userManager.GetUserId(this.User);
            await this.clientService.ActivateClient(clientId, userId);
            
            return this.RedirectToAction(nameof(this.All));
        }

        // TODO: Check for correct redirection after deployment
        public async Task<IActionResult> Join(string clientId)
        {
            string clientName = await this.clientService.GetClientNameByIdAsync(clientId);
            var register = new RegisterModel.InputModel
            {
                Company = clientName,
            };

            return this.RedirectToAction(nameof(RegisterModel), register);
        }
    }
}
