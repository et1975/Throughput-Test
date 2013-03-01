using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using MassTransit;
using Topshelf;
using log4net.Config;

namespace Throughtput.Test
{
    class Program
    {
        public enum InstanceRole
        {
            Publisher,
            Consumer
        }

        public static InstanceRole Role { get; private set; }

        static void Main(string[] args)
        {
            Role = args.Select(a => a.ToLower()).Contains("consumer")
                       ? InstanceRole.Consumer
                       : InstanceRole.Publisher;

            XmlConfigurator.Configure();

            var container = BoostrapContainer();

            var h = HostFactory.New(x =>
            {
                x.AfterStoppingServices(n => Console.WriteLine("AfterStoppingServices action invoked, services are stopping"));

                x.Service<IStartable>(s =>
                {
                    s.SetServiceName("Throughtput.Test");
                    s.ConstructUsing(name => container.Resolve<IStartable>());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(_ => container.Dispose());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Test " + Role);
                x.SetDisplayName("Test" + Role);
                x.SetServiceName("Test" + Role);
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
