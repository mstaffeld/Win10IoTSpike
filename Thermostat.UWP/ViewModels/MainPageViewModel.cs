using Thermostat.Data;
using Thermostat.Devices.RaspberryPi;
using Thermostat.UWP.ViewModels.ViewModels;

namespace Thermostat.UWP.ViewModels
{
    public class MainPageViewModel : NotificationBase
    {
        private readonly IGpioService _gpioService;

        public MainPageViewModel(IGpioService gpioService)
        {
            _gpioService = gpioService;
        }

        public FurnaceState FurnaceState
        {
            get { return _furnaceState; }
            set
            {
                _furnaceState = value;
                RaisePropertyChanged(nameof(FurnaceState));
            }
        }

        public bool IsPowerOn
        {
            get { return _isPowerOn; }
            set
            {
                _isPowerOn = value;
                RaisePropertyChanged(nameof(IsPowerOn));

                this.IsCoolButtonEnabled = _isPowerOn;
                this.IsHeatButtonEnabled = _isPowerOn;
            }
        }

        private int _setTemp;
        private FurnaceState _furnaceState;
        private bool _isPowerOn;
        private bool _isCoolButtonEnabled;
        private bool _isHeatButtonEnabled;
        private bool _isUpButtonEnabled;
        private bool _isDownButtonEnabled;

        public int SetTemp
        {
            get { return _setTemp; }
            set
            {
                _setTemp = value;
                RaisePropertyChanged(nameof(SetTemp));
            }
        }

        public bool IsHeatButtonEnabled
        {
            get { return _isHeatButtonEnabled; }
            set
            {
                _isHeatButtonEnabled = value; 
                RaisePropertyChanged(nameof(IsHeatButtonEnabled));
            }
        }

        public bool IsCoolButtonEnabled
        {
            get { return _isCoolButtonEnabled; }
            set
            {
                _isCoolButtonEnabled = value; 
                RaisePropertyChanged(nameof(IsCoolButtonEnabled));
            }
        }

        public bool IsUpButtonEnabled
        {
            get { return _isUpButtonEnabled; }
            set
            {
                _isUpButtonEnabled = value; 
                RaisePropertyChanged(nameof(IsUpButtonEnabled));
            }
        }

        public bool IsDownButtonEnabled
        {
            get { return _isDownButtonEnabled; }
            set
            {
                _isDownButtonEnabled = value; 
                RaisePropertyChanged(nameof(IsDownButtonEnabled));
            }
        }

        private void ToggleMode()
        {
            if (this.FurnaceState == FurnaceState.Heating)
            {
                this.IsHeatButtonEnabled = false;
                this.IsCoolButtonEnabled = true;
                this._gpioService.SetHeatingPinOn();
            }

            if (this.FurnaceState == FurnaceState.Cooling)
            {
                this.IsHeatButtonEnabled = true;
                this.IsCoolButtonEnabled = false;
                this._gpioService.SetCoolingPinOn();
            }

            if (this.FurnaceState == FurnaceState.Disabled)
            {
                this._gpioService.SetHeatingPinOn(false);
                this._gpioService.SetCoolingPinOn(false);
                this.IsCoolButtonEnabled = false;
                this.IsHeatButtonEnabled = false;
            }

            this.ToggleSetButtons();

        }

        public void SetFurnaceState(FurnaceState state)
        {
            this.FurnaceState = state;

            this.ToggleMode();
        }

        private void ToggleSetButtons()
        {
            if (this.FurnaceState != FurnaceState.Disabled)
            {
                this.IsUpButtonEnabled = true;
                this.IsDownButtonEnabled = true;
            }
            else
            {
                this.IsUpButtonEnabled = false;
                this.IsDownButtonEnabled = false;
            }
        }

        public void ToggleMainPower()
        {
            if (this.IsPowerOn)
            {
                this.IsPowerOn = false;
                this.SetFurnaceState(FurnaceState.Disabled);
            }
            else
            {
                this.IsPowerOn = true;
            }
        }
    }
}
