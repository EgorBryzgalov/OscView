using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMTRADEParser
{
    class Channel
    {
        public int Number { get; set; }

        public string Id { get; set; }


    }
    class AnalogChannel : Channel
    {
        public AnalogChannel() : base()
        {

        }

        public List<AnalogFrame> DataCollection { get; set; } = new List<AnalogFrame>();

        public string Phase { get; set; }

        public string Circuit { get; set; }

        public string Unit { get; set; }

        public float A { get; set; }

        public float B { get; set; }

        public float Skew { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public int Primary { get; set; }

        public int Secondary { get; set; }

        public string PS { get; set; }

        public void AddFrame(AnalogFrame frame)
        {
            DataCollection.Add(frame);
        }
        public void AddFrame(int num, int timeBias, int value)
        {
            AnalogFrame frame = new AnalogFrame(num, timeBias, value);
            DataCollection.Add(frame);
        }
    }
    class DigitalChannel : Channel
    {
        public int DefaultState { get; set; }

        public bool State { get; set; }

        public List<DigitalFrame> DataCollection { get; set; } = new List<DigitalFrame>();

        public void AddFrame(DigitalFrame frame)
        {
            DataCollection.Add(frame);
        }
        public void AddFrame(int num, int timeBias, bool value)
        {
            DigitalFrame frame = new DigitalFrame(num, timeBias, value);
            DataCollection.Add(frame);
        }
    }

    struct AnalogFrame
    {
        public int Value { get; set; }

        public int TimeBias { get; set; }

        public int Num { get; set; }

        public AnalogFrame(int num, int timeBias, int value)
        {
            Value = value;
            TimeBias = timeBias;
            Num = num;
        }

    }
    struct DigitalFrame
    {
        public int TimeBias { get; set; }

        public int Num { get; set; }

        public bool Value { get; set; }

        public DigitalFrame(int num, int timeBias, bool value)
        {
            Value = value;
            TimeBias = timeBias;
            Num = num;
        }
    }
}
