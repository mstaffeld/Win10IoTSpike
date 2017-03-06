using System;

using Thermostat.Data;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Thermostat.UWP
{
    using Devices.RaspberryPi;

    public sealed partial class MainPage : Page
    {
        private bool PowerIsOn;

        private readonly IGpioService pinService;
        private FurnaceState furnaceState = FurnaceState.Disabled;

        private int desiredTemperatureF;

        public int DesiredTemperatureF
        {
            get
            {
                return this.desiredTemperatureF;
            }

            set
            {
                this.desiredTemperatureF = value;
                this.SetTemp.Text = this.desiredTemperatureF.ToString();
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            this.InitControls();

            this.DesiredTemperatureF = 67; // 2 digit max?

            // poor mans feature toggle, need to do some more setup yet
            bool hardwareConnected = false;
            if (hardwareConnected)
            {
                this.pinService = new GpioService();
                this.pinService.Init();
            }
            else
            {
                this.pinService = new Devices.Fakes.GpioService();
            }
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
            this.pinService.SetPowerPinOn(this.PowerIsOn);
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
            this.pinService.SetHeatingPinOn(this.furnaceState == FurnaceState.Heating);
        }

        private void ToggleCoolingPin()
        {
            this.pinService.SetCoolingPinOn(this.furnaceState == FurnaceState.Cooling);
        }

        private void RaiseTemp_OnClick(object sender, RoutedEventArgs e)
        {
            this.DesiredTemperatureF += 1;
        }

        private void LowerTemp_OnClick(object sender, RoutedEventArgs e)
        {
            this.DesiredTemperatureF -= 1;
        }
    }
}
