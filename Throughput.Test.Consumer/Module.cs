using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MassTransit;

namespace Throughput.Test.Consumer
{
    class Module : Autofac.Module
    {
        public int BatchSize { get; set; }
        public string QueueAddress { get; set; }
 
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => new Services.Consumer1(c.Resolve<IServiceBus>()))
                .AsSelf()
                .As<IStartable>()
                .SingleInstance();

            builder
                .Register(c => new Services.Consumer2(c.Resolve<IServiceBus>()))
                .AsSelf()
                .As<IStartable>()
                .SingleInstance();

            builder
                .Register(c => ServiceBusFactory.New(sbc =>
                {
                    var scope = c.Resolve<ILifetimeScope>();
                    sbc.UseRabbitMq();
                    sbc.SetPurgeOnStartup(true);
                    sbc.ReceiveFrom(string.Format(QueueAddress, "consumer"));

                    sbc.Subscribe(cfg => cfg.LoadFrom(scope));
                }))
                .As<IServiceBus>()
                .SingleInstance();
        }

    }
}
