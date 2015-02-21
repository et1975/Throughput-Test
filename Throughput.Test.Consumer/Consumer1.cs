using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magnum.Extensions;
using MassTransit;
using log4net;

namespace Throughput.Test.Services
{
    class Consumer1 : Consumes<IMessage1>.All, Autofac.IStartable
    {
        public Consumer1(IServiceBus bus)
        {
        }

        private static ILog _log = LogManager.GetLogger(typeof(Consumer1));
        static int _count = 0;

        public void Consume(IMessage1 message)
        {
            Interlocked.Increment(ref _count);
        }

        private Timer _timer;
        public void Start()
        {
            _log.Info("Starting Consumer1");
            _timer = new Timer(_ => _log.Info("Received 1s: " + _count), null, 30.Seconds(), 30.Seconds());
        }
    }
}
