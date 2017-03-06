using System;

using Thermostat.Data;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Thermostat.UWP
{
    using Devices.RaspberryPi;

    using Windows.UI.Xaml.Media;

    using Thermostat.Devices.Fakes;

    using GpioService = Thermostat.Devices.RaspberryPi.GpioService;

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

            this.InitTemperatureSampling();
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
            this.LogOutput.Text += "Raising Temperature +1" + Environment.NewLine;
        }

        private void LowerTemp_OnClick(object sender, RoutedEventArgs e)
        {
            this.DesiredTemperatureF -= 1;
            this.LogOutput.Text += "Lowering Temperature -1" + Environment.NewLine;

        }

        private void LogOutput_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var grid = (Grid)VisualTreeHelper.GetChild(this.LogOutput, 0);
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);
                if (!(obj is ScrollViewer)) continue;
                ((ScrollViewer)obj).ChangeView(0.0f, ((ScrollViewer)obj).ExtentHeight, 1.0f);
                break;
            }
        }

        // every x seconds, sample the TemperatureSensorService and log the temp
        private void InitTemperatureSampling()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            timer.Tick += this.SampleTemperature;
            timer.Start();
        }

        private void SampleTemperature(object sender, object e)
        {
            var tempService = new TemperatureSensorService();
            var insideTemperatureF = tempService.GetInsideTemperatureF();
            var outsideTemperatureF = tempService.GetOutsideTemperatureF();

            this.LogOutput.Text += string.Format("Inside temperature: {0}{1} ", insideTemperatureF, Environment.NewLine);
            this.LogOutput.Text += string.Format("Outisde temperature: {0}{1} ", outsideTemperatureF, Environment.NewLine);
        }
    }
}
