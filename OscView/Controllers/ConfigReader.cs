using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Windows;

namespace COMTRADEParser
{
    enum DataType
    {
        BINARY,
        ASCII
    }
    class ConfigReader
    {
        private static ConfigReader Instance;
        
        public string FilePath { get; set; }

        public int NumberOfChannels { get; set; }

        public int NumberOfAnalogs { get; set; }

        public int NumberOfDigitals { get; set; }

        public List<AnalogChannel> AnalogChannels { get; set; } = new List<AnalogChannel>();

        public List<DigitalChannel> DigitalChannels { get; set; } = new List<DigitalChannel>();

        public int GridFrequency { get; set; }

        public int SampleRate { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime TripTime { get; set; }

        public DataType dataType { get; set; }

        public int TimeMultiplyer { get; set; }

        private ConfigReader(string filePath)
        {
            FilePath = filePath;
            Parse();
        }

        public static ConfigReader GetInstance(string filePath)
        {
            if (Instance == null)
                Instance = new ConfigReader(filePath);
            return Instance;
        }

        public void Parse()
        {
            
            using (StreamReader reader = new StreamReader(FilePath))
            {
                List<string> lines = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                int LineNum = 1;

                //Кол-во каналов, кол-во аналоговых каналов, кол-во цифровых каналов
                #region парсинг второй строки
                string[] second = lines[LineNum].Split(',');
                NumberOfChannels = int.Parse(second[0]);
                StringBuilder StrBuilder = new StringBuilder();
                foreach (char c in second[1])
                {
                    if (c != 'A')
                        StrBuilder.Append(c);
                }
                NumberOfAnalogs = int.Parse(StrBuilder.ToString());
                StrBuilder.Clear();
                foreach (char c in second[2])
                {
                    if (c != 'D')
                        StrBuilder.Append(c);
                }
                NumberOfDigitals = int.Parse(StrBuilder.ToString());
                LineNum++;
                #endregion

                //Парсинг ирнформации о каналах
                #region парсинг каналов
                AnalogChannel GetAnalog(string[] s)
                {
                    if (s.Length != 13)
                        throw new ArgumentException("Ошибка чтения строки, неверное количество данных по аналогу");
                    AnalogChannel analog = new AnalogChannel();
                    analog.Number = int.Parse(s[0]);
                    analog.Id = s[1];
                    analog.Phase = s[2];
                    analog.Circuit = s[3];
                    analog.Unit = s[4];
                    analog.A = float.Parse(s[5].Replace('.', ','));
                    analog.B = float.Parse(s[6].Replace('.', ','));
                    analog.Skew = float.Parse(s[7].Replace('.', ','));
                    analog.Min = int.Parse(s[8]);
                    analog.Max = int.Parse(s[9]);
                    analog.Primary = int.Parse(s[10]);
                    analog.Secondary = int.Parse(s[11]);
                    analog.PS = s[12];
                    return analog;


                }
                DigitalChannel GetDigital(string[] s)
                {
                    if (s.Length != 3)
                        throw new ArgumentException("Ошибка чтения строки, неверное количество данных по дискрету");
                    DigitalChannel digital = new DigitalChannel();
                    digital.Number = int.Parse(s[0]);
                    digital.Id = s[1];
                    digital.DefaultState = int.Parse(s[2]);
                    return digital;
                }
                for (int i = 1; i <= NumberOfChannels; i++)
                {
                    string[] chan = lines[LineNum].Split(',');
                    if (chan.Length == 3)
                        DigitalChannels.Add(GetDigital(chan));
                    else
                        AnalogChannels.Add(GetAnalog(chan));
                    LineNum++;
                }

                #endregion

                GridFrequency = int.Parse(lines[LineNum]);
                LineNum++;

                if (int.Parse(lines[LineNum++]) != 1)
                    throw new NotImplementedException("Не реализована работа с несколькими частотами дискретизации");

                string[] SmplRate = lines[LineNum++].Split(',');
                SampleRate = int.Parse(SmplRate[0]);

                BeginTime = DateTime.Parse(lines[LineNum++]);
                TripTime = DateTime.Parse(lines[LineNum++]);

                if (lines[LineNum] == "BINARY")
                    dataType = DataType.BINARY;
                else if (lines[LineNum] == "ASCII")
                    dataType = DataType.ASCII;
                else
                    throw new InvalidDataException("Неверный тип представления данных (допускается BINARY или ASCII");
                LineNum++;

                TimeMultiplyer = int.Parse(lines[LineNum]);
            }

        #if DEBUG
            MessageBox.Show("Успешно");
        #endif

        }
    }
}
