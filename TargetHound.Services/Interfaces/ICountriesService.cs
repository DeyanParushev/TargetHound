namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesService
    {
        public  Task<ICollection<T>> GetAllCountriesAsync<T>();

        public Task<T> GetCountryByIdAsync<T>(int countryId);
    }
}
