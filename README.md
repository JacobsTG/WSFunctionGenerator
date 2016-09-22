# WSFunctionGenerator

WSFunctionGenerator is a C# console application designed to generate periodic function values via a WebSocket.

 - Random, Sawtooth, Sine, Square, and Triangle functions are supported
 - Connect using ws://localhost:50000/
 - Simple JSON commands, for example:
	 - Start a sine wave: { 'Action': 'Start', 'FunctionType': 'Sine' }
	 - Start a random number generator: { 'Action': 'Start', 'FunctionType': 'Random' }
	 - Stop the data stream: { 'Action': 'Stop' }
 - Simple JSON format for each sample:
	 - { 'Value': '0.1616425' }