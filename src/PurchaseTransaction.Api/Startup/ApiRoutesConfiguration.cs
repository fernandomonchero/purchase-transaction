namespace PurchaseTransaction.Api.Startup
{
    public static class ApiRoutesConfiguration
    {
        public static IServiceCollection ConfigureRoutesAndSwagger(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}