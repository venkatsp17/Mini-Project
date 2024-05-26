using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using System.Text;

namespace ShoppingAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            });

            //Debug.WriteLine(builder.Configuration["TokenKey:JWT"]);
            #region authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                    };

                });
            #endregion

            #region contexts
            builder.Services.AddDbContext<ShoppingAppContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
                );
            #endregion

            #region repos
            builder.Services.AddScoped<IRepository<int, CartItem>, CartItemRepository>();
            builder.Services.AddScoped<IRepository<int, Cart>, CartRepository>();
            builder.Services.AddScoped<IRepository<int, Category>, CategoryRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IRepository<int, OrderDetail>, OrderDetailRepository>();
            builder.Services.AddScoped<IRepository<int, Order>, OrderRepository>();
            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<int, Product>, ProductRepository>();
            builder.Services.AddScoped<IRepository<int, Refund>, RefundRepository>();
            builder.Services.AddScoped<IRepository<int, Review>, ReviewRepository>();
            builder.Services.AddScoped<IRepository<int, Seller>, SellerRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region services

            #endregion



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
