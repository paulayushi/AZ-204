using Microsoft.Data.SqlClient;
using Microsoft.FeatureManagement;
using SampleApplication.Models;

namespace SampleApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _featureManager;

        public ProductService(IConfiguration configuration, IFeatureManager featureManager)
        {
            _configuration = configuration;
            _featureManager = featureManager;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SqlConnStrings"));
        }
        public async Task<List<Product>> GetProducts()
        {
            var products = new List<Product>();
            //Azure Fn calls
            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://productfncapp.azurewebsites.net/api/");
                products = await httpClient.GetFromJsonAsync<List<Product>>("GetProducts?code=6PGuw6FC7LrUEX834WeRwfWK-IUNFT2ZGObZIYS8x2JlAzFuCwaVQA==");
            }
            //var sqlStatement = "SELECT ProductId, ProductName, Quantity FROM Products";
            //var conn = GetConnection();
            //var command = new SqlCommand(sqlStatement, conn);
            //conn.Open();

            //using (SqlDataReader reader = await command.ExecuteReaderAsync())
            //{
            //    while (reader.Read())
            //    {
            //        var product = new Product
            //        {
            //            ProductId = reader.GetInt32(0),
            //            ProductName = reader.GetString(1),
            //            Quantity = reader.GetInt32(2)
            //        };

            //        products.Add(product);
            //    }
            //};

            //conn.Close();

            return products;
        }

        public async Task<bool> GetBetaVersion()
        {
            return await _featureManager.IsEnabledAsync("beta");
        }
    }
}
