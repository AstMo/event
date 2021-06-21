using bgTeam;
using bgTeam.Core;
using bgTeam.Core.Impl;
using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl;
using bgTeam.DataAccess.Impl.Dapper;
using bgTeam.DataAccess.Impl.PostgreSQL;
using bgTeam.Impl;
using bgTeam.Impl.Rabbit;
using bgTeam.Impl.Serilog;
using bgTeam.Queues;
using PartyMaker.Application.Translator;
using PartyMaker.Application.Validator;
using PartyMaker.Common;
using PartyMaker.Common.Dapper;
using PartyMaker.Common.Impl;
using PartyMaker.Common.ServiceWaiter;
using PartyMaker.Common.Translator;
using PartyMaker.Common.Validation;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.Schema;
using PartyMaker.Story;
using Dapper;
using DapperExtensions;
using IdmClinic.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartyMaker.Common.Approver;
using PartyMaker.Application.Approvers;
using PartyMaker.Common.Email;
using PartyMaker.Application.Service.EmailSender;

namespace PartyMaker.Application.Service
{
    public static class AppIocConfigure
    {
        public static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Scan(scan => scan
                     .FromAssemblyOf<IStoryLibrary>()
                     .AddClasses(classes => classes.AssignableTo(typeof(IStory<,>)))
                     .AsImplementedInterfaces()
                     .WithTransientLifetime());

            services.Scan(scan => scan
                .FromAssemblyOf<IQueryLibrary>()
                .AddClasses(classes => classes.AssignableTo(typeof(IQuery<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            var config = new AppConfigurationDefault();

            services
                .AddSingleton(services)
                .AddSingleton<IAppConfiguration>(config)
                .AddSingleton<IAppSettings, AppSettings>()
                .AddSingleton<IDatabaseSettings>(x => x.GetService<IAppSettings>().DatabaseSettings)
                .AddSingleton<IImageStoreSetting>(x => x.GetService<IAppSettings>().ImageStoreSettings)
                .AddSingleton<IWebAppConfigSettings>(x => x.GetService<IAppSettings>().WebAppConfigSettings)
                .AddSingleton<IConnectionSetting>(x => x.GetService<IAppSettings>().DatabaseSettings)
                .AddSingleton<IQueuesSettings>(x => x.GetService<IAppSettings>().QueuesSettings)
                .AddSingleton<IQueueProviderSettings>(x => x.GetService<IAppSettings>().QueueProviderSettings)
                .AddSingleton<IAuthenticationSettings>(x => x.GetService<IAppSettings>().AuthenticationSettings)
                .AddSingleton<IAppLoggerConfig, AppSettings>()
                .AddSingleton<IAppLogger, AppLoggerSerilog>()
                .AddSingleton<IStoryFactory, StoryFactory>()
                .AddSingleton<IStoryBuilder, StoryBuilder>()
                .AddSingleton<IQueryFactory, QueryFactory>()
                .AddSingleton<IQueryBuilder, QueryBuilder>()
                .AddSingleton<bgTeam.DataAccess.ISqlDialect, SqlDialectDapper>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IConnectionFactory, ConnectionFactoryPostgreSQL>()
                .AddSingleton<ICrudService, CrudServiceDapper>()
                .AddSingleton<IRepository, RepositoryDapper>()
                .AddSingleton<ITranslatorFactory, WebAppTranslatorsFactory>()
                .AddSingleton<IWebAppValidatorFactory, WebAppValidatorFactory>()
                .AddSingleton<IWebApproverFactory, WebAppApproverFactory>()
                .AddSingleton<RabbitMQ.Client.IConnectionFactory, ConnectionFactoryRabbitMQ>()
                .AddSingleton<IMessageProvider, MessageProviderDefault>()
                .AddSingleton<IWebAppEmailSenderService, EmailSenderService>();

            InfrastructureSchemaIocConfiguration.ConfigureServices(services);
            CommonIocConfigure.ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IRabbitServiceWaiter>().Wait();
            app.ApplicationServices.GetService<IDatabaseServiceWaiter>().Wait();
            app.ApplicationServices.GetService<ITranslatorFactory>().Initialize();
            app.ApplicationServices.GetService<IWebAppValidatorFactory>().Initialize();
            app.ApplicationServices.GetService<IWebApproverFactory>().Initialize();
            app.ApplicationServices.GetService<IConnectionFactory>();


            DapperHelper.SqlDialect = new PostgreSqlDialect();
            SqlMapper.AddTypeHandler(new DateTimeHandler());
            InfrastructureSchemaIocConfiguration.Configure(app.ApplicationServices);
        }
    }
}
