﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MongoApi.Configurations;
using MongoApi.Hub;
using MongoApi.Models;
using MongoDB.Driver;

namespace MongoApi.Services
{
    public class DriverService
    {
        private readonly IHubContext<MessageHub> _context;
        private readonly IMongoCollection<Driver> _drivers;
        public DriverService(IOptions<DatabaseSettings> settings, IHubContext<MessageHub> context)
        {
            _context = context;
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _drivers = database.GetCollection<Driver>(settings.Value.CollectionName);
            _context = context;
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

        internal async Task<bool> UpdateDriver(Driver driver)
        {
            try
            {
                await _drivers.ReplaceOneAsync(d => d.Id == driver.Id, driver);
                await _context.Clients.All.SendAsync("DriverUpdated", driver);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal async Task<bool> DeleteDriver(string id)
        {
            try
            {
                await _drivers.DeleteOneAsync(d => d.Id == id);
                await _context.Clients.All.SendAsync("DriverDeleted", id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
