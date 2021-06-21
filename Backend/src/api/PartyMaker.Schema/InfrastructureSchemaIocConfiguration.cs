using Microsoft.Extensions.DependencyInjection;
using System;

namespace PartyMaker.Schema
{
    public static class InfrastructureSchemaIocConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISchemaProvider, SchemaProvider>();
        }

        public static void Configure(IServiceProvider app)
        {
            app.GetService<ISchemaProvider>().Initialize();
        }
    }
}
