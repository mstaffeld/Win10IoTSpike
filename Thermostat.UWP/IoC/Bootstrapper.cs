using SimpleInjector;
using Thermostat.Devices.RaspberryPi;

namespace Thermostat.UWP.IoC
{
    public static class Bootstrapper
    {
        public static Container Bootstrap()
        {
            var container = new Container();
            container.Register<IGpioService, Devices.Fakes.GpioService>();

            container.Verify();

            return container;
        }
    }
}
