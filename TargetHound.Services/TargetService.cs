﻿namespace TargetHound.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.Services.ErrorMessages;
    using TargetHound.Services.Interfaces;

    public class TargetService : ITargetService
    {
        private readonly TargetHoundContext dbContext;

        public TargetService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpdateTargets(string projectId, string userId, ICollection<Target> targets)
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

            foreach (var target in targets)
            {
                var targetDataObject =
                    this.dbContext.Targets.SingleOrDefault(x => x.Id == target.Id && x.IsDeleted == false);

                if (targetDataObject == null)
                {
                    this.dbContext.Targets.Add(target);
                }
            }

            await this.dbContext.SaveChangesAsync();
        }
    }
}
