using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ConfigParser.Types;

namespace ConfigParser.ConfigurationUnits
{
    public class IngotUnit : BaseConfigUnit
    {
        public Point StartPos { get; set; }

        public Point FinishPos { get; set; }

        public ushort ThreadUid { get; set; }

        [JsonIgnore]
        public uint Rid { get; set; }

        [JsonIgnore]
        public uint L3id { get; set; }

        public uint Bid { get; set; }

        public double XCentre => Math.Abs(StartPos.PosX + FinishPos.PosX) / 2.0;

        [JsonIgnore]
        public double YCentre => Math.Abs(StartPos.PosY + FinishPos.PosY) / 2.0;

        public double Length => Math.Abs(FinishPos.PosX - StartPos.PosX);
        
        [JsonIgnore]
        public double Width => Math.Abs(FinishPos.PosY - StartPos.PosY);

        public List<IngotType> TypeInfo { get; set; }

        [JsonIgnore]
        public double InputWeight { get; set; }

        [JsonIgnore]
        public double OutputWeight { get; set; }

        public ConsoleColor Color { get; set; }

        public int PlavNumber { get; set; }

        public string Model { get; set; }

        public Point CoordinateOrigin { get; set; }

        public Point CoordinatesEnd { get; set; }

        public int Number { get; set; }

        public bool StopAtEnds { get; set; }

        public IngotUnit()
        {
            Uid = default;
            Rid = default;
            Bid = default;
            L3id = default;
            ThreadUid = default;
            InputWeight = default;
            OutputWeight = default;
            Model = default;
            Number = default;
            StopAtEnds = true;
            PlavNumber = default;
            StartPos = new Point();
            FinishPos = new Point();
            CoordinateOrigin = new Point();
            CoordinatesEnd = new Point();
            TypeInfo = new List<IngotType>();
            
            int color = new Random().Next(15);
            Color = (ConsoleColor)color;
        }

        // public override string ToString()
        // {
        //     string result = "ЕдиницаУчета\n(\n";
        //     
        //     result += $"\tИдентификатор={Uid}\n";
        //     result += $"\tИмя={Name}\n";
        //     result += $"\tИдентификаторБД={Bid}\n";
        //     result += $"\tИдентификаторL3={L3id}\n";
        //     result += $"\tКоординатаНачала={StartPos.PosX.ToString("F2").Replace(",", ".")}\n";
        //     result += $"\tКоординатаЗавершения={FinishPos.PosX.ToString("F2").Replace(",",".")}\n";
        //
        //     result += ")\n";
        //     return result;
        // }
    }
}