using System;

namespace MyVegas
{
    internal class Object
    {
        public Object(string name, double probability, CustomBox box ) 
        { 
            Name = name;
            Probability = probability;
            Position = box;
        }

        public static double Threashold = 0.5;
        public string Name { get;}
        public double Probability { get;}
        public CustomBox Position { get;}

        public int GetCenterX()
        {
            return Int32.Parse(Math.Round((1920 * Position.Left) + ((1920 * Position.Width) / 2)).ToString());
        }

        public int GetCenterY()
        {
            return Int32.Parse(Math.Round((1080 * Position.Top) + ((1080 * Position.Height) / 2)).ToString());
        }
    }
}
