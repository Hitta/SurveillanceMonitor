using System;
using Hitta.Surveillance.Monitor.ColorCoders;

namespace Hitta.Surveillance.Monitor.HealthLevels
{
    public abstract class HealthLevelBase :HealthLevel
    {
        HealthLevel successor;
        protected readonly double ControlValue;
        protected readonly ColorCoder ColorCoder;

        protected HealthLevelBase(double controlValue, ColorCoder colorCoder)
        {
            this.ControlValue = controlValue;
            this.ColorCoder = colorCoder;
        }

        protected abstract ColorCoder GetColorCoderInternal(double value);

        public HealthLevel Successor
        {
            set { successor = value; }
        }

        public ColorCoder GetColorCoder(double value)
        {
            var colorCoder = GetColorCoderInternal(value);

            if (colorCoder != null) return colorCoder;

            return successor != null ? successor.GetColorCoder(value) : new ColorCoderNormal();
        }
    }
}
