using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Magnum.Extensions;
using MassTransit;
using Topshelf;
using log4net.Config;

namespace Throughput.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var container = BoostrapContainer();

            var h = HostFactory.New(x =>
            {
                x.Service<IStartable>(s =>
                {
                    s.ConstructUsing(name => container.Resolve<IStartable>());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(_ =>
                    {
                        Console.WriteLine("AfterStoppingServices action invoked, services are stopping");
                        container.Dispose();
                    });
                });
                x.RunAsLocalSystem();
            });

            h.Run();
        }

        private static IContainer BoostrapContainer()
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule(new ConfigurationSettingsReader("autofac"));
            return cb.Build();
        }
    }
}
