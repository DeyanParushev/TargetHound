namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TargetHound.Models;
    using TargetHound.MVC.Areas.Identity;
    using TargetHound.Services.Interfaces;
    using TargetHound.SharedViewModels.ViewModels;
    using TargetHound.SharedViewModels.InputModels;

    public class ClientsController : Controller
    {
        private readonly IClientService clientService;
        private readonly UserManager<ApplicationUser> userManager;

        public ClientsController(IClientService clientService, UserManager<ApplicationUser> userManager)
        {
            this.clientService = clientService;
            this.userManager = userManager;
        }
        
        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> Edit(string clientId)
        {
            string userId = this.userManager.GetUserId(this.User);
            bool userIsClientAdmin =
                await this.clientService.GetAdminId(clientId) == userId;

            if (!userIsClientAdmin)
            {
                return this.View("Error");
            }

            ClientEditInputModel clientInfo =
                 await this.clientService.GetClientInfoByAdminId<ClientEditInputModel>(clientId, userId);

            return this.View(clientInfo);
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            string userId = this.userManager.GetUserId(this.User);
            ICollection<ClientViewModel> clientsInfo = 
                await this.clientService.GetAllClientsByAdminId<ClientViewModel>(userId);
            ClientListModel clientList = new ClientListModel { Clients = clientsInfo };

            return this.View(clientList);
        }

        [Authorize] 
        public async Task<IActionResult> Create()
        {
            ClientCreateInputModel client = new ClientCreateInputModel();
            return this.View(client);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ClientCreateInputModel model)
        {
            ApplicationUser user = await this.userManager.GetUserAsync(this.User);
            string clientId = await this.clientService.CreateClientAsync(model.Name, user.Id);
            await this.userManager.AddToRoleAsync(user, SiteIdentityRoles.ClientAdmin);
            bool userIsAsigned = await this.clientService.AsingAdmin(clientId, user.Id);

            if (!userIsAsigned)
            {
                return this.View("Error");
            }

            return this.Redirect("/Clients/All");
        }
    }
}
