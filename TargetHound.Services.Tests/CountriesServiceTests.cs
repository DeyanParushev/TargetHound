namespace TargetHound.Services.Tests
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;
    using TargetHound.SharedViewModels.ViewModels;

    public class CountriesServiceTests
    {
        [TestCase]
        public async Task GetAllCountriesShouldReturnCollectionOfViewModels()
        {
            //// Setup
            var data = new List<Country>
            {
                new Country { Id = 5, Name = "BBB" },
                new Country { Id = 10, Name = "ZZZ" },
                new Country { Id = 15, Name = "AAA" },
            };

            AutoMapperConfig.RegisterMappings(
                typeof(CountryViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;

            var context = new TargetHoundContext(options);
            context.Countries.AddRange(data);
            context.SaveChanges();
            CountriesService countriesService = new CountriesService(context);

            //// Act
            var countries = await countriesService.GetAllCountriesAsync<CountryViewModel>();
            var countriesList = new CountryViewModel[data.Count];
            countries.CopyTo(countriesList, 0);

            //// Assert
            Assert.AreEqual(3, countries.Count);
            Assert.AreEqual(data[0].Name, countriesList[0].Name);
            Assert.AreEqual(data[2].Id, countriesList[2].Id);
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        public async Task GetCountryByIdShouldReturnViewModel(int countryId)
        {
            //// Setup
            var countryData = new Country { Id = countryId, Name = "Bahamas" };
            AutoMapperConfig.RegisterMappings(
                typeof(CountryViewModel).GetTypeInfo().Assembly);
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;
            
            var context = new TargetHoundContext(options);
            context.Countries.Add(countryData);
            context.SaveChanges();
            CountriesService countriesService = new CountriesService(context);

            //// Act
            var country = await countriesService.GetCountryByIdAsync<CountryViewModel>(countryId);

            //// Assert
            Assert.AreEqual(countryData.Name, country.Name);
            Assert.AreEqual(countryData.Id, country.Id);
        }

        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        public async Task GetCountryByIdShouldThrowException(int countryId)
        {
            //// Setup
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
                .UseInMemoryDatabase(databaseName: "FakeConnectionString")
                .Options;

            var context = new TargetHoundContext(options);
            CountriesService countriesService = new CountriesService(context);

            //// Assert
            Assert.That(
                    async () => await countriesService.GetCountryByIdAsync<CountryViewModel>(countryId),
                    Throws.Exception);
        }
    }
}
