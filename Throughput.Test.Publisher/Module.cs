using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MassTransit;

namespace Throughput.Test.Publisher
{
    class Module : Autofac.Module
    {
        public int BatchSize { get; set; }
        public string QueueAddress { get; set; }
 
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(c => new Services.Publisher(c.Resolve<IServiceBus>(), BatchSize))
                .As<IStartable>()
                .SingleInstance();

            builder
                .Register(c => ServiceBusFactory.New(sbc =>
                {
                    var scope = c.Resolve<ILifetimeScope>();
                    sbc.UseRabbitMq();
                    sbc.SetPurgeOnStartup(true);
                    sbc.ReceiveFrom(string.Format(QueueAddress, "publisher"));

                    sbc.Subscribe(cfg => cfg.LoadFrom(scope));
                }))
                .As<IServiceBus>()
                .SingleInstance();
        }

    }
}
