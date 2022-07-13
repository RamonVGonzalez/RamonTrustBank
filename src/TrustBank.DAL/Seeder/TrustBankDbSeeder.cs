using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Authorization;
using TrustBank.Infrastructure.Data;

namespace TrustBank.Infrastructure.Seeder
{
    public class TrustBankDbSeeder
    {
        static string jsonFilesFolder = Path.GetFullPath(@"../TrustBank.DAL/JsonFiles/");
        //static string jsonFilesFolder = Path.GetFullPath(@"C:\Users\RAMON\Documents\PROGRAMMING FOLDER\Projects\RamonTrustBank\src\TrustBank.DAL\JsonFiles");
        //static string jsonFilesFolder = Path.GetFullPath(@"C:\Users\RAMON\Documents\PROGRAMMING FOLDER\Projects\RamonTrustBank\src\TrustBank.DAL\JsonFiles");

        public static async Task SeedDataBase(IApplicationBuilder app)
        {

            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();


            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (context.Users.Any())
            {
                return;
            }
            else
            {
                var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<Customer>>();

                var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await SeedRoles(context, roleManager);
                await SeedProduct(context);
                //await SeedCustomer(applicationDbContext);
                //await SeedAddress(applicationDbContext);
                //await SeedAccount(applicationDbContext);
            }

        }


        private static async Task SeedRoles(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            var roles = new string[] { Role.Admin, Role.User };
            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedProduct(ApplicationDbContext context)
        {
            var productList = GetSampleData<Product>("Product.Json");

            await context.AddRangeAsync(productList);
            await context.SaveChangesAsync();
        }


        private static async Task SeedCustomer(ApplicationDbContext context)
        {
            var customerStream = await File.ReadAllTextAsync
                (jsonFilesFolder + "Customer.json");
            var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(customerStream);
            await context.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedAddress(ApplicationDbContext context)
        {
            var addressStream = await File.ReadAllTextAsync
                (jsonFilesFolder + "Address.json");
            var addresses = JsonSerializer.Deserialize<IEnumerable<Address>>(addressStream);
            await context.AddRangeAsync(addresses);
            await context.SaveChangesAsync();
        }

        private static async Task SeedAccount(ApplicationDbContext context)
        {
            var accountStream = await File.ReadAllTextAsync
                (jsonFilesFolder + "Address.json");
            var accounts = JsonSerializer.Deserialize<IEnumerable<Account>>(accountStream);
            await context.AddRangeAsync(accounts);
            await context.SaveChangesAsync();
        }

        private static List<T> GetSampleData<T>(string fileName)
        {
            var file = File.ReadAllText(jsonFilesFolder+ fileName);
            var output = JsonSerializer.Deserialize<List<T>>(file, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return output;
        }

    }
}
