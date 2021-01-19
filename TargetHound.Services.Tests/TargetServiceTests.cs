namespace TargetHound.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    using TargetHound.Data;
    using TargetHound.DataModels;

    public class TargetServiceTests
    {
        [TestCase("Username1")]
        [TestCase("AnotherRandomUserName")]
        public void UpdateTargetShouldThrowExceptionWhenUserDontExist(string username)
        {
            //// Arange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = username,
            };

            var project = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeRandomProject",
            };
            var targets = new List<Target>();

            var context = this.SetupContext();
            context.Projects.Add(project);
            context.SaveChanges();

            var targetService = new TargetService(context);

            //// Assert
            Assert.That(
                    async () => await targetService.UpdateTargets(project.Id, user.Id, targets),
                    Throws.ArgumentNullException);
        }

        [TestCase("ProjectName")]
        [TestCase("AnotherRandomProjectName")]
        public void UpdateTargetShouldThrowExceptionsWhenProjectDontExist(string projectName)
        {
            //// Arange
            var project = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = projectName,
            };

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "SomeRanodmUserName",
                PasswordHash = "123456",
                Email = "random_user@mail.com",
            };
            var targets = new List<Target>();

            var context = this.SetupContext();
            context.Users.Add(user);
            context.SaveChanges();

            var targetService = new TargetService(context);

            //// Assert
            Assert.That(
                    async () => await targetService.UpdateTargets(project.Id, user.Id, targets),
                    Throws.ArgumentNullException);
        }

        [TestCase("SomeRandomUsername", "SomeRandomProjectName")]
        [TestCase("SomeRandomUsername1", "SomeRandomProjectName1")]
        [TestCase("SomeRandomUsername2", "SomeRandomProjectName2")]
        public void UpdateTargetsShouldThrowExceptionWhenUserIsNotInProject(
            string username,
            string projectName)
        {
            //// Arange 
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = username,
                PasswordHash = username,
                Email = username,
            };

            var project = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = projectName,
            };

            var targets = new List<Target>();

            var context = this.SetupContext();
            context.Users.Add(user);
            context.Projects.Add(project);
            context.SaveChanges();

            var targetService = new TargetService(context);
            //// Assert
            Assert.That(
                    async () => await targetService.UpdateTargets(project.Id, user.Id, targets),
                    Throws.ArgumentException);
        }

        [TestCase(0, 0, 0)]
        [TestCase(10, 10, 10)]
        [TestCase(-100, -100, -100)]
        public async Task UpdateTargetsShouldUpdateExistingTargets(
            double easting,
            double northing,
            double elevation)
        {
            //// Arange 
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "SomeUsername",
                PasswordHash = "SomeUsername",
                Email = "SomeUsername",
            };
            var project = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeProjectName",
            };
            var targets = new List<Target>
            {
                new Target
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Target",
                    Easting = easting,
                    Northing = northing,
                    Elevation = elevation
                },
            };
            var userProject = new UserProject
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                ProjectId = project.Id,
            };
            var context = this.SetupContext();
            context.Users.Add(user);
            context.Projects.Add(project);
            context.UsersProjects.Add(userProject);
            context.Targets.AddRange(targets);
            context.SaveChanges();
            var targetService = new TargetService(context);

            //// Act
            int coordinateChange = 10;
            foreach (var target in targets)
            {
                target.Easting += coordinateChange;
                target.Northing += coordinateChange;
                target.Elevation += coordinateChange;
            }

            await targetService.UpdateTargets(project.Id, user.Id, targets);

            var updatedTargets = new List<Target>();
            foreach (var target in targets)
            {
                var updatedTarget = context.Targets.FirstOrDefault(x => x.Id == target.Id);
                updatedTargets.Add(updatedTarget);
            }
            ////Assert
            foreach (var updatedTarget in updatedTargets)
            {
                Assert.AreEqual(easting + coordinateChange, updatedTarget.Easting);
                Assert.AreEqual(northing + coordinateChange, updatedTarget.Northing);
                Assert.AreEqual(elevation + coordinateChange, updatedTarget.Elevation);
            }
        }

        [TestCase(0, 0, 0)]
        [TestCase(10, 10, 10)]
        [TestCase(-100, -100, -100)]
        public async Task UpdateTargetsShouldCreateNewTargetIfItDoesntExist(
            double easting,
            double northing,
            double elevation)
        {
            //// Arange 
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "SomeUsername",
                PasswordHash = "SomeUsername",
                Email = "SomeUsername",
            };
            var project = new Project
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeProjectName",
            };

            var userProject = new UserProject
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationUserId = user.Id,
                ProjectId = project.Id,
            };
            var context = this.SetupContext();
            context.Users.Add(user);
            context.Projects.Add(project);
            context.UsersProjects.Add(userProject);
            context.SaveChanges();
            var targetService = new TargetService(context);

            //// Act
            var targets = new List<Target>
            {
                new Target
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Target",
                    Easting = easting,
                    Northing = northing,
                    Elevation = elevation
                },
            };

            await targetService.UpdateTargets(project.Id, user.Id, targets);
            var target = context.Targets.FirstOrDefault(x => x.Id == targets[0].Id);

            //// Assert
            Assert.AreEqual(target.Easting, targets[0].Easting);
            Assert.AreEqual(target.Northing, targets[0].Northing);
            Assert.AreEqual(target.Elevation, targets[0].Elevation);
            Assert.AreEqual(target.Name, targets[0].Name);
            Assert.AreEqual(target.Id, targets[0].Id);
        }

        private TargetHoundContext SetupContext()
        {
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;

            var context = new TargetHoundContext(options);
            return context;
        }
    }
}
