using CandyLicense.Api.Data;
using CandyLicense.Api.Endpoints;

namespace CandyLicense.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<CandyLicenseContext>();
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            LicenseEndpoint.SetupRoute(app);

            if (app.Environment.IsDevelopment())
            // Configure the HTTP request pipeline.
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();


        }
    }
}