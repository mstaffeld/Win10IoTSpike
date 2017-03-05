namespace Thermostat.Devices.Fakes
{
    using Thermostat.Devices.RaspberryPi;

    public class GpioService : IGpioService
    {
        public void Init()
        {
            // NO OP
        }

        public void SetCoolingPinOn(bool @on = true)
        {
            // NO OP
        }

        public void SetHeatingPinOn(bool @on = true)
        {
            // NO OP
        }

        public void SetPowerPinOn(bool @on = true)
        {
            // NO OP
        }
    }
}
