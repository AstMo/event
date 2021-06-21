using FluentMigrator.Runner;
using bgTeam;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using PartyMaker.Configuration.Interfaces;

namespace PartyMaker.Schema
{
    public class SchemaProvider : ISchemaProvider
    {
        private readonly IDatabaseSettings _databaseSettings;
        private readonly IAppLogger _logger;

        public SchemaProvider(IDatabaseSettings databaseSettings, IAppLogger logger)
        {
            _databaseSettings = databaseSettings;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.Debug($"Running migration for {_databaseSettings.ConnectionString}");
            var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(o =>
                    o.AddPostgres()
                        .WithGlobalConnectionString(_databaseSettings.ConnectionString)
                        .ScanIn(typeof(ISchemaProvider).Assembly)
                        .For.Migrations())
                .BuildServiceProvider(false);

            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            using (serviceProvider.CreateScope())
            {
                runner.MigrateUp();
            }
        }
    }
}
