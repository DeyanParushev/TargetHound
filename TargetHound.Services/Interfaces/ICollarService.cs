namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TargetHound.DataModels;

    public interface ICollarService
    {
        public Task UpdateCollars(string projectId, string userId, ICollection<Collar> collars);
    }
}
