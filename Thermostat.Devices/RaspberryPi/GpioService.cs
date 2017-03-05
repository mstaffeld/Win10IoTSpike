namespace Thermostat.Devices.RaspberryPi
{
    using Windows.Devices.Gpio;

    public class GpioService : IGpioService
    {
        private const int POWER_PIN = 5;
        private GpioPin powerPin;
        private GpioPinValue powerPinValue;

        private const int COOLING_PIN = 6;
        private GpioPin coolingPin;
        private GpioPinValue coolingPinValue;

        private const int HEATING_PIN = 12;
        private GpioPin heatingPin;
        private GpioPinValue heatingPinValue;

        public void Init()
        {
            this.InitPowerPin();
            this.InitCoolingPin();
            this.InitHeatingPin();
        }

        public void SetPowerPinOn(bool on = true)
        {
            this.powerPin.Write(@on ? GpioPinValue.High : GpioPinValue.Low);
        }

        public void SetCoolingPinOn(bool on = true)
        {
            this.coolingPin.Write(@on ? GpioPinValue.High : GpioPinValue.Low);
        }

        public void SetHeatingPinOn(bool on = true)
        {
            this.heatingPin.Write(@on ? GpioPinValue.High : GpioPinValue.Low);
        }

        private void InitPowerPin()
        {
            var gpio = GpioController.GetDefault();
            this.powerPin = gpio.OpenPin(POWER_PIN);
            this.powerPinValue = GpioPinValue.Low;
            this.powerPin.Write(this.powerPinValue);
            this.powerPin.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void InitCoolingPin()
        {
            var gpio = GpioController.GetDefault();
            this.coolingPin = gpio.OpenPin(COOLING_PIN);
            this.coolingPinValue = GpioPinValue.Low;
            this.coolingPin.Write(this.coolingPinValue);
            this.coolingPin.SetDriveMode(GpioPinDriveMode.Output);
        }

        private void InitHeatingPin()
        {
            var gpio = GpioController.GetDefault();
            this.heatingPin = gpio.OpenPin(HEATING_PIN);
            this.heatingPinValue = GpioPinValue.Low;
            this.heatingPin.Write(this.heatingPinValue);
            this.heatingPin.SetDriveMode(GpioPinDriveMode.Output);
        }


    }
}
