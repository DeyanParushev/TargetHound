namespace TargetHound.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TargetHound.Models;

    public interface IBoreholeService
    {
        public ICollection<Borehole> GetProjectBoreHoles(string projectName);

        public async Task CreateBorehole(string name) { }
    }
}