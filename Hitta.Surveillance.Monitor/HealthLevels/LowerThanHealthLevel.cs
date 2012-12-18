using Hitta.Surveillance.Monitor.ColorCoders;

namespace Hitta.Surveillance.Monitor.HealthLevels
{
    public class LowerThanHealthLevel:HealthLevelBase
    {

        public LowerThanHealthLevel(double controlValue, ColorCoder colorCoder) : base(controlValue, colorCoder)
        {
        }

        protected override ColorCoder GetColorCoderInternal(double value)
        {
            return value <= ControlValue ? ColorCoder : null;
        }
    }
}
