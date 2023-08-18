using DomainLibrary;
using Microsoft.Extensions.Options;
using MongoApi.Configurations;
using MongoDB.Driver;

namespace MongoApi.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IOptions<DatabaseSettings> settings)
        {
            var database = new MongoClient(settings.Value.ConnectionString).GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<Product>("products");
        }

        public async Task<ICollection<Product>> GetProducts() => await _products.Find(_ => true).ToListAsync();

        internal async Task<bool> CreateProduct(Product product)
        {
            try
            {
                await _products.InsertOneAsync(product);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                await _products.ReplaceOneAsync(x => x.Id == product.Id, product);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal async Task<bool> DeleteProduct(int id)
        {
            try
            {
                await _products.DeleteOneAsync(x => x.Id == id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
