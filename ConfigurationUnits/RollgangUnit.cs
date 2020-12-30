using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class RollgangUnit : BaseConfigUnit
    {
        public Point StartPos { get; set; } // КоординатаНачала
        public Point FinishPos { get; set; } // КоординатаЗавершения
        public int SignalSpeed { get; set; } // ИдентификаторСигналаСкорость
        public int ThreadNumber { get; set; } // НомерНити
        public double SpeedValue { get; set; } // КонстантаСкорости

        public RollgangUnit()
        {
            Uid = default;
            Name = default;
            StartPos = new Point();
            FinishPos = new Point();
            SignalSpeed = default;
            ThreadNumber = default;
            SpeedValue = default;
        }

        public RollgangUnit(ConfigurationUnit confConfigurationUnit)
        {
            if (confConfigurationUnit.Type == ConfigurationUnitType.Rollgang)
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
                        case "ИДЕНТИФИКАТОРСИГНАЛАСКОРОСТЬ":
                            SignalSpeed = Convert.ToInt32(value);
                            break;
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                        case "КОНСТАНТАСКОРОСТИ":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            SpeedValue = dbValue;
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "Рольганг\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tКоординатаНачала={StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tКоординатаЗавершения={FinishPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tИдентификаторСигналаСкорость={SignalSpeed}\n";
            result += $"\tКонстантаСкорости={SpeedValue.ToString("F2").Replace(",", ".")}\n";
            result += $"\tНомерНити={ThreadNumber}\n";

            result += ")\n";
            return result;
        }
    }
}
