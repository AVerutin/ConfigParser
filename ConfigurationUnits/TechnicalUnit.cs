using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class TechnicalUnit : BaseConfigUnit
    {
        public Point StartPos { get; set; }	    // КоординатаНачала
        public Point FinishPos { get; set; }	// КоординатаЗавершения
        public int ThreadNumber { get; set; }	    // НомерНити
        public int AggregateUid { get; set; }	// Агрегат

        public TechnicalUnit()
        {
            Uid = default;
            Name = default;
            StartPos = new Point();
            FinishPos = new Point();
            ThreadNumber = default;
            AggregateUid = default;
        }

        public TechnicalUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.TechnicalUnit)
            {
                StartPos = new Point();
                FinishPos = new Point();
                
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
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                        case "АГРЕГАТ":
                            AggregateUid = Convert.ToInt32(value);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "Техузел\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tКоординатаНачала={StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tКоординатаЗавершения={FinishPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tАгрегат={AggregateUid}\n";
            result += $"\tНомерНити={ThreadNumber}\n";

            result += ")\n";
            return result;
        }
    }
}
