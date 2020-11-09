namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TargetHound.Models;

    public interface ITargetService
    {
        public async Task<bool> Create(string name, double easting, double northing, double elevation) { return true; }

        public ICollection<Target> GetAllTargets(string projectId);
      
        public Target GetByName(string targetName);
        
        public Target GetById(string targetId);

        public Task<bool> ChangeName(string targetId, string newName);

        public Task<bool> ChangeEasting(string targetId, double newEasting);

        public Task<bool> ChangeNorthing(string targetId, double newNorthing);

        public Task<bool> ChangeElevation(string targetId, double newElevation);

        public Task<bool> DeleteTarget(string targetId);
    }
}
