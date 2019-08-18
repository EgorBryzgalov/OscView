using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace COMTRADEParser
{
    class DataReader
    {
        private ConfigReader Config { get; set; }
        public List<AnalogChannel> AnalogChannels;
        public List<DigitalChannel> DigitalChannels;
        private string DataPath { get; set; }

        public DataReader()
        {

        }

        public DataReader(ConfigReader config) : this()
        {
            Config = config;
            AnalogChannels = Config.AnalogChannels;
            DigitalChannels = Config.DigitalChannels;
            DataPath = config.FilePath.Replace("cfg", "dat");
        
        }

        public void GetData()
        {
            try
            {


                using (BinaryReader reader = new BinaryReader(File.Open(DataPath, FileMode.Open)))
                {
                    bool FileEndNotReached = true;
                    while (FileEndNotReached)
                    {
                        try
                        {
                            int num = reader.ReadInt32();
                            int timeBias = reader.ReadInt32();
                            foreach (AnalogChannel chan in AnalogChannels)
                            {
                                chan.AddFrame(num, timeBias, reader.ReadInt16());
                            }
                            foreach (DigitalChannel chan in DigitalChannels)
                            {
                                chan.AddFrame(num, timeBias, reader.ReadBoolean());
                            }
                            if (DigitalChannels.Count > 0)
                            {
                                int BitsToSkip = 16 - DigitalChannels.Count % 16;
                                for (int i = 0; i < BitsToSkip; i++)
                                {
                                    reader.ReadBoolean();
                                }
                            }
                        }
                        catch (EndOfStreamException ex)
                        {
                            FileEndNotReached = false;
                           // MessageBox.Show(AnalogChannels[0].DataCollection.Count.ToString());
                        }
                    }
                }
            #if DEBUG
                MessageBox.Show("данные считаны");
            #endif

            }
            catch (FileNotFoundException e)
            {
                
            }

        }


    }
}
