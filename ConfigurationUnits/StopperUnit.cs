using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class StopperUnit : BaseConfigUnit
    {
        public int ThreadNumber { get; set; }	    // НомерНити
        public Point Position { get; set; }	    // Координата
        public int SignalUid { get; set; }	    // СигналУпорУстановлен

        public StopperUnit()
        {
            Uid = default;
            Name = default;
            ThreadNumber = default;
            Position = new Point();
            SignalUid = default;
        }

        public StopperUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.Stopper)
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
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                        case "КООРДИНАТА":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            Position.PosX = dbValue;
                            break;
                        case "СИГНАЛУПОРУСТАНОВЛЕН":
                            SignalUid = Convert.ToInt32(value);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "Упор\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tКоордината={Position.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tСигналУпорУстановлен={SignalUid}\n";
            result += $"\tНомерНити={ThreadNumber}\n";

            result += ")\n";
            return result;
        }
    }
}
