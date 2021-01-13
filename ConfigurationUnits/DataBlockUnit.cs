using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class DataBlockUnit : BaseConfigUnit
    {
        public DataBlockType Type { get; set; } // ТипСвязи
        public int DataBlockSize { get; set; } // РазмерБлокаДанных
        public bool Reversal { get; set; } // ПризнакПерестановкиБайт
        public string ServerName { get; set; } //ИмяСервера (для типа связи TCP_SERVER)
        public int Port { get; set; } // Порт (для типа связи TCP_SERVER)
        public int SenderPort { get; set; } // ПортОтправителя (для типа связи OPC, UDP, TCP_CLIENT)
        public string SenderHost { get; set; } // IPАдресОтправителя (для типа связи OPC, UDP, TCP_CLIENT)
        public string ServerPath { get; set; } // ПутьДоступа
        public bool HasHead { get; set; } // Заголовок

        public DataBlockUnit()
        {
            Uid = default;
            Name = default;
            Type = DataBlockType.TIMESET;
            DataBlockSize = default;
            Reversal = false;
            ServerName = default;
            Port = default;
            SenderHost = default;
            SenderPort = default;
            HasHead = true;
            ServerPath = default;
        }

        public DataBlockUnit(ConfigurationUnit confConfigurationUnit)
        {
            if (confConfigurationUnit.Type == ConfigurationUnitType.DataBlock)
            {
                foreach (KeyValuePair<string, string> param in confConfigurationUnit.Parameters)
                {
                    string val = param.Value;
                    switch (param.Key.ToUpper())
                    {
                        case "ИДЕНТИФИКАТОРБЛОКАДАННЫХ":
                            Uid = Convert.ToInt32(val);
                            break;
                        case "ИМЯБЛОКАДАННЫХ":
                            Name = val;
                            break;
                        case "ТИПСВЯЗИ":
                            DataBlockType type = getDataBlockTypes(val);
                            Type = type;
                            break;
                        case "РАЗМЕРБЛОКАДАННЫХ":
                            DataBlockSize = Convert.ToInt32(val);
                            break;
                        case "ПРИЗНАКПЕРЕСТАНОВКИБАЙТ":
                            int intVal = Convert.ToInt32(val);
                            Reversal = intVal > 0;
                            break;
                        case "ПОРТ":
                            Port = Convert.ToInt32(val);
                            break;
                        case "ПОРТОТПРАВИТЕЛЯ":
                            SenderPort = Convert.ToInt32(val);
                            break;
                        case "IPАДРЕСОТПРАВИТЕЛЯ":
                            SenderHost = val;
                            break;
                        case "ЗАГОЛОВОК":
                            int tmpVal = Convert.ToInt32(val);
                            HasHead = tmpVal > 0;
                            break;
                        case "СЕРВЕР":
                            ServerName = val;
                            break;
                        case "ПУТЬДОСТУПА":
                            ServerPath = val;
                            break;
                    }
                }
            }
        }

        private DataBlockType getDataBlockTypes(string type)
        {
            DataBlockType res = DataBlockType.TIMESET;
            switch (type.Trim().ToUpper())
            {
                case "UDP":
                    res = DataBlockType.UDP;
                    break;
                case "TCP_CLIENT":
                    res = DataBlockType.TCP_CLIENT;
                    break;
                case "TCP_SERVER":
                    res = DataBlockType.TCP_SERVER;
                    break;
                case "TIMESET":
                    res = DataBlockType.TIMESET;
                    break;
                case "OPC":
                    res = DataBlockType.OPC;
                    break;
            }

            return res;
        }

        public override string ToString()
        {
            string result = "БлокДанных\n(\n";
            
            result += $"\tИдентификаторБлокаДанных={Uid}\n";
            result += $"\tИмяБлокаДанных={Name}\n";
            result += $"\tТипСвязи={Type.ToString()}\n";
            result += $"\tЗаголовок={(HasHead ? 1 : 0)}\n";
            result += $"\tРазмерБлокаДанных={DataBlockSize}\n";
            result += $"\tПризнакПерестановкиБайт={(Reversal ? 1 : 0)}\n";
            result += $"\tIPАдресОтправителя={SenderHost}\n";
            result += $"\tПортОтправителя={SenderPort}\n";
            result += $"\tСервер={ServerName}\n";
            result += $"\tПорт={Port}\n";

            result += ")\n";
            return result;
        }
    }
}

