namespace TargetHound.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.Services.ErrorMessages;
    using TargetHound.Services.Interfaces;

    public class CollarService : ICollarService
    {
        private readonly TargetHoundContext dbContext;

        public CollarService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpdateCollars(string projectId, string userId, ICollection<Collar> collars)
        {
            if (!this.dbContext.Users.Any(x => x.Id == userId && x.IsDeleted == false))
            {
                throw new ArgumentNullException(UserErrorMessages.UserDoesNotExist);
            }

            if (!this.dbContext.Projects.Any(x => x.Id == projectId && x.IsDeleted == false))
            {
                throw new ArgumentNullException(ProjectErrorMessages.ProjectDoesNotExist);
            }

            if (!this.dbContext.UsersProjects.Any(x => x.ApplicationUserId == userId && x.ProjectId == projectId))
            {
                throw new ArgumentException(ProjectErrorMessages.UserNotInProject);
            }

            foreach (var collar in collars)
            {
                var collarDataObject =
                    this.dbContext.Collars.SingleOrDefault(x => x.Id == collar.Id && x.IsDeleted == false);

                if (collarDataObject == null)
                {
                    this.dbContext.Collars.Add(collar);
                }
            }

            await this.dbContext.SaveChangesAsync();
        }
    }
}
