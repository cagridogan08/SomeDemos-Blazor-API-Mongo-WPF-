using BlazorUIMongo.Data;
using BlazorUIMongo.Services;
using BlazorUIMongo.ViewModels;
using DomainLibrary;

namespace BlazorUIMongo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddHttpClient<IService<Driver>, DriverService>((client =>
            {
                client.BaseAddress = new Uri(uriString: "http://localhost:5041/");
            }));
            builder.Services.AddHttpClient<IService<Product>, ProductService>((client =>
            {
                client.BaseAddress = new Uri(uriString: "http://localhost:5041/");
            }));
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<ProductsViewModel>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}