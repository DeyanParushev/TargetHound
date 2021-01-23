namespace TargetHound.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;
    using TargetHound.SharedViewModels.ViewModels;

    public class ContractorServiceTests
    {
        [TestCase]
        public async Task GetAllContractorsShouldThrowExceptionIfContractorDoesntExist()
        {
            //// Arange
            var contractorId = Guid.NewGuid().ToString();

            AutoMapperConfig.RegisterMappings(
                typeof(DrillRigViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;

            var context = new TargetHoundContext(options);
            var contractorService = new ContractorService(context);

            //// Assert
            Assert.That(
                    async () => await contractorService.GetDrillRigsAsync<DrillRigViewModel>(contractorId),
                    Throws.ArgumentException);
        }

        [TestCase]
        public async Task GetAllContractorsShouldThrowExceptionIfContractorIsDeleted()
        {
            //// Arange
            var contractor = new Contractor
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeRandomName",
                IsDeleted = true,
            };

            AutoMapperConfig.RegisterMappings(
                typeof(DrillRigViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;

            var context = new TargetHoundContext(options);
            context.Contractors.Add(contractor);
            await context.SaveChangesAsync();
            var contractorService = new ContractorService(context);

            //// Assert
            Assert.That(
                    async () => await contractorService.GetDrillRigsAsync<DrillRigViewModel>(contractor.Id),
                    Throws.ArgumentException);
        }

        [TestCase]
        public async Task GetAllContractorsShouldWorkProperly()
        {
            var contractorId = Guid.NewGuid().ToString();
            //// Arange
            var drillRigs = new List<DrillRig>
            {
                new DrillRig {Id = Guid.NewGuid().ToString(), Name = "SomeRigName", 
                    Model = "SomeRigModel", Number = "SomeRigNumber", ContractorId = contractorId},
                new DrillRig {Id = Guid.NewGuid().ToString(), Name = "SomeRigName1", 
                    Model = "SomeRigModel1", Number = "SomeRigNumber1", ContractorId = contractorId},
                new DrillRig {Id = Guid.NewGuid().ToString(), Name = "SomeRigName2", 
                    Model = "SomeRigModel2", Number = "SomeRigNumber2", ContractorId = contractorId},
            };

            var contractor = new Contractor
            {
                Id = contractorId,
                Name = "SomeRandomName",
                DrillRigs = drillRigs,
                IsDeleted = false,
            };

            AutoMapperConfig.RegisterMappings(
                typeof(DrillRigViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;

            var context = new TargetHoundContext(options);
            context.Contractors.Add(contractor);
            context.DrillRigs.AddRange(drillRigs);
            await context.SaveChangesAsync();
            var contractorService = new ContractorService(context);

            //// Act
            var drilRigsCollection = await contractorService.GetDrillRigsAsync<DrillRigViewModel>(contractor.Id);
            var drillRigsResult = new DrillRigViewModel[drillRigs.Count];
            drilRigsCollection.CopyTo(drillRigsResult, 0);

            //// Assert
            Assert.AreEqual(drillRigs[0].Id, drillRigsResult[0].Id);
            Assert.AreEqual(drillRigs[1].Name, drillRigsResult[1].Name);
            Assert.AreEqual(typeof(DrillRigViewModel), drillRigsResult[0].GetType());
        }
    }
}
