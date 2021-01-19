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

    public class CollarServiceTests
    {
        [TestCase("Username1")]
        [TestCase("AnotherRandomUserName")]
        public void UpdateCollarsShouldThrowExceptionWhenUserDontExist(string username)
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
            var collars = new List<Collar>();

            var context = this.SetupContext();
            context.Projects.Add(project);
            context.SaveChanges();

            var collarService = new CollarService(context);

            //// Assert
            Assert.That(
                    async () => await collarService.UpdateCollars(project.Id, user.Id, collars),
                    Throws.ArgumentNullException);
        }

        [TestCase("ProjectName")]
        [TestCase("AnotherRandomProjectName")]
        public void UpdateCollarsShouldThrowExceptionsWhenProjectDontExist(string projectName)
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
            var collars = new List<Collar>();

            var context = this.SetupContext();
            context.Users.Add(user);
            context.SaveChanges();

            var collarService = new CollarService(context);

            //// Assert
            Assert.That(
                    async () => await collarService.UpdateCollars(project.Id, user.Id, collars),
                    Throws.ArgumentNullException);
        }

        [TestCase("SomeRandomUsername", "SomeRandomProjectName")]
        [TestCase("SomeRandomUsername1", "SomeRandomProjectName1")]
        [TestCase("SomeRandomUsername2", "SomeRandomProjectName2")]
        public void UpdateCollarsShouldThrowExceptionWhenUserIsNotInProject(
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

            var collars = new List<Collar>();

            var context = this.SetupContext();
            context.Users.Add(user);
            context.Projects.Add(project);
            context.SaveChanges();

            var collarService = new CollarService(context);
            //// Assert
            Assert.That(
                    async () => await collarService.UpdateCollars(project.Id, user.Id, collars),
                    Throws.ArgumentException);
        }

        [TestCase(0, 0, 0)]
        [TestCase(10, 10, 10)]
        [TestCase(-100, -100, -100)]
        public async Task UpdateCollarsShouldUpdateExistingCollars(
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
            var collars = new List<Collar>
            {
                new Collar
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Collar",
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
            context.Collars.AddRange(collars);
            context.SaveChanges();
            var collarService = new CollarService(context);

            //// Act
            int coordinateChange = 10;
            foreach (var collar in collars)
            {
                collar.Easting += coordinateChange;
                collar.Northing += coordinateChange;
                collar.Elevation += coordinateChange;
            }

            await collarService.UpdateCollars(project.Id, user.Id, collars);

            var updatedCollars = new List<Collar>();
            foreach (var collar in collars)
            {
                var updatedCollar = context.Collars.FirstOrDefault(x => x.Id == collar.Id);
                updatedCollars.Add(updatedCollar);
            }
            ////Assert
            foreach (var updatedCollar in updatedCollars)
            {
                Assert.AreEqual(easting + coordinateChange, updatedCollar.Easting);
                Assert.AreEqual(northing + coordinateChange, updatedCollar.Northing);
                Assert.AreEqual(elevation + coordinateChange, updatedCollar.Elevation);
            }
        }

        [TestCase(0, 0, 0)]
        [TestCase(10, 10, 10)]
        [TestCase(-100, -100, -100)]
        public async Task UpdateCollarsShouldCreateNewCollarIfItDoesntExist(
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
            var collarService = new CollarService(context);

            //// Act
            var collars = new List<Collar>
            {
                new Collar
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Collar",
                    Easting = easting,
                    Northing = northing,
                    Elevation = elevation
                },
            };

            await collarService.UpdateCollars(project.Id, user.Id, collars);
            var collar = context.Collars.FirstOrDefault(x => x.Id == collars[0].Id);

            //// Assert
            Assert.AreEqual(collar.Easting, collars[0].Easting);
            Assert.AreEqual(collar.Northing, collars[0].Northing);
            Assert.AreEqual(collar.Elevation, collars[0].Elevation);
            Assert.AreEqual(collar.Name, collars[0].Name);
            Assert.AreEqual(collar.Id, collars[0].Id);
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
