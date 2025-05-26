
using ExpenseManagerAPI.Data;
using ExpenseManagerAPI.Model;
using ExpenseManagerAPI.Repository;

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExpenseManagerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped<EventRepository>();
            builder.Services.AddScoped<TransactionRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();


            builder.Services.AddDbContext<AppDbContext>(e => e.UseAzureSql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });



            //builder.Services.AddControllers().AddFluentValidation(fv =>
            //{
            //    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //});

            // builder.Services.AddValidatorsFromAssemblyContaining<EventValidation>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.WebHost.UseUrls("http://*:5033", "https://*:5034");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
