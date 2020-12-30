using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class CageUnit : BaseConfigUnit
    {
        public Point Position { get; set; } // Координата
        public double AdvanceRatio { get; set; } // ПриблизительныйКоэффициентОпережения
        public double LagRatio { get; set; } // ПриблизительныйКоэффициентОтставания
        public CagesType CageType { get; set; } // ТипКлети
        public int SignalInWork { get; set; } // ИдентификаторСигналаКлетьВРаботе
        public int SignalSpeed { get; set; } // ИдентификаторСигналаСкорость
        public int ThreadNumber { get; set; } // НомерНити

        public CageUnit()
        {
            Uid = default;
            Name = default;
            Position = new Point();
            AdvanceRatio = default;
            LagRatio = default;
            CageType = CagesType.StandTypeHorizontal;
            SignalSpeed = default;
            SignalInWork = default;
            ThreadNumber = default;
        }

        public CageUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.Cage)
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
                        case "ПРИБЛИЗИТЕЛЬНЫЙКОЭФФИЦИЕНТОПЕРЕЖЕНИЯ":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            AdvanceRatio = dbValue;
                            break;
                        case "ПРИБЛИЗИТЕЛЬНЫЙКОЭФФИЦИЕНТОТСТАВАНИЯ":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            LagRatio = dbValue;
                            break;
                        case "ТИПКЛЕТИ":
                            CagesType tmpType = getCageType(value);
                            CageType = tmpType;
                            break;
                        case "ИДЕНТИФИКАТОРСИГНАЛАКЛЕТЬВРАБОТЕ":
                            SignalInWork = Convert.ToInt32(value);
                            break;
                        case "ИДЕНТИФИКАТОРСИГНАЛАСКОРОСТЬ":
                            SignalSpeed = Convert.ToInt32(value);
                            break;
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                    }
                }
            }
        }

        private CagesType getCageType(string type)
        {
            CagesType res = CagesType.StandTypeHorizontal;

            switch (type)
            {
                case "STAND_TYPE_HORIZONTAL":
                    res = CagesType.StandTypeHorizontal;
                    break;
                case "STAND_TYPE_VERTICAL":
                    res = CagesType.StandTypeVertical;
                    break;
            }

            return res;
        }

        public override string ToString()
        {
            string result = "Клеть\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tКоордината={Position.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tПриблизительныйКоэффициентОпережения={AdvanceRatio.ToString("F2").Replace(",", ".")}\n";
            result += $"\tПриблизительныйКоэффициентОтставания={LagRatio.ToString("F2").Replace(",", ".")}\n";
            result += $"\tТипКлети={(CageType==CagesType.StandTypeHorizontal ? "STAND_TYPE_HORIZONTAL" : "STAND_TYPE_VERTICAL")}\n";
            result += $"\tИдентификаторСигналаКлетьВРаботе={SignalInWork}\n";
            result += $"\tИдентификаторСигналаСкорость={SignalSpeed}\n";
            result += $"\tНомерНити={ThreadNumber}\n";

            result += ")\n";
            return result;
        }
    }
}
