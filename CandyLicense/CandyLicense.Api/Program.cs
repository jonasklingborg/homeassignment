using CandyLicense.Api.Application.Commands;
using CandyLicense.Api.Data;
using CandyLicense.Api.Endpoints;
using CandyLicense.Api.Services;
using CandyLicense.Api.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

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
            builder.Services.AddScoped<IValidator<AddLicense.Command>, AddLicenseValidator>();
            builder.Services.AddScoped<IValidator<CreateLicenseRental.Command>, CreateLicenseRentalValidator>();
            builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining<Program>());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHostedService<CandyLicenseCleanupService>();

            var app = builder.Build();

            LicenseEndpoint.SetupRoute(app);

            if (app.Environment.IsDevelopment())
            // Configure the HTTP request pipeline.
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // TODO: Put in some class
            // Setup global handling of FluentValidation 
            app.UseExceptionHandler(appError => appError.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (exceptionHandlerPathFeature?.Error is ValidationException ex)
                    {
                        context.Response.StatusCode = 422;
                        await context.Response.WriteAsJsonAsync(ex.Errors.Select(x => x.ErrorMessage));
                    }
                }
            ));

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();

        }
    }
}