namespace TargetHound.Services
{
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

        public ICollection<T> GetAllCountriesAsync<T>()
        {
            return this.dbContext
                .Countries
                .To<T>()
                .ToList();
        }
    }
}
