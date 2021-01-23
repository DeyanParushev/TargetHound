namespace TargetHound.Services.Tests
{
    using System;
    using System.Threading.Tasks;
    
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    
    using TargetHound.Data;
    using TargetHound.DataModels;

    //// TODO: Test the rest of the mothods
    public class BoreholeServiceTests
    {
        [TestCase]
        public async Task GetBoreholeNameShouldThrowExeptionIfHoleDoesntExist()
        {
            //// Arrange
            var boreholeId = Guid.NewGuid().ToString();
            
            var context = this.SetupContext();
            var boreholeService = new BoreholeService(context);

            //// Assert 
            Assert.That(
                async () => await boreholeService.GetBoreholeName(boreholeId), Throws.ArgumentException);
        }

        [TestCase]
        public async Task GetBoreholeNameShouldThrowExeptionIfHoleIsDeleted()
        {
            //// Arrange
            var borehole = new Borehole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Some random name",
                IsDeleted = true,
            };

            var context = this.SetupContext();
            context.Boreholes.Add(borehole);
            await context.SaveChangesAsync();
            var boreholeService = new BoreholeService(context);

            //// Assert 
            Assert.That(
                async () => await boreholeService.GetBoreholeName(borehole.Id), Throws.ArgumentException);
        }

        [TestCase("Borehole1")]
        [TestCase("Borehole2")]
        [TestCase("Borehole3")]
        public async Task GetBoreholeNameShouldWorkProperly(string holeName)
        {
            //// Arrange
            var borehole = new Borehole
            {
                Id = Guid.NewGuid().ToString(),
                Name = holeName,
                IsDeleted = false,
            };

            var context = this.SetupContext();
            context.Boreholes.Add(borehole);
            await context.SaveChangesAsync();

            var boreholeService = new BoreholeService(context);

            //// Assert
            Assert.AreEqual(holeName, await boreholeService.GetBoreholeName(borehole.Id));
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
