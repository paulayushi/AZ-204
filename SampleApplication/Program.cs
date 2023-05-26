using SampleApplication.Services;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
//var azConnection = "Endpoint=https://appsettings1005.azconfig.io;Id=1Cqh;Secret=xUW0vqyzg7wiAcfhnFv+xzLrT5Jclhf+llR1EsG5kCU=";

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Host.ConfigureAppConfiguration(configBuilder =>
//{
//    configBuilder.AddAzureAppConfiguration(options =>
//    {
//        options.Connect(azConnection).UseFeatureFlags();
//    });
//});
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddFeatureManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
