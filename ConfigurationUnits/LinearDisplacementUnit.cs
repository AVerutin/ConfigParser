using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class LinearDisplacementUnit : BaseConfigUnit
    {
        public Point StartPos { get; set; } // КоординатаНачала
        public Point FinishPos { get; set; } // КоординатаЗавершения
        public int StepSizeSignalUid { get; set; } // СигналВеличиныДвижения
        public int StartMovingSignalUid { get; set; } // СигналФактаДвижения
        public int ThreadNumber { get; set; } // НомерНити

        public LinearDisplacementUnit()
        {
            Uid = default;
            Name = default;
            StartPos = new Point();
            FinishPos = new Point();
            StepSizeSignalUid = default;
            StartMovingSignalUid = default;
            ThreadNumber = default;
        }

        public LinearDisplacementUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.LinearMoving)
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
                        case "СИГНАЛВЕЛИЧИНЫДВИЖЕНИЯ":
                            StepSizeSignalUid = Convert.ToInt32(value);
                            break;
                        case "СИГНАЛФАКТАДВИЖЕНИЯ":
                            StartMovingSignalUid = Convert.ToInt32(value);
                            break;
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "АгрегатЛинейногоПеремещения\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tНомерНити={ThreadNumber}\n";
            result += $"\tКоординатаНачала={StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tКоординатаЗавершения={FinishPos.PosX.ToString("F2").Replace(",",".")}\n";
            result += $"\tСигналВеличиныДвижения={StepSizeSignalUid}\n";
            result += $"\tСигналФактаДвижения={StartMovingSignalUid}\n";

            result += ")\n";
            return result;
        }
    }
}
