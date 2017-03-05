namespace Thermostat.Devices.RaspberryPi
{
    public interface IGpioService
    {
        void Init();

        void SetCoolingPinOn(bool on = true);

        void SetHeatingPinOn(bool on = true);

        void SetPowerPinOn(bool on = true);
    }
}