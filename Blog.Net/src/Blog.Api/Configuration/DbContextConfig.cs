using Blog.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionSqLite") ?? throw new InvalidOperationException("Connection string 'DefaultConnectionSqLite' not found.");

                builder.Services.AddDbContext<MeuDbContext>(options =>
                    options.UseSqlite(connectionString));

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(connectionString));

                return builder;
            }
            else
            {

                builder.Services.AddDbContext<MeuDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                return builder;
            }

        }
    }
}
