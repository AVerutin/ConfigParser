using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class ConfigurationUnit : BaseConfigUnit
    {
        public ConfigurationUnitType Type { get; set; }
        public Dictionary<string,string> Parameters { get; set; }
        public List<ConfigurationUnit> SubObjects { get; set; }

        public ConfigurationUnit()
        {
            Uid = -1;
            Name = "";
            Parameters = new Dictionary<string, string>();
            SubObjects = new List<ConfigurationUnit>();
            Type = ConfigurationUnitType.Default;
        }
    }
}
