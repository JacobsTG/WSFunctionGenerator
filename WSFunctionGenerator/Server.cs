using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSFunctionGenerator
{
    public class Server
    {
        private string _address;

        private WebSocketServer _server;

        private List<IWebSocketConnection> _allSockets = new List<IWebSocketConnection>();

        private Dictionary<Guid, Writer> _activeWriters = new Dictionary<Guid, Writer>();

        public Server(string address)
        {
            _address = address;
        }

        public void Start()
        {
            _server = new WebSocketServer(_address);
            _server.SupportedSubProtocols = new[] { "wsfg.jacobs.com/1.0" };
            _server.Start(socket =>
            {
                socket.OnOpen = () => OnOpen(socket);
                socket.OnClose = () => OnClose(socket);
                socket.OnMessage = message =>
                {
                    OnMessage(socket, message);
                };
            });
        }

        private void OnOpen(IWebSocketConnection socket)
        {
            _allSockets.Add(socket);
        }

        private void OnMessage(IWebSocketConnection socket, string message)
        {
            var command = JsonConvert.DeserializeObject<Command>(message);

            if (command == null)
                return;

            switch (command.Action.ToUpper())
            {
                case "START":
                    StartFunction(socket, command.FunctionType);
                    break;

                case "STOP":
                    StopFunction(socket);
                    break;

                default:
                    break;
            }
        }

        private void OnClose(IWebSocketConnection socket)
        {
            _allSockets.Remove(socket);
        }

        private void StartFunction(IWebSocketConnection socket, string functionType)
        {
            FunctionType type;

            // Stop any running writer
            StopFunction(socket);

            // Get the function type
            switch (functionType.ToUpper())
            {
                case "RANDOM":
                    type = FunctionType.Random;
                    break;
                case "SAWTOOTH":
                    type = FunctionType.Sawtooth;
                    break;
                case "SINE":
                    type = FunctionType.Sine;
                    break;
                case "SQUARE":
                    type = FunctionType.Square;
                    break;
                case "TRIANGLE":
                    type = FunctionType.Triangle;
                    break;
                default:
                    return;
            }

            // Create the function generator
            var generator = new FunctionGenerator(type);

            // Start a thread to write out the function's value periodically
            var writer = new Writer(socket, generator, 50);
            writer.Start();

            _activeWriters.Add(socket.ConnectionInfo.Id, writer);
        }

        private void StopFunction(IWebSocketConnection socket)
        {
            // Stop the writer for this socket if one is already running
            var id = socket.ConnectionInfo.Id;
            if (_activeWriters.ContainsKey(id))
            {
                var w = _activeWriters[id];
                w.Stop();
                _activeWriters.Remove(id);
            }
        }
    }
}
