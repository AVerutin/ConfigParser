using System;
using System.Collections.Generic;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class SignalUnit : BaseConfigUnit
    {
        public SignalType Type { get; set; } // Тип
        public string ServerName { get; set; } // Имя тега на OPC-сервере
        public int DataBlockUid { get; set; } // ИдентификаторБлокаДанных
        public int Byte { get; set; } // Байт
        public int Bit { get; set; } // Бит
        public double VirtualValue { get; set; } // ЗначениеВиртуальногоСигнала
        public CompoundSignalType CompoundSignal { get; set; } // СоставнойСигнал
        public bool UserWrite { get; set; } // ЗаписьКлиентами
        public int MinAnalogLevel { get; set; } // МинимальныйУровеньАналоговогоСигнала

        public SignalUnit()
        {
            Uid = default;
            Name = default;
            Type = SignalType.INT;
            DataBlockUid = default;
            ServerName = default;
            Byte = 14;
            Bit = 0;
            VirtualValue = default;
            CompoundSignal = CompoundSignalType.SIMPLE_SIGNAL;
            UserWrite = true;
            MinAnalogLevel = default;
        }

        public SignalUnit(ConfigurationUnit confConfigurationUnit)
        {
            if (confConfigurationUnit.Type == ConfigurationUnitType.Signal)
            {
                foreach (KeyValuePair<string, string> param in confConfigurationUnit.Parameters)
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
                        case "ИМЯСЕРВЕР":
                            ServerName = value;
                            break;
                        case "ТИП":
                            SignalType type = getSignalsTypes(value);
                            Type = type;
                            break;
                        case "ИДЕНТИФИКАТОРБЛОКАДАННЫХ":
                            DataBlockUid = Convert.ToInt32(value);
                            break;
                        case "БАЙТ":
                            Byte = Convert.ToInt32(value);
                            break;
                        case "БИТ":
                            Bit = Convert.ToInt32(value);
                            break;
                        case "ЗНАЧЕНИЕВИРТУАЛЬНОГОСИГНАЛА":
                            VirtualValue = Convert.ToDouble(value.Replace(".",","));
                            break;
                        case "СОСТАВНОЙСИГНАЛ":
                            CompoundSignalType tmpType = getCompoundSignalsTypes(value);
                            CompoundSignal = tmpType;
                            break;
                        case "ЗАПИСЬКЛИЕНТАМИ":
                            int tmpValue = Convert.ToInt32(value);
                            UserWrite = tmpValue > 0;
                            break;
                        case "МИНИМАЛЬНЫЙУРОВЕНЬАНАЛОГОВОГОСИГНАЛА":
                            MinAnalogLevel = Convert.ToInt32(value);
                            break;                        
                    }
                }
            }
        }

        private SignalType getSignalsTypes(string type)
        {
            SignalType res = SignalType.INT;
            switch (type)
            {
                case "BOOL":
                    res = SignalType.BOOL;
                    break;
                case "BYTE":
                    res = SignalType.BYTE;
                    break;
                case "INT":
                    res = SignalType.INT;
                    break;
                case "WORD":
                    res = SignalType.WORD;
                    break;
                case "DINT":
                    res = SignalType.DINT;
                    break;
                case "DWORD":
                    res = SignalType.DWORD;
                    break;
                case "REAL":
                    res = SignalType.REAL;
                    break;
                case "BINARY":
                    res = SignalType.BINARY;
                    break;
                case "REALOM":
                    res = SignalType.REALOM;
                    break;
                case "DWORDOM":
                    res = SignalType.DWORDOM;
                    break;
                default:
                    res = SignalType.NONE;
                    break;
            }

            return res;
        }

        private CompoundSignalType getCompoundSignalsTypes(string type)
        {
            CompoundSignalType res = CompoundSignalType.SIMPLE_SIGNAL;
            switch (type)
            {
                case "REAL_SIGNAL":
                    res = CompoundSignalType.SIMPLE_SIGNAL;
                    break;
                case "VIRTUAL_SIGNAL":
                    res = CompoundSignalType.VIRTUAL_SIGNAL;
                    break;
                default:
                    res = CompoundSignalType.SIMPLE_SIGNAL;
                    break;
            }

            return res;
        }

        public override string ToString()
        {
            //TODO: Добавить различные параметры для различных типов подключения
            string result = "Сигнал\n(\n";
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";

            
            if (CompoundSignal == CompoundSignalType.SIMPLE_SIGNAL)
            {
                // Для реального сигнала
                result += $"\tТип={Type.ToString()}\n";
                result += $"\tИдентификаторБлокаДанных={DataBlockUid}\n";
                result += $"\tБайт={Byte}\n";
            }
            else
            {
                // Для виртуального сигнала
                result += $"\tЗначениеВиртуальногоСигнала={VirtualValue.ToString("F2").Replace(",", ".")}\n";
                result += $"\tСоставнойСигнал={CompoundSignal.ToString()}\n";
                result += $"\tЗаписьКлиентами={(UserWrite ? 1 : 0)}\n";
            }

            // result += $"\tИмяСервер={ServerName}\n";
            // result += $"\tТип={Type.ToString()}\n";
            // result += $"\tИдентификаторБлокаДанных={DataBlockUid}\n";
            // result += $"\tБайт={Byte}\n";
            // result += $"\tБит={Bit}\n";
            // result += $"\tЗначениеВиртуальногоСигнала={VirtualValue.ToString("F2").Replace(",", ".")}\n";
            // result += $"\tСоставнойСигнал={CompoundSignal.ToString()}\n";
            // result += $"\tЗаписьКлиентами={(UserWrite ? 1 : 0)}\n";
            // result += $"\tМинимальныйУровеньАналоговогоСигнала={MinAnalogLevel}\n";

            result += ")\n";
            return result;
        }
    }
}
