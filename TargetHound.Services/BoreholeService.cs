namespace TargetHound.Services
{
    using Microsoft.EntityFrameworkCore;
    using ServiceStack.Text;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.Services.ErrorMessages;

    public class BoreholeService : IBoreholeService
    {
        private readonly TargetHoundContext dbContext;

        public BoreholeService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> ExportBoreholeAsync(string projectId, string userId, Borehole borehole)
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

            var exportBorehole = CsvSerializer.SerializeToString(borehole.SurveyPoints);
            var rootFolder = @$"\wwwroot";
            var rootAddress = @$"\FilesCSV\{borehole.Name}.csv";
            var localAdress = Environment.CurrentDirectory + rootFolder + rootAddress;
            await this.CreateCsvFile(exportBorehole, localAdress);

            return rootAddress;
        }

        public async Task UpdateBoreholesAsync(string projectId, string userId, Borehole borehole)
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

        private async Task CreateCsvFile(string content, string localAdress)
        {
            await File.WriteAllTextAsync(localAdress, content);
        }
    }
}
