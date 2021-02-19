using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopBot.Data;

[assembly: HostingStartup(typeof(ShopBot.Areas.Identity.IdentityHostingStartup))]
namespace ShopBot.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AuthDbContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("AuthDbContextConnection"), new MySqlServerVersion(new Version(8, 0, 21))));

                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                   // .AddEntityFrameworkStores<AuthDbContext>();
            });
        }
    }
}