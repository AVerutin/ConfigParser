using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class MillConfigUnit
    {
        public string VisualHost { get; set; }	        // IPАдресСервераВизуализацииСлежения
        public int VisualPort { get; set; }	            // ПортСервераВизуализацииСлежения
        public int PeriodAccumulation { get; set; }	    // ПериодНакопленияДанных
        public int PeriodRecording { get; set; }	    // ПериодЗаписиДанных
        public long BufferSize { get; set; }	        // РазмерФайлаБуфера
        public string Name { get; set; }	            // Имя
        public string Comment { get; set; }	            // Комментарий
        public string ArchiveFile { get; set; }	        // ФайлТеговАрхива
        public string ArchiveHost { get; set; }	        // IPАдресСервераАрхивов
        public int ArchivePort { get; set; }	        // ПортСервераАрхивов
        public double MaxObjectSpeed { get; set; }	    // МаксимальнаяСкоростьОбъекта
        public double ObjectAcceleration { get; set; }	// УскорениеОбъекта
        public double CageNeighborhood { get; set; }	// ОкрестностьЗаКлетью
        public double HeadNeighborhood { get; set; }	// ОкрестностьДатчикаГолова
        public double TailNeighborhood { get; set; }	// ОкрестностьДатчикаХвост
        public int MaxUnitsCount { get; set; }	        // МаксимальноеКоличествоЕдиниц
        public long MaxFileSize { get; set; }	        // СуммарныйРазмерФайловБуфера

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public MillConfigUnit()
        {
            VisualHost = default;
            VisualPort = default;
            PeriodAccumulation = default;
            PeriodRecording = default;
            BufferSize = default;
            Name = default;
            Comment = default;
            ArchiveFile = default;
            ArchiveHost = default;
            ArchivePort = default;
            MaxObjectSpeed = default;
            ObjectAcceleration = default;
            CageNeighborhood = default;
            HeadNeighborhood = default;
            TailNeighborhood = default;
            MaxUnitsCount = default;
            MaxFileSize = default;
        }

        /// <summary>
        /// Конструктор из объекта Objects
        /// </summary>
        /// <param name="confConfigurationUnit">Экземпляр объекта Objects</param>
        public MillConfigUnit(ConfigurationUnit confConfigurationUnit)
        {
            if (confConfigurationUnit.Type == ConfigurationUnitType.MillConfig)
            {
                foreach (KeyValuePair<string, string> param in confConfigurationUnit.Parameters)
                {
                    string val = param.Value;
                    switch (param.Key.ToUpper())
                    {
                        case "IPАДРЕССЕРВЕРАВИЗУАЛИЗАЦИИСЛЕЖЕНИЯ":
                            VisualHost = val;
                            break;
                        case "ПОРТСЕРВЕРАВИЗУАЛИЗАЦИИСЛЕЖЕНИЯ":
                            VisualPort = Convert.ToInt32(val);
                            break;
                        case "ПЕРИОДНАКОПЛЕНИЯДАННЫХ":
                            PeriodAccumulation = Convert.ToInt32(val);
                            break;
                        case "ПЕРИОДЗАПИСИДАННЫХ":
                            PeriodRecording = Convert.ToInt32(val);
                            break;
                        case "РАЗМЕРФАЙЛАБУФЕРА":
                            BufferSize = Convert.ToInt64(val);
                            break;
                        case "ИМЯ":
                            Name = val;
                            break;
                        case "КОММЕНТАРИЙ":
                            Comment = val;
                            break;
                        case "ФАЙЛТЕГОВАРХИВА":
                            ArchiveFile = val;
                            break;
                        case "IPАДРЕССЕРВЕРААРХИВОВ":
                            ArchiveHost = val;
                            break;
                        case "ПОРТСЕРВЕРААРХИВОВ":
                            ArchivePort = Convert.ToInt32(val);
                            break;
                        case "МАКСИМАЛЬНАЯСКОРОСТЬОБЪЕКТА":
                            val = val.Replace(".", ",");
                            MaxObjectSpeed = Convert.ToDouble(val);
                            break;
                        case "УСКОРЕНИЕОБЪЕКТА":
                            val = val.Replace(".", ",");
                            ObjectAcceleration = Convert.ToDouble(val);
                            break;
                        case "ОКРЕСТНОСТЬЗАКЛЕТЬЮ":
                            val = val.Replace(".", ",");
                            CageNeighborhood = Convert.ToDouble(val);
                            break;
                        case "ОКРЕСТНОСТЬДАТЧИКАГОЛОВА":
                            val = val.Replace(".", ",");
                            HeadNeighborhood = Convert.ToDouble(val);
                            break;
                        case "ОКРЕСТНОСТЬДАТЧИКАХВОСТ":
                            val = val.Replace(".", ",");
                            TailNeighborhood = Convert.ToDouble(val);
                            break;
                        case "МАКСИМАЛЬНОЕКОЛИЧЕСТВОЕДИНИЦ":
                            MaxUnitsCount = Convert.ToInt32(val);
                            break;
                        case "СУММАРНЫЙРАЗМЕРФАЙЛОВБУФЕРА":
                            MaxFileSize = Convert.ToInt64(val);
                            break;
                    }
                }
            }
        }

        public override string ToString()
        {
            string result = "ОбщиеПараметрыСтана\n(\n";
            
            result += $"\tIPАдресСервераВизуализацииСлежения={VisualHost}\n";
            result += $"\tПортСервераВизуализацииСлежения={VisualPort}\n";
            result += $"\tПериодНакопленияДанных={PeriodAccumulation}\n";
            result += $"\tПериодЗаписиДанныхв={PeriodRecording}\n";
            result += $"\tРазмерФайлаБуфера={BufferSize}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tКомментарий={Comment}\n";

            string relativePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), ArchiveFile);
            result += $"\tФайлТеговАрхива={relativePath}\n";
            
            result += $"\tIPАдресСервераАрхивов={ArchiveHost}\n";
            result += $"\tПортСервераАрхивов={ArchivePort}\n";
            result += $"\tМаксимальнаяСкоростьОбъекта={MaxObjectSpeed.ToString("F2").Replace(",", ".")}\n";
            result += $"\tУскорениеОбъекта={ObjectAcceleration.ToString("F2").Replace(",", ".")}\n";
            result += $"\tОкрестностьЗаКлетью={CageNeighborhood.ToString("F2").Replace(",", ".")}\n";
            result += $"\tОкрестностьДатчикаГолова={HeadNeighborhood.ToString("F2").Replace(",", ".")}\n";
            result += $"\tОкрестностьДатчикаХвост={TailNeighborhood.ToString("F2").Replace(",", ".")}\n";
            result += $"\tМаксимальноеКоличествоЕдиниц={MaxUnitsCount}\n";
            result += $"\tСуммарныйРазмерФайловБуфера={MaxFileSize}\n";

            result += ")\n";
            return result;
        }
    }
}
