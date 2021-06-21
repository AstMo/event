using Microsoft.Extensions.Hosting;
using PartyMaker.EmailServiceHost;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PartyMaker.EmailService.Host
{
    public class EventListenerHost : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private EmailEventListener _eventListener;

        public EventListenerHost(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AppIocConfigure.Configure(_serviceProvider);
            _eventListener = (EmailEventListener)_serviceProvider.GetService(typeof(EmailEventListener));
            _eventListener.StartListen();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventListener?.StopListen();
            return Task.CompletedTask;
        }
    }
}
