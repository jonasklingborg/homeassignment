﻿using CandyLicense.Api.Application.Commands;
using CandyLicense.Api.Application.Queries;
using CandyLicense.Api.Requests;
using CandyLicense.Api.Responses;
using MediatR;

namespace CandyLicense.Api.Endpoints
{
    public class LicenseEndpoint
    {
        public static void SetupRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("api/license", AddLicense)
                .Produces(200)
                .Produces<string[]>(422)
                .WithName("AddLicense");

            app.MapGet("api/license", GetLicenses)
                .Produces<IEnumerable<GetLicenseResponse>>()
                .WithName("GetLicenses");

            app.MapPost("api/rental", CreateRental)
                .Produces<CreateRentalResponse>()
                .Produces<string[]>(422)
                .WithName("RentLicense");

            app.MapGet("api/rental", GetRental)
                .Produces<GetRentalResponse>()
                .WithName("GetRental");
        }

        private static async Task<IResult> AddLicense(IMediator mediator, AddLicenseRequest request)
        {
            await mediator.Send(new AddLicense.Command(request.Name));

            return Results.Ok();
        }

        private static async Task<IResult> GetLicenses(IMediator mediator)
        {
            var result = await mediator.Send(new GetAllLicenses.Query());

            return Results.Ok(result);
        }

        private static async Task<IResult> CreateRental(IMediator mediator) 
        {
            // TODO: Fix real login and authentication
            //var user = httpContext.User.Identity?.Name;
            var user = "Jonas Klingborg";

            var result = await mediator.Send(new CreateLicenseRental.Command(user));

            return result != null ? Results.Ok(result) : Results.NotFound();
        }

        private static async Task<IResult> GetRental(IMediator mediator)
        {
            // TODO: Fix real login and authentication
            //var user = httpContext.User.Identity?.Name;
            var user = "Jonas Klingborg";

            var result = await mediator.Send(new GetRental.Query(user));

            return result != null ? Results.Ok(result) : Results.NotFound();
        }
    }
}
