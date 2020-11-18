namespace TargetHound.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        public Task<string> GetUserNameById(string userId);
    }
}
