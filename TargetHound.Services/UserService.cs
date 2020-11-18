namespace TargetHound.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Services.Interfaces;

    public class UserService : IUserService
    {
        private readonly TargetHoundContext dbContext;

        public UserService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> GetUserNameById(string userId)
        {
            string userName = this.dbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId).UserName;
            return userName;
        }
    }
}
