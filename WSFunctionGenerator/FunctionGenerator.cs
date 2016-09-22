using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSFunctionGenerator
{
    public class FunctionGenerator
    {
        private FunctionType functionType = FunctionType.Sine;
        /// <summary>
        /// Signal Type.
        /// </summary>
        public FunctionType FunctionType
        {
            get { return functionType; }
            set { functionType = value; }
        }

        private float frequency = 1f;
        /// <summary>
        /// Signal Frequency.
        /// </summary>
        public float Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        private float phase = 0f;
        /// <summary>
        /// Signal Phase.
        /// </summary>
        public float Phase
        {
            get { return phase; }
            set { phase = value; }
        }

        private float amplitude = 1f;
        /// <summary>
        /// Signal Amplitude.
        /// </summary>
        public float Amplitude
        {
            get { return amplitude; }
            set { amplitude = value; }

        }

        private float offset = 0f;
        /// <summary>
        /// Signal Offset.
        /// </summary>
        public float Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private float invert = 1; // Yes=-1, No=1
                                  /// <summary>
                                  /// Signal Inverted?
                                  /// </summary>
        public bool Invert
        {
            get { return invert == -1; }
            set { invert = value ? -1 : 1; }
        }

        /// <summary>
        /// Time the signal generator was started
        /// </summary>
        private long startTime = Stopwatch.GetTimestamp();

        /// <summary>
        /// Ticks per second on this CPU
        /// </summary>
        private long ticksPerSecond = Stopwatch.Frequency;

        /// <summary>
        /// A random number generator
        /// </summary>
        private Random random = new Random((int)Stopwatch.GetTimestamp());

        public FunctionGenerator(FunctionType initialSignalType)
        {
            functionType = initialSignalType;
        }

        public FunctionGenerator() { }

#if DEBUG
        public float GetValue(float time)
#else
    private float GetValue(float time)
#endif
        {
            float value = 0f;
            float t = frequency * time + phase;
            switch (functionType)
            {
                case FunctionType.Sine: // sin( 2 * pi * t )
                    value = (float)Math.Sin(2f * Math.PI * t);
                    break;
                case FunctionType.Square: // sign( sin( 2 * pi * t ) )
                    value = Math.Sign(Math.Sin(2f * Math.PI * t));
                    break;
                case FunctionType.Triangle:
                    // 2 * abs( t - 2 * floor( t / 2 ) - 1 ) - 1
                    value = 1f - 4f * (float)Math.Abs
                        (Math.Round(t - 0.25f) - (t - 0.25f));
                    break;
                case FunctionType.Sawtooth:
                    // 2 * ( t/a - floor( t/a + 1/2 ) )
                    value = 2f * (t - (float)Math.Floor(t + 0.5f));
                    break;
                case FunctionType.Random:
                    value = (float)random.NextDouble();
                    break;
            }

            return (invert * amplitude * value + offset);
        }

        public float GetValue()
        {
            float time = (float)(Stopwatch.GetTimestamp() - startTime)
                            / ticksPerSecond;
            return GetValue(time);
        }

        public void Reset()
        {
            startTime = Stopwatch.GetTimestamp();
        }
    }
}
