namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    using TargetHound.Models;
    using TargetHound.MVC.Areas.Identity;
    using TargetHound.Services.Interfaces;
    using TargetHound.SharedViewModels.ViewModels;
    using TargetHound.SharedViewModels.InputModels;
    using TargetHound.Services.Messages;

    public class ClientsController : Controller
    {
        private readonly IClientService clientService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ClientsController(
            IClientService clientService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.clientService = clientService;
            this.emailSender = emailSender;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var userId = this.userManager.GetUserId(this.User);
            var clientsInfo =
                await this.clientService.GetAllClientsByAdminId<ClientViewModel>(userId);
            ClientListModel clientList = new ClientListModel { Clients = clientsInfo };

            return this.View(clientList);
        }

        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> Edit(string clientId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var userIsClientAdmin = await this.clientService.IsUserClientAdminAsync(userId, clientId);

            if (!userIsClientAdmin)
            {
                return this.View("Error");
            }

            var clientInfo =
                 await this.clientService.GetClientInfoByAdminId<ClientEditInputModel>(clientId, userId);

            return this.View(clientInfo);
        }

        [HttpPost]
        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> Edit(string clientId, string name)
        {
            var userId = this.userManager.GetUserId(this.User);
            var userIsClientAdmin = await this.clientService.IsUserClientAdminAsync(userId, clientId);

            if (!userIsClientAdmin)
            {
                return this.View("Error");
            }

            var changeIsSuccess = await this.clientService.ChangeClientNameAsync(clientId, name);

            if (!changeIsSuccess)
            {
                return this.View("Error");
            }

            return this.RedirectToAction("All");
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var client = new ClientCreateInputModel();
            return this.View(client);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ClientCreateInputModel model)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var clientId = await this.clientService.CreateClientAsync(model.Name, user.Id);
            await this.clientService.AsignUserToClientAsync(user.Id, clientId);
            await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.ClientAdmin);
            var userIsAsigned = await this.clientService.AsingAdminAsync(clientId, user.Id);

            if (!userIsAsigned)
            {
                return this.View("Error");
            }

            return this.Redirect("/Clients/All");
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
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isAdmin = await this.clientService.IsUserClientAdminAsync(currentUser.Id, clientId);

            if (!isAdmin)
            {
                return this.View("Error");
            }

            await this.userManager.RemoveFromRoleAsync(currentUser, SiteIdentityRoles.ClientAdmin);
            bool adminIsChanged = await this.clientService.ChangeClientAdmin(clientId, userId);
            ApplicationUser newAdmin = await this.userManager.FindByIdAsync(userId);

            if (newAdmin == null)
            {
                return this.View("Error");
            }

            await this.userManager.AddToRoleAsync(newAdmin, SiteIdentityRoles.ClientAdmin);

            if (!adminIsChanged)
            {
                return this.View("Error");
            }

            await this.signInManager.SignOutAsync();
            return this.Redirect("/Identity/Pages/Account/Login");
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
                var newUser = await this.userManager.FindByEmailAsync(email);
                var receiverEmail = email;
                if (newUser == null)
                {
                    await this.emailSender.SendEmailAsync(
                        user.Email, 
                        user.UserName, 
                        receiverEmail, 
                        receiverEmail, 
                        "hi", 
                        "Hi from targetHound");
                }
                else
                {
                    await this.clientService.AsignUserToClientAsync(newUser.Id, clientId);
                    await this.emailSender.SendEmailAsync(
                        user.Email, 
                        user.UserName, 
                        newUser.Email,
                        newUser.UserName, 
                        "hi", 
                        "Hi from targetHound");
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
            }

            return this.Redirect("/Clients/All");
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
            catch(Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
            }

            return this.Redirect("/Clients/All");
        }
    }
}
