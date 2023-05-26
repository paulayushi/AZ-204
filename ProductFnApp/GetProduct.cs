using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Domain;
using Microsoft.Data.SqlClient;
using System;

namespace ProductFnApp
{
    public static class GetProduct
    {
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
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
            return new OkObjectResult(products);
        }

        [FunctionName("GetProductById")]
        public static async Task<IActionResult> GetProductById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var productId = req.Query["ProductId"];
            var product = new Product();
            var sqlStatement = $"SELECT ProductId, ProductName, Quantity FROM Products where ProductId = {productId}";
            var conn = GetConnection();
            var command = new SqlCommand(sqlStatement, conn);
            conn.Open();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Quantity = reader.GetInt32(2)
                    };
                }
            };

            conn.Close();
            return new OkObjectResult(product);
        }

        public static SqlConnection GetConnection()
        {
            var connString = Environment.GetEnvironmentVariable("SqlConnStrings");
            return new SqlConnection(connString);
        }
    }
}
