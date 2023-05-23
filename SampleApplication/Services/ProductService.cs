using Microsoft.Data.SqlClient;
using SampleApplication.Models;

namespace SampleApplication.Services
{
    public class ProductService : IProductService
    {
        private static readonly string _dbSource = "appserver1006.database.windows.net";
        private static readonly string _dbUserName = "superadmin";
        private static readonly string _dbPassword = "Reyansh@1987";
        private static readonly string _dbName = "appdb";
        private SqlConnection GetConnection()
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = _dbSource,
                UserID = _dbUserName,
                Password = _dbPassword,
                InitialCatalog = _dbName
            };

            return new SqlConnection(builder.ConnectionString);
        }
        public async Task<List<Product>> GetProducts()
        {
            var products = new List<Product>();
            var sqlStatement = "SELECT ProductId, ProductName, Quantity FROM Products";
            var conn = GetConnection();
            var command = new SqlCommand(sqlStatement, conn);
            conn.Open();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var product = new Product
                    {
                        ProductId = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Quantity = reader.GetInt32(2)
                    };

                    products.Add(product);
                }
            };

            conn.Close();

            return products;
        }
    }
}
