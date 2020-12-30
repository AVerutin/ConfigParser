using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class DeleterUnit : BaseConfigUnit
    {
        public int ThreadNumber { get; set; }	        // НомерНити
        public Point StartPos { get; set; }	        // КоординатаНачала
        public Point FinishPos { get; set; }	    // КоординатаЗавершения
        public double DeletingTime { get; set; }	// ВремяУдаления

        public DeleterUnit()
        {
            Uid = default;
            Name = default;
            ThreadNumber = default;
            StartPos = new Point();
            FinishPos = new Point();
            DeletingTime = default;
        }

        public DeleterUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.Deleter)
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
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
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
                        case "ВРЕМЯУДАЛЕНИЯ":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            DeletingTime = dbValue;
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "УдалениеЗастрявших\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tНомерНити={ThreadNumber}\n";
            result += $"\tКоординатаНачала={StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tКоординатаЗавершения={FinishPos.PosX.ToString("F2").Replace(",",".")}\n";
            result += $"\tВремяУдаления={DeletingTime.ToString("F2").Replace(",", ".")}\n";

            result += ")\n";
            return result;
        }
    }
}
