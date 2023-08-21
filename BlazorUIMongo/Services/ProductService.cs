using DomainLibrary;

namespace BlazorUIMongo.Services
{
    public class ProductService : ServiceBase<Product>
    {
        public ProductService(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}
