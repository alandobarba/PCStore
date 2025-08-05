using P_CStore.Stripe;
using Stripe;

namespace P_CStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
