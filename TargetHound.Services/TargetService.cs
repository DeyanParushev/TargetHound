namespace TargetHound.Services
{
    using Microsoft.EntityFrameworkCore.Internal;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Models;
    using TargetHound.Models.Interfaces;
    using TargetHound.Services.Interfaces;

    public class TargetService : ITargetService
    {
        private TargetHoundContext dbContext;

        public TargetService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(string name, double easting, double northing, double elevation)
        {
            await this.dbContext.Targets.AddAsync(new Target
            {
                Name = name,
                Easting = easting,
                Northing = northing,
                Elevation = elevation,
                Depth = 0,
                Dip = 0,
                Azimuth = 0,
            });
            return true;
        }

        public ICollection<Target> GetAllTargets(string projectId)
        {
            var targets = this.dbContext.Projects.SingleOrDefault(x => x.Id == projectId)?.Targets.ToList();

            return targets;
        }

        public async Task<bool> ChangeName(string targetId, string newName)
        {
            Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);
            if(target == null)
            {
                return false;
            }

            target.Name = newName;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeEasting(string targetId, double newEasting)
        {
            Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);
            if (target == null)
            {
                return false;
            }

            target.Easting = newEasting;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeNorthing(string targetId, double newNorthing)
        {
            Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);
            if (target == null)
            {
                return false;
            }

            target.Northing = newNorthing;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeElevation(string targetId, double newElevation)
        {
            Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);
            if (target == null)
            {
                return false;
            }

            target.Elevation = newElevation;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTarget(string targetId)
        {
            Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);
            if(target == null)
            {
                return false;
            }

            target.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public Target GetByName(string targetName)
        {
            Target target = this.dbContext.Targets.SingleOrDefault(x => x.Name == targetName);
            return target;
        }

        public Target GetById(string targetId)
        {
            Target target = this.dbContext.Targets.SingleOrDefault(x => x.Id == targetId);
            return target;
        }
    }
}
