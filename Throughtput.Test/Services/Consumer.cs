using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magnum.Extensions;
using MassTransit;
using log4net;

namespace Throughtput.Test.Services
{
    class Consumer : Consumes<Message>.All, Autofac.IStartable
    {
        private static ILog _log = LogManager.GetLogger(typeof(Consumer));
        static int _count = 0;

        public void Consume(Message message)
        {
            Interlocked.Increment(ref _count);
        }

        private Timer _timer;
        public void Start()
        {
            _log.Info("Starting Consumer");
            _timer = new Timer(_ => _log.Info("Received: " + _count), null, 30.Seconds(), 30.Seconds());
        }
    }
}
