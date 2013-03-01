using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MassTransit;

namespace Throughtput.Test
{
    class Module : Autofac.Module
    {
        public int BatchSize { get; set; }
        public string QueueAddress { get; set; }
 
        protected override void Load(ContainerBuilder builder)
        {
            if( Program.Role == Program.InstanceRole.Publisher)
                builder
                    .Register(c => new Services.Publisher(c.Resolve<IServiceBus>(), BatchSize))
                    .As<IStartable>()
                    .SingleInstance();
            else
                builder
                    .Register(c => new Services.Consumer())
                    .AsSelf()
                    .As<IStartable>()
                    .SingleInstance();

            builder
                .Register(c => ServiceBusFactory.New(sbc =>
                {
                    var scope = c.Resolve<ILifetimeScope>();
                    sbc.UseRabbitMq();
                    sbc.UseRabbitMqRouting();
                    sbc.SetPurgeOnStartup(true);
                    sbc.ReceiveFrom(string.Format(QueueAddress, Program.Role));

                    sbc.Subscribe(cfg => cfg.LoadFrom(scope));
                    sbc.UseControlBus();
                }))
                .SingleInstance();
        }

    }
}
