using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class ThreadUnit : BaseConfigUnit
    {
        public int ThreadNumber { get; set; }           // НомерНити
        public Point StartPos { get; set; }             // КоординатаНачала
        public Point FinishPos { get; set; }            // КоординатаЗавершения
        public ThreadDirection Direction { get; set; }  // Направление
        public int PrevThread { get; set; }             // ПредыдущаяНить
        public int NextThread { get; set; }             // СледующаяНить
        public bool StopOnEnds { get; set; }            // ОстанавливатьНаКонцахНити

        public ThreadUnit()
        {
            Uid = default;
            Name = default;
            StartPos = new Point();
            FinishPos = new Point();
            Direction = ThreadDirection.Horizontal;
            PrevThread = default;
            NextThread = default;
            ThreadNumber = default;
            StopOnEnds = true;
        }

        public ThreadUnit(ConfigurationUnit confConfigurationUnit)
        {
            if (confConfigurationUnit.Type == ConfigurationUnitType.Thread)
            {
                StartPos = new Point();
                FinishPos = new Point();
            
                foreach (KeyValuePair<string, string> param in confConfigurationUnit.Parameters)
                {
                    string value = param.Value;
                    string tmpVal;
                    double dbValue;
                    
                    switch (param.Key.ToUpper())
                    {
                        case "ИДЕНТИФИКАТОР":
                            Uid = Convert.ToInt32(value);
                            break;
                        case "ИМЯ":
                            Name = value;
                            break;
                        case "КООРДИНАТАНАЧАЛА":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            StartPos.PosX = dbValue;
                            break;
                        case "КООРДИНАТАЗАВЕРШЕНИЯ": 
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            FinishPos.PosX = dbValue;
                            break;
                        case "НАПРАВЛЕНИЕ":
                            ThreadDirection direction = getThreadDirection(value);
                            Direction = direction;
                            break;
                        case "ПРЕДЫДУЩАЯНИТЬ":
                            PrevThread = Convert.ToInt32(value);
                            break;
                        case "СЛЕДУЮЩАЯНИТЬ":
                            NextThread = Convert.ToInt32(value);
                            break;
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                        case "ОСТАНАВЛИВАТЬНАКОНЦАХНИТИ":
                            int tmpValue = Convert.ToInt32(value);
                            StopOnEnds = tmpValue > 0;
                            break;
                    }
                }
            }
        }

        private ThreadDirection getThreadDirection(string value)
        {
            ThreadDirection res = ThreadDirection.Horizontal;
            switch (value.ToUpper())
            {
                case "ГОРИЗОНТАЛЬНО":
                    res = ThreadDirection.Horizontal;
                    break;
                case "ВЕРТИКАЛЬНО":
                    res = ThreadDirection.Vertical;
                    break;
            }

            return res;
        }

        public override string ToString()
        {
            string result = "Нить\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tНомерНити={ThreadNumber}\n";
            result += $"\tКоординатаНачала={StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tКоординатаЗавершения={FinishPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tНаправление={(Direction == ThreadDirection.Horizontal ? "Горизонтально" : "Вертикально")}\n";
            result += $"\tПредыдущаяНить={PrevThread}\n";
            result += $"\tСледующаяНити={NextThread}\n";
            result += $"\tОстанавливатьНаКонцахНити={(StopOnEnds ? "1" : "0")}\n";

            result += ")\n";
            return result;
        }
    }
}
