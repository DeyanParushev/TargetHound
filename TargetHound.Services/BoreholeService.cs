﻿namespace TargetHound.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
   
    using Microsoft.EntityFrameworkCore;
    using ServiceStack.Text;
    
    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;
    using TargetHound.Services.ErrorMessages;

    public class BoreholeService : IBoreholeService
    {
        private readonly TargetHoundContext dbContext;

        public BoreholeService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateBoreholeCsv(string projectId, string userId, Borehole borehole, string saveDirectory)
        {
            this.CheckUserAndProjectExist(projectId, userId);
             
            var exportData = borehole.SurveyPoints
                .Select(x => new
                {
                    Depth = x.Depth,
                    Azimuth = x.Azimuth,
                    Dip = x.Dip,
                    Easting = x.Easting,
                    Northing = x.Northing,
                    Elevation = x.Elevation
                })
                .OrderBy(x => x.Depth)
                .ToList();
            var exportBorehole = CsvSerializer.SerializeToString(exportData);

            await File.WriteAllTextAsync(saveDirectory, exportBorehole);
        }

        public async Task UpdateBoreholesAsync(string projectId, string userId, Borehole borehole)
        {
            this.CheckUserAndProjectExist(userId, projectId);

            if (!this.dbContext.Boreholes.Any(x => x.Id == borehole.Id && x.IsDeleted == false))
            {
                if (this.dbContext.Collars.Any(x => x.ProjectId == projectId
                 && x.IsDeleted == false
                 && x.Id == borehole.Collar.Id))
                {
                    this.dbContext.Entry(borehole.Collar).State = EntityState.Modified;
                }

                this.dbContext.Boreholes.Add(borehole);
            }
            else if (this.dbContext.Entry(borehole).State == EntityState.Modified)
            {
                this.dbContext.Boreholes.Update(borehole);
            }

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<string> GetBoreholeName(string boreholeId)
        {
            if(!this.dbContext.Boreholes.Any(x => x.Id == boreholeId && x.IsDeleted == false))
            {
                throw new ArgumentException("Borehole does not exist.");
            }

            string name = this.dbContext.Boreholes.FirstOrDefault(x => x.Id == boreholeId && x.IsDeleted == false)?.Name;
            return name;
        }

        public async Task<T> GetBorehole<T>(string boreholeId)
        {
            var borehole = this.dbContext.Boreholes.Where(x => x.Id == boreholeId).To<T>().SingleOrDefault();
            
            if(borehole == null)
            {
                throw new ArgumentNullException("Borehole does not exist.");
            }

            return borehole;
        }

        public async Task Create(Borehole borehole)
        {
            this.dbContext.Boreholes.Add(borehole);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task Edit(Borehole inputBorehole)
        {
            var borehole = this.dbContext.Boreholes.SingleOrDefault(x => x.Id == inputBorehole.Id);

            if(borehole == null)
            {
                throw new ArgumentNullException("Borehole does not exist.");
            }

            borehole = inputBorehole;
            await this.dbContext.SaveChangesAsync();
        }

        private void CheckUserAndProjectExist(string userId, string projectId)
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
        }
    }
}
