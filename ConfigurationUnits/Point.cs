namespace ConfigParser.ConfigurationUnits
{
    public class Point
    {
        public double PosX { get; set; }
        public double PosY { get; set; }

        public Point()
        {
            PosX = default;
            PosY = default;
        }

        public Point(double posX, double posY)
        {
            PosX = posX;
            PosY = posY;
        }
    }
}