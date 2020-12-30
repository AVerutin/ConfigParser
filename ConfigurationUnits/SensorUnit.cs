using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class SensorUnit : BaseConfigUnit
    {
        public Point Position { get; set; } // Координата
        public int SignalUid { get; set; } // ИдентификаторСигналаСДатчика
        public int ThreadNumber { get; set; } // НомерНити
        public int Resolution { get; set; } // Разрешение

        public SensorUnit()
        {
            Uid = default;
            Name = default;
            Position = new Point();
            SignalUid = default;
            ThreadNumber = default;
            Resolution = default;
        }

        public SensorUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.Sensor)
            {
                Position = new Point();
                foreach (KeyValuePair<string, string> param in configurationUnit.Parameters)
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
                        case "КООРДИНАТА":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            Position.PosX = dbValue;
                            break;
                        case "ИДЕНТИФИКАТОРСИГНАЛАСДАТЧИКА":
                            SignalUid = Convert.ToInt32(value);
                            break;
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                        case "РАЗРЕШЕНИЕ":
                            Resolution = Convert.ToInt32(value);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "Датчик\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tКоордината={Position.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tИдентификаторСигналаСДатчика={SignalUid}\n";
            result += $"\tРазрешение={Resolution}\n";
            result += $"\tНомерНити={ThreadNumber}\n";

            result += ")\n";
            return result;
        }
    }
}

