using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magnum.Extensions;
using MassTransit;
using log4net;
using System.Diagnostics;

namespace Throughtput.Test.Services
{
    class Publisher : Autofac.IStartable
    {
        private static ILog _log = LogManager.GetLogger(typeof (Publisher));
        private readonly IServiceBus _bus;
        private readonly int _size;

        public Publisher(IServiceBus bus, int size)
        {
            _bus = bus;
            _size = size;
        }

        public void Start()
        {
            _log.Info("Starting Publisher");

            Thread.Sleep(30.Seconds());
            var watch = Stopwatch.StartNew();
            _log.Info("Publishing "+_size+" small...");
            Enumerable.Range(1, _size)
                      .Select(_ => Message.Small)
                      .Each(_bus.Publish);
            _log.Info("Done publishing " + _size + " small: "+watch.Elapsed);

            Thread.Sleep(30.Seconds());
            watch = Stopwatch.StartNew();
            _log.Info("Publishing " + _size + " medium...");
            Enumerable.Range(1, _size)
                      .Select(_ => Message.Medium)
                      .Each(_bus.Publish);
            _log.Info("Done publishing " + _size + " medium: " + watch.Elapsed);

            Thread.Sleep(30.Seconds());
            watch = Stopwatch.StartNew();
            _log.Info("Publishing " + _size + " large...");
            Enumerable.Range(1, _size)
                      .Select(_ => Message.Large)
                      .Each(_bus.Publish);
            _log.Info("Done publishing " + _size + " large: " + watch.Elapsed);

        }
    }
}
