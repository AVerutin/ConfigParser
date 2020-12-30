using System.Collections.Generic;
using ConfigParser.ConfigurationUnits;
using ConfigParser.Types;

namespace ConfigParser.Data
{
    public class ProductionThread
    {
        public int Uid { get; set; }                    // Идентификатор
        public int ThreadNumber { get; set; }           // НомерНити
        public string Name { get; set; }                // Имя
        public Point StartPos { get; set; }             // КоординатаНачала
        public Point FinishPos { get; set; }            // КоординатаЗавершения
        public ThreadDirection Direction { get; set; }  // Направление
        public int PrevThread { get; set; }             // ПредыдущаяНить
        public int NextThread { get; set; }             // СледующаяНить
        public bool StopOnEnds { get; set; }            // ОстанавливатьНаКонцахНити
        
        
        public List<RollgangUnit> ListRollgangUnits { get; set; }
        public List<LabelUnit> ListLabelUnits { get; set; }
        public List<SensorUnit> ListSensorUnits { get; set; }
        public List<StopperUnit> ListStopperUnits { get; set; }
        public List<LinearDisplacementUnit> ListLinearDisplacementUnits { get; set; }
        public List<DeleterUnit> ListDeleterUnits { get; set; }
        public List<CageUnit> ListCagesUnits { get; set; }
        public List<TechnicalUnit> ListTechnicalUnits { get; set; }

        public ProductionThread()
        {
            ListRollgangUnits = new List<RollgangUnit>();
            ListLabelUnits = new List<LabelUnit>();
            ListSensorUnits = new List<SensorUnit>();
            ListStopperUnits = new List<StopperUnit>();
            ListLinearDisplacementUnits = new List<LinearDisplacementUnit>();
            ListDeleterUnits = new List<DeleterUnit>();
            ListCagesUnits = new List<CageUnit>();
            ListTechnicalUnits = new List<TechnicalUnit>();
        }

        public override string ToString()
        {
            // Печать параметров нити
            string result = $"// Линия производтва №{Uid} [Номер нити = {ThreadNumber}]\n";
            
            result += "Нить\n(\n";
            result += $"\tИдентификатор={Uid}\n";
            result += $"\tИмя={Name}\n";
            result += $"\tНомерНити={ThreadNumber}\n";
            result += $"\tКоординатаНачала={StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tКоординатаЗавершения={FinishPos.PosX.ToString("F2").Replace(",", ".")}\n";
            result += $"\tНаправление={(Direction == ThreadDirection.Horizontal ? "Горизонтально" : "Вертикально")}\n";
            result += $"\tПредыдущаяНить={PrevThread}\n";
            result += $"\tСледующаяНити={NextThread}\n";
            result += $"\tОстанавливатьНаКонцахНити={(StopOnEnds ? "1" : "0")}\n";

            result += ")\n";
            
            // Печать списка рольгангов
            foreach (RollgangUnit rollgang in ListRollgangUnits)
            {
                result += "Рольганг\n(\n";
                result += $"\tИдентификатор={rollgang.Uid}\n";
                result += $"\tИмя={rollgang.Name}\n";
                result += $"\tКоординатаНачала={rollgang.StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tКоординатаЗавершения={rollgang.FinishPos.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tИдентификаторСигналаСкорость={rollgang.SignalSpeed}\n";
                result += $"\tКонстантаСкорости={rollgang.SpeedValue.ToString("F2").Replace(",", ".")}\n";
                result += $"\tНомерНити={rollgang.ThreadNumber}\n";
                result += ")\n";
            }
            
            // Печать списка меток
            foreach (LabelUnit label in ListLabelUnits)
            {
                result += "Метка\n(\n";
                result += $"\tКоордината={label.Position.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tНомерНити={label.ThreadNumber}\n";
                result += $"\tТекст={label.Text}\n";
                result += ")\n";
            }
            
            // Печать списка датчиков
            foreach (SensorUnit sensor in ListSensorUnits)
            {
                result += "Датчик\n(\n";
                result += $"\tИдентификатор={sensor.Uid}\n";
                result += $"\tИмя={sensor.Name}\n";
                result += $"\tКоордината={sensor.Position.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tИдентификаторСигналаСДатчика={sensor.SignalUid}\n";
                result += $"\tРазрешение={sensor.Resolution}\n";
                result += $"\tНомерНити={sensor.ThreadNumber}\n";
                result += ")\n";
            }
            
            // Печать списка упоров
            foreach (StopperUnit stopper in ListStopperUnits)
            {
                result += "Упор\n(\n";
                result += $"\tИдентификатор={stopper.Uid}\n";
                result += $"\tИмя={stopper.Name}\n";
                result += $"\tКоордината={stopper.Position.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tСигналУпорУстановлен={stopper.SignalUid}\n";
                result += $"\tНомерНити={stopper.ThreadNumber}\n";
                result += ")\n";
            }
            
            // Печать списка агрегатов линейного перемещения
            foreach (LinearDisplacementUnit linear in ListLinearDisplacementUnits)
            {
                result += "АгрегатЛинейногоПеремещения\n(\n";
                result += $"\tИдентификатор={linear.Uid}\n";
                result += $"\tИмя={linear.Name}\n";
                result += $"\tНомерНити={linear.ThreadNumber}\n";
                result += $"\tКоординатаНачала={linear.StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tКоординатаЗавершения={linear.FinishPos.PosX.ToString("F2").Replace(",",".")}\n";
                result += $"\tСигналВеличиныДвижения={linear.StepSizeSignalUid}\n";
                result += $"\tСигналФактаДвижения={linear.StartMovingSignalUid}\n";
                result += ")\n";
            }
            
            // Печать списка Удаление застрявших
            foreach (DeleterUnit deleter in ListDeleterUnits)
            {
                result += "УдалениеЗастрявших\n(\n";
                result += $"\tИдентификатор={deleter.Uid}\n";
                result += $"\tИмя={deleter.Name}\n";
                result += $"\tНомерНити={deleter.ThreadNumber}\n";
                result += $"\tКоординатаНачала={deleter.StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tКоординатаЗавершения={deleter.FinishPos.PosX.ToString("F2").Replace(",",".")}\n";
                result += $"\tВремяУдаления={deleter.DeletingTime.ToString("F2").Replace(",", ".")}\n";
                result += ")\n";
            }
            
            // Печать списка клетей
            foreach (CageUnit cage in ListCagesUnits)
            {
                result += "Клеть\n(\n";
                result += $"\tИдентификатор={cage.Uid}\n";
                result += $"\tИмя={cage.Name}\n";
                result += $"\tКоордината={cage.Position.PosX.ToString("F2").Replace(",", ".")}\n";
                result += $"\tПриблизительныйКоэффициентОпережения={cage.AdvanceRatio.ToString("F2").Replace(",", ".")}\n";
                result += $"\tПриблизительныйКоэффициентОтставания={cage.LagRatio.ToString("F2").Replace(",", ".")}\n";
                result += $"\tТипКлети={(cage.CageType==CagesType.StandTypeHorizontal ? "STAND_TYPE_HORIZONTAL" : "STAND_TYPE_VERTICAL")}\n";
                result += $"\tИдентификаторСигналаКлетьВРаботе={cage.SignalInWork}\n";
                result += $"\tИдентификаторСигналаСкорость={cage.SignalSpeed}\n";
                result += $"\tНомерНити={cage.ThreadNumber}\n";
                result += ")\n";
            }
            
            return result;
        }
    }
}