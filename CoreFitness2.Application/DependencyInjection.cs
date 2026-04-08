using CoreFitness2.Application.Interfaces;
using CoreFitness2.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness2.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IGymClassService, GymClassService>();
        services.AddScoped<IMemberService, MemberService>();
        return services;
    }
}