namespace TargetHound.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
   
    using TargetHound.Data;
    using TargetHound.Models;

    public class BoreholeService : IBoreholeService
    {
        private TargetHoundContext dbContext;

        public BoreholeService(TargetHoundContext context)
        {
            this.dbContext = context;
        }

        public async Task CreateBorehole(string name)
        {
            this.dbContext.Boreholes.Add(new Borehole { Name = name });
            await this.dbContext.SaveChangesAsync();
        }

        public ICollection<Borehole> GetProjectBoreHoles(string projectName)
        {
            var boreholes = this.dbContext.Boreholes
                .Where(x => x.Project.Name == projectName)
                .ToList();

            return boreholes;
        }

        //public async Task<bool> AddTargetToBorehole(string boreholeId, string targetId)
        //{
        //    Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);
        //    if (target == null)
        //    {
        //        return false;
        //    }

        //    this.dbContext.Boreholes.SingleOrDefault(x => x.Id == boreholeId).Targets.Add(target);
        //    await this.dbContext.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<T> GetStraighHoleCollarAngles<T>(string collarId, string targetId)
        //{
        //    Collar collar = this.dbContext.Collars.SingleOrDefault(x => x.Id == collarId);
        //    Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);

        //    StraightExtrapolationCalculator straightExtrapolation = new StraightExtrapolationCalculator();
        //    collar.Azimuth = straightExtrapolation.GetInitialAzimuthAngle(collar, target);
        //    collar.Dip = straightExtrapolation.GetInitialDipAngle(collar, target);

        //    return collar;
        //}
    }
}
