using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class AggregateUnit : BaseConfigUnit
    {
        public string Prefix { get; set; }          // Префикс
        public int WriteSignalsPeriod { get; set; } // ЧастотаЗаписиСигналов

        public AggregateUnit()
        {
            Uid = default;
            Name = default;
            Prefix = default;
            WriteSignalsPeriod = default;
        }

        public AggregateUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.Aggregate)
            {
                foreach (KeyValuePair<string, string> param in configurationUnit.Parameters)
                {
                    string value = param.Value;

                    switch (param.Key.ToUpper())
                    {
                        case "ИДЕНТИФИКАТОР":
                            Uid = Convert.ToInt32(value);
                            break;
                        case "ИМЯ":
                            Name = value;
                            break;
                        case "ПРЕФИКС":
                            Prefix = value;
                            break;
                        case "ЧАСТОТАЗАПИСИСИГНАЛОВ":
                            WriteSignalsPeriod = Convert.ToInt32(value);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Вывод объекта "Агрегат" в текстовом виде для сохранения в конфигурацмм
        /// </summary>
        /// <returns>Текстовый вид в формате файла конфигурации</returns>
        public override string ToString()
        {
            string result = "Агрегат\n(\n";
            
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            if(!string.IsNullOrEmpty(Prefix))
            {
                result += $"\tПрефикс={Prefix}\n";
            }
            result += $"\tЧастотаЗаписиСигналов={WriteSignalsPeriod}\n";

            result += ")\n";
            return result;
        }
    }
}
