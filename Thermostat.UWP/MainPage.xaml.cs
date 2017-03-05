using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Thermostat.UWP
{
    using System.Threading.Tasks;

    using Windows.Devices.Gpio;

    using Thermostat.Data;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
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

        private bool PowerIsOn;

        private FurnaceState furnaceState = FurnaceState.Disabled;

        public MainPage()
        {
            this.InitializeComponent();

            this.InitControls();

            this.InitPowerPin();
            this.InitCoolingPin();
            this.InitHeatingPin();
        }

        private void InitControls()
        {
           this.MainPower.Content = "Power On";
           this.ToggleControls();
        }

        private void ToggleControls()
        {
            this.HeatMode.IsEnabled = this.PowerIsOn; // UI
            this.CoolMode.IsEnabled = this.PowerIsOn; // UI
        }

        private void MainPower_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleMainPower();
            this.MainPower.Content = this.PowerIsOn ? "Power Off" : "Power On";
        }

        private void ToggleMainPower()
        {
            if (this.PowerIsOn)
            {
                this.PowerIsOn = false;
                this.SetFurnaceState(FurnaceState.Disabled);
            }
            else
            {
                this.PowerIsOn = true;
            }

            this.ToggleMainPowerLed();
            this.ToggleControls();
        }

        private void ToggleMainPowerLed()
        {
            if (this.PowerIsOn)
            {
                this.powerPinValue = GpioPinValue.High;
                this.powerPin.Write(this.powerPinValue);
            }
            else
            {
                this.powerPinValue = GpioPinValue.Low;
                this.powerPin.Write(this.powerPinValue);
            }
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

        private void HeatMode_Click(object sender, RoutedEventArgs e)
        {
            this.HeatMode.IsEnabled = false; // UI
            this.CoolMode.IsEnabled = true; // UI

            this.SetFurnaceState(FurnaceState.Heating);
        }

        private void CoolMode_Click(object sender, RoutedEventArgs e)
        {
            this.CoolMode.IsEnabled = false; // UI
            this.HeatMode.IsEnabled = true; // UI

            this.SetFurnaceState(FurnaceState.Cooling);
        }

        private void SetFurnaceState(FurnaceState state)
        {
            this.furnaceState = state;

            this.ToggleCoolingPin();
            this.ToggleHeatingPin();
        }

        private void ToggleHeatingPin()
        {
            if (this.furnaceState == FurnaceState.Heating)
            {
                this.heatingPinValue = GpioPinValue.High;
                this.heatingPin.Write(this.heatingPinValue);
            }
            else
            {
                this.heatingPinValue = GpioPinValue.Low;
                this.heatingPin.Write(this.heatingPinValue);
            }
        }

        private void ToggleCoolingPin()
        {
            if (this.furnaceState == FurnaceState.Cooling)
            {
                this.coolingPinValue = GpioPinValue.High;
                this.coolingPin.Write(this.coolingPinValue);
            }
            else
            {
                this.coolingPinValue = GpioPinValue.Low;
                this.coolingPin.Write(this.coolingPinValue);
            }
        }

        private void RaiseTemp_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LowerTemp_OnClickTemp_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
