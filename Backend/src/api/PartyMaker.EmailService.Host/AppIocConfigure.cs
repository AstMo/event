using bgTeam;
using bgTeam.Core;
using bgTeam.Impl;
using bgTeam.Impl.Rabbit;
using bgTeam.Impl.Serilog;
using bgTeam.Queues;
using Microsoft.Extensions.DependencyInjection;
using PartyMaker.Common;
using PartyMaker.Common.Impl;
using PartyMaker.Common.ServiceWaiter;
using PartyMaker.Configuration.Interfaces;
using PartyMaker.EmailService.Common.Services;
using PartyMaker.EmailService.Host;
using PartyMaker.Story;
using System;

namespace PartyMaker.EmailServiceHost
{
    public static class AppIocConfigure
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // сканируем story
            services.Scan(scan => scan
                    .FromAssemblyOf<IStoryLibrary>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IStory<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services
                .AddSingleton<IAppLoggerConfig, AppSettings>()
                .AddSingleton<IAppLogger, AppLoggerSerilog>()
                .AddSingleton<IStoryFactory, StoryFactory>()
                .AddSingleton<IStoryBuilder, StoryBuilder>()
                .AddSingleton<IAppSettings, AppSettings>()
                .AddSingleton<IEmailServiceSettings>(x => x.GetService<IAppSettings>().EmailSettings)
                .AddSingleton<EmailEventListener, EmailEventListener>()
                .AddSingleton<IQueueProviderSettings>(x => x.GetService<IAppSettings>().QueueProviderSettings)
                .AddSingleton<IQueuesSettings>(x => x.GetService<IAppSettings>().QueuesSettings)
                .AddSingleton<IEmailSenderService, EmailSenderService>()
                .AddSingleton<IMimeMap, MimeMap>()
                .AddSingleton<ISmtpClientPool, SmtpClientPool>()
                .AddSingleton<ITemplateService, TemplateService>()
                .AddTransient<IHttpCommunicator, HttpCommunicator>()

                .AddSingleton<RabbitMQ.Client.IConnectionFactory, ConnectionFactoryRabbitMQ>()
                .AddSingleton<IMessageProvider, MessageProviderDefault>();

            CommonIocConfigure.ConfigureServices(services);
        }

        public static void Configure(IServiceProvider serviceProvider)
        {
            serviceProvider.GetService<IRabbitServiceWaiter>().Wait();
        }
    }
}
