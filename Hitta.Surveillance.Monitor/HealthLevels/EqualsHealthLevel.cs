using Hitta.Surveillance.Monitor.ColorCoders;

namespace Hitta.Surveillance.Monitor.HealthLevels
{
    public class EqualsHealthLevel:HealthLevelBase
    {
        public EqualsHealthLevel(double controlValue, ColorCoder colorCoder):base(controlValue, colorCoder)
        {
        }

        protected override ColorCoder GetColorCoderInternal(double value)
        {
            return value == ControlValue ? ColorCoder : null;
        }
    }
}
