
using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Implemantations;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Implementations;
using ePizzaHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ePizzaHub.Services
{
    public class ConfigureDependencies
    {
        public static void RegisterService(IServiceCollection services, IConfiguration configuration)
        {
            //db
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });
            // repositories
            services.AddScoped<IRepository<Item>, Repository<Item>>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IRepository<CartItem>, Repository<CartItem>>();
            services.AddScoped<IUserRepository  , UserRepository>();
            //services
           // services.AddScoped<IService<Item>, Service<Item>>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<ICartService, CartService>();
        }
    }
}
