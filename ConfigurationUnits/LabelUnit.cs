using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class LabelUnit : BaseConfigUnit
    {
        public Point Position { get; set; }	         // Координата
        public int ThreadNumber { get; set; }	     // НомерНити
        public string Text { get; set; }             // Текст
        public AlignmentType Alignment { get; set; } // Выравнивание

        public LabelUnit()
        {
            Position = new Point();
            ThreadNumber = default;
            Text = default;
            Alignment = AlignmentType.Left;
        }

        public LabelUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.Label)
            {
                Position = new Point();
                foreach (KeyValuePair<string, string> param in configurationUnit.Parameters)
                {
                    string value = param.Value;
                    string tmpVal;
                    double dbValue;

                    switch (param.Key.ToUpper())
                    {
                        case "КООРДИНАТА":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            Position.PosX = dbValue;
                            break;
                        case "НОМЕРНИТИ":
                            ThreadNumber = Convert.ToInt32(value);
                            break;
                        case "ТЕКСТ":
                            Text = value;
                            break;
                        case "ВЫРАВНИВАНИЕ":
                            AlignmentType type = getAlignmentType(value);
                            Alignment = type;
                            break;
                    }
                }
            }
        }

        private AlignmentType getAlignmentType(string type)
        {
            AlignmentType res = AlignmentType.Default;
            switch (type.ToUpper())
            {
                case "ВЛЕВО":
                    res = AlignmentType.Left;
                    break;
                case "ВПРАВО":
                    res = AlignmentType.Right;
                    break;
                case "ПОЦЕНТРУ":
                    res = AlignmentType.Center;
                    break;
                case "ПОШИРИНЕ":
                    res = AlignmentType.Justify;
                    break;
                case "ПОВЕРХУ":
                    res = AlignmentType.Top;
                    break;
                case "ПОСЕРЕДИНЕ":
                    res = AlignmentType.Middle;
                    break;
                case "ПОНИЗУ":
                    res = AlignmentType.Bottom;
                    break;
            }

            return res;
        }

        public override string ToString()
        {
            string result = "Метка\n(\n";
            
            // result += $"\tИдентификатор={Uid}\n";
            // result += $"\tИмя={Name}\n";
            result += $"\tКоордината={Position.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tНомерНити={ThreadNumber}\n";
            result += $"\tТекст={Text}\n";
            // result += $"\tВыравнивание=";
            //
            // switch (Alignment)
            // {
            //     case AlignmentType.Top: result += "ПоВерху\n"; break;
            //     case AlignmentType.Left: result += "Влево\n"; break;
            //     case AlignmentType.Right: result += "Вправо\n"; break;
            //     case AlignmentType.Justify: result += "ПоШирине\n"; break;
            //     case AlignmentType.Bottom: result += "ПоНизу\n"; break;
            //     case AlignmentType.Middle: result += "ПоСередине\n"; break;
            //     case AlignmentType.Center: result += "ПоЦентру\n"; break;
            // }

            result += ")\n";
            return result;
        }
    }
}
