namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TargetHound.DataModels;

    public interface ITargetService
    {
        public Task UpdateTargets(string projectId, string userId, ICollection<Target> targets);
    }
}
