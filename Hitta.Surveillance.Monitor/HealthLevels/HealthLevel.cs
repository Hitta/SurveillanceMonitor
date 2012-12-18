namespace Hitta.Surveillance.Monitor.HealthLevels
{
    public interface HealthLevel
    {
        HealthLevel Successor{ set;}

        ColorCoders.ColorCoder GetColorCoder(double value);
    }
}
