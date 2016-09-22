using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSFunctionGenerator
{
    public class Writer
    {
        private FunctionGenerator _generator;

        IWebSocketConnection _socket;

        private bool _running;

        public Writer(IWebSocketConnection socket, FunctionGenerator generator, int updateRate)
        {
            _socket = socket;
            _generator = generator;
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;

            var task = new Task(() =>
            {
                while (_running)
                {
                    var value = _generator.GetValue();
                    _socket.Send("{ 'Value': '" + value + "' }");
                    Thread.Sleep(50);
                }
            }, TaskCreationOptions.LongRunning);
            task.Start();
        }

        public void Stop()
        {
            _running = false;
        }
    }
}
