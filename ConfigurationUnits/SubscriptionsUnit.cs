using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class SubscriptionsUnit
    {
        public int Threads { get; set; }	                    // Потоки
        public List<ConfigurationUnit> SubObjects { get; set; }	// TcpServer

        public SubscriptionsUnit()
        {
            Threads = default;
            SubObjects = new List<ConfigurationUnit>();
        }

        public SubscriptionsUnit(ConfigurationUnit confConfigurationUnit)
        {
            if (confConfigurationUnit.Type == ConfigurationUnitType.Subscription)
            {
                foreach (KeyValuePair<string, string> param in confConfigurationUnit.Parameters)
                {
                    switch(param.Key.ToUpper())
                    {
                        case "ПОТОКИ":
                            Threads = Convert.ToInt32(param.Value);
                            break;
                    }
                }

                SubObjects = confConfigurationUnit.SubObjects.Count > 0
                    ? confConfigurationUnit.SubObjects
                    : new List<ConfigurationUnit>();
            }
        }

        public override string ToString()
        {
            string result = "Подписки\n(\n";
            
            result += $"\tПотоки={Threads}\n";
            result += SubObjectsToString(SubObjects);

            result += ")\n";
            return result;
        }

        private string SubObjectsToString(List<ConfigurationUnit> subObjects, int level=1)
        {
            string result = "";
            string tab = "";
            for (int i = 0; i < level; i++)
            {
                tab += "\t";
            }

            foreach (ConfigurationUnit item in subObjects)
            {
                result = tab + item.Name + "\n" + tab + "(\n";
                
                // Выводим параметры объекта
                if(item.Parameters.Count>0)
                {
                    foreach (KeyValuePair<string, string> parameter in item.Parameters)
                    {
                        result += tab + "\t" + parameter.Key + "=" + parameter.Value + "\n";
                    }
                }

                if (item.SubObjects.Count > 0)
                {
                    result += SubObjectsToString(item.SubObjects, ++level);
                }

                result += tab + ")\n";
            }

            return result;
        }
    }
}