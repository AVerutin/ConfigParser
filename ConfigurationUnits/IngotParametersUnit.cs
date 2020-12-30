using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class IngotParametersUnit : BaseConfigUnit
    {
        public IngotParameterType Type { get; set; }	// Тип
        public double Value { get; set; }	                // ЗначениеЧисло
        public bool Logging { get; set; }	            // ЛогированиеЗаписи

        public IngotParametersUnit()
        {
            Uid = default;
            Name = default;
            Type = IngotParameterType.INT;
            Value = 0;
            Logging = true;
        }

        public IngotParametersUnit(ConfigurationUnit configurationUnit)
        {
            if (configurationUnit.Type == ConfigurationUnitType.IngotParams)
            {
                foreach (KeyValuePair<string, string> param in configurationUnit.Parameters)
                {
                    string value = param.Value;
                    string tmpVal;
                    double dbValue;
                    int intVal;

                    switch (param.Key.ToUpper())
                    {
                        case "ИДЕНТИФИКАТОР":
                            Uid = Convert.ToInt32(value);
                            break;
                        case "ИМЯ":
                            Name = value;
                            break;
                        case "ТИП":
                            IngotParameterType type = getIngotParameterType(value);
                            Type = type;
                            break;
                        case "ЗНАЧЕНИЕЧИСЛО":
                            tmpVal = value.Replace(".", ",");
                            dbValue = Convert.ToDouble(tmpVal);
                            Value = dbValue;
                            break;
                        case "ЛОГИРОВАНИЕЗАПИСИ":
                            intVal = Convert.ToInt32(value);
                            Logging = intVal > 0;
                            break;
                    }
                }
            }
        }

        private IngotParameterType getIngotParameterType(string type)
        {
            IngotParameterType res = IngotParameterType.INT;
            switch (type)
            {
                case "BOOL":
                    res = IngotParameterType.BOOL;
                    break;
                case "BYTE":
                    res = IngotParameterType.BYTE;
                    break;
                case "INT":
                    res = IngotParameterType.INT;
                    break;
                case "WORD":
                    res = IngotParameterType.WORD;
                    break;
                case "DINT":
                    res = IngotParameterType.DINT;
                    break;
                case "DWORD":
                    res = IngotParameterType.DWORD;
                    break;
                case "REAL":
                    res = IngotParameterType.REAL;
                    break;
                case "BINARY":
                    res = IngotParameterType.BINARY;
                    break;
            }

            return res;
        }
    }
}


