namespace TargetHound.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TargetHound.Data;
    using TargetHound.Services.Automapper;
    using TargetHound.Services.Interfaces;

    public class CountriesService : ICountriesService
    {
        private readonly TargetHoundContext dbContext;

        public CountriesService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<T>> GetAllCountriesAsync<T>()
        {
            var countries = this.dbContext
                .Countries
                .To<T>()
                .ToList();

            return countries;
        }

        public async Task<T> GetCountryByIdAsync<T>(int countryId)
        {
            if(!this.dbContext.Countries.Any(x => x.Id == countryId))
            {
                throw new ArgumentOutOfRangeException();
            }

            var country = this.dbContext
                .Countries
                .Where(x => x.Id == countryId)
                .To<T>()
                .ToList()[0];

            return country;
        }
    }
}
