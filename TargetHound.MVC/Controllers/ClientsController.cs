namespace TargetHound.MVC.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TargetHound.MVC.Areas.Identity;
    using TargetHound.Services.Interfaces;
    using TargetHound.SharedViewModels.ViewModels;

    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [Authorize(Roles = SiteIdentityRoles.ClientAdmin)]
        public async Task<IActionResult> Edit(string clientId)
        {
            string userId = this.User.Identity.Name;
            bool userIsClientAdmin =
                await this.clientService.GetAdminId(clientId) == this.User.Identity.Name;

            if (!userIsClientAdmin)
            {
                return this.View("Error");
            }

            ICollection<ClientViewModel> clientInfo =
                 await this.clientService.GetClientInfoByAdminId<ClientViewModel>(userId);

            return this.View(clientInfo);
        }

        [Authorize]
        public async Task<IActionResult> All(string userId)
        {
            ICollection<ClientViewModel> clientsInfo = 
                await this.clientService.GetClientInfoByUserId<ClientViewModel>(userId);
            ClientListModel clientList = new ClientListModel { Clients = clientsInfo };

            return this.View(clientList);
        }
    }
}
