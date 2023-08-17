using Microsoft.Extensions.Options;
using MongoApi.Configurations;
using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.Services
{
    public class DriverService
    {
        private readonly IMongoCollection<Driver> _drivers;
        public DriverService(IOptions<DatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _drivers = database.GetCollection<Driver>(settings.Value.CollectionName);
        }

        public async Task<ICollection<Driver>> GetDrivers() => await _drivers.Find(_ => true).ToListAsync();

        internal async Task<bool> CreateDriver(Driver driver)
        {
            try
            {
                await _drivers.InsertOneAsync(driver);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
