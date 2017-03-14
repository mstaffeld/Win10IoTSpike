using System;

using Thermostat.Data;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Thermostat.UWP.ViewModels;

namespace Thermostat.UWP
{
    using Devices.RaspberryPi;

    using Windows.UI.Xaml.Media;

    using Thermostat.Devices.Fakes;

    using GpioService = Thermostat.Devices.RaspberryPi.GpioService;

    public sealed partial class MainPage : Page
    {
        private MainPageViewModel vm;
        public MainPage()
        {
            // todo: feature toggle
            this.pinService = new Devices.Fakes.GpioService();

            vm = new MainPageViewModel(this.pinService);
            DataContext = vm;

            this.InitializeComponent();

            this.InitControls();

            this.InitTemperatureSampling();

            // todo: default convenience from configs
            vm.SetTemp = 60;
        }

        private readonly IGpioService pinService;

        private void InitControls()
        {
           this.MainPower.Content = "Power On";
        }

        private void MainPower_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleMainPower();
            this.MainPower.Content = this.vm.IsPowerOn ? "Power Off" : "Power On"; // todo: mvvm
        }

        private void ToggleMainPower()
        {
            this.vm.ToggleMainPower();
            this.ToggleMainPowerLed();
        }

        private void ToggleMainPowerLed()
        {
            this.pinService.SetPowerPinOn(this.vm.IsPowerOn);
        }

        private void HeatMode_Click(object sender, RoutedEventArgs e)
        {
            this.vm.SetFurnaceState(FurnaceState.Heating);
        }

        private void CoolMode_Click(object sender, RoutedEventArgs e)
        {
            this.vm.SetFurnaceState(FurnaceState.Cooling);
        }

        private void RaiseTemp_OnClick(object sender, RoutedEventArgs e)
        {
            this.vm.SetTemp += 1;
            this.LogOutput.Text += "Raising Temperature +1" + Environment.NewLine;
        }

        private void LowerTemp_OnClick(object sender, RoutedEventArgs e)
        {
            this.vm.SetTemp -= 1;
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
