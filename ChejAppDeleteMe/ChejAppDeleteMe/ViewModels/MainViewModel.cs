namespace ChejAppDeleteMe.ViewModels
{
    using ChejAppDeleteMe.Models;
    using ChejAppDeleteMe.Services;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Commands

        public ICommand ConvertCommand
        {
            get { return new RelayCommand(Convert); }
        }

        #endregion Commands

        #region Attributes

        private string _messageResult;
        private string _amount;
        private bool _isEnabled;
        private bool _isRunning;
        private string _result;
        private List<Rate> _rate;
        private string _sourceRate;
        private string _targetRate;
        private ApiService apiService;
        private DialogService dialogService;

        #endregion Attributes

        #region Properties

        public string Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        //  Se crea ObservableCollection para qeu los cambios que se hagan en la lista (List<Rate>) sean observable por el usuario  \\
        public ObservableCollection<Rate> Rates
        {
            get;
            set;
        }

        public Rate SourceRate
        {
            get;
            set;

            //get
            //{
            //    return _sourceRate;
            //}
            //set
            //{
            //    if (value != _sourceRate)
            //    {
            //        _sourceRate = value;
            //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceRate)));
            //    }
            //}
        }

        public Rate TargetRate
        {
            get;
            set;

            //get
            //{
            //    return _targetRate;
            //}
            //set
            //{
            //    if (value != _targetRate)
            //    {
            //        _targetRate = value;
            //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetRate)));
            //    }
            //}
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (value != _isRunning)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (value != _result)
                {
                    _result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
        }

        #endregion Properties

        #region Constructor

        public MainViewModel()
        {
            //  Instancia de los Services   \\
            apiService = new ApiService();
            dialogService = new DialogService();

            _messageResult = "Enter an mount, select source rate, ";
            _messageResult += "select target rate and press convert button";

            //  Instancia de los ObservableCollection   \\
            Rates = new ObservableCollection<Rate>();

            //  Invoca el metodo que hace la carga de datos de las tasas (Rates)    \\
            LoadRates();
        }

        #endregion Constructor

        #region Methods

        private async void LoadRates()
        {
            try
            {
                IsEnabled = false;
                IsRunning = true;
                Result = "Loading rates, please wait...!!!";

                //  Invoca el metodo que hace la busqueda de las tasas  (Rates) \\
                var response = await apiService.GetRates();

                if (!response.IsSuccess)
                {
                    IsRunning = false;
                    await dialogService.ShowMessage("Error", response.Message, "Accept");
                    return;
                }

                IsEnabled = true;
                IsRunning = false;

                Result = _messageResult;

                //  Captura el objeto List<Rate>    \\
                _rate = (List<Rate>)response.Result;

                //  invoca el metodo que hace la carga de datos de las tasas (Rate) \\
                ReloadRates(_rate);

                await dialogService.ShowMessage("Information", "CHEJ Consultor, C.A.", "Accept");
            }
            catch (Exception ex)
            {
                IsEnabled = false;
                IsRunning = false;
                await dialogService.ShowMessage("Error", ex.Message.Trim(), "Accept");
            }
        }

        /// <summary>
        /// Metodo que hace la carga de las tasas (Rates)
        /// </summary>
        /// <param name="rates">Obketo de tipo List(Rate)</param>
        private void ReloadRates(List<Rate> rates)
        {
            Rates.Clear();

            foreach (var rate in rates)
            {
                Rates.Add(new Rate
                {
                    Code = rate.Code,
                    Name = rate.Name,
                    RateId = rate.RateId,
                    TaxRate = rate.TaxRate,
                });
            }
        }

        private async void Convert()
        {
            try {
                if (string.IsNullOrEmpty(Amount))
                {
                    await dialogService.ShowMessage("Error", "Enter the amount to convert...!!!", "Accept");
                    return;
                }

                var amount = 0.0m;
                if(!decimal.TryParse(Amount, out amount))
                {
                    await dialogService.ShowMessage("Error", "Enter the amount numeric to convert...!!!", "Accept");
                    Amount = null;
                    return;
                }

                //if (string.IsNullOrEmpty(SourceRate) || int.Parse(SourceRate) == 0)
                //{
                //    await dialogService.ShowMessage("Error", "Select source rate...!!!", "Accept");
                //    return;
                //}
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message.Trim(), "Accept");
            }
        }

        #endregion Methods
    }
}
