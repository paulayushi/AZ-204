using SampleApplication.Models;

namespace SampleApplication.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
        Task<bool> GetBetaVersion();
    }
}
