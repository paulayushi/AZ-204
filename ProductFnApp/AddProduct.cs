using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Data;

namespace ProductFnApp
{
    public static class AddProduct
    {
        [FunctionName("AddProduct")]
        public static async Task<IActionResult> AddProducts(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            SqlConnection conn = null;
            try
            {
                var data = new StreamReader(req.Body).ReadToEndAsync().Result;
                var product = JsonConvert.DeserializeObject<Product>(data);
                var sqlStatement = $"INSERT INTO Products(ProductId, ProductName, Quantity) VALUES(@param1,@param2,@param3) ";
                conn = GetProduct.GetConnection();
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(sqlStatement, conn))
                {
                    cmd.Parameters.Add("@param1", SqlDbType.Int).Value = product.ProductId;
                    cmd.Parameters.Add("@param2", SqlDbType.VarChar, 1000).Value = product.ProductName;
                    cmd.Parameters.Add("@param3", SqlDbType.Int).Value = product.Quantity;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                };

                conn.Close();
                return new OkObjectResult(product);
            }
            catch (Exception ex)
            {
                if(conn != null)
                    conn.Close();
                throw ex;
            }
            
        }
    }
}
