namespace Products.ViewModels
{
    using System.ComponentModel;
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;

    public class NewProductViewModel : INotifyPropertyChanged
    {

        #region Attributes

        public event PropertyChangedEventHandler PropertyChanged;
        private string _description;
        private string _price;
        private bool _isToggled;
        private DateTime _lastPurchase;
        private string _remarks;
        private bool _isRunnig;
        private bool _isEnabled;
        private DialogService dialogService;

        #endregion

        #region Commands

        public ICommand SaveProductCommand
        {
            get 
            { 
                return new RelayCommand(SaveProduct); 
            }
        }

        #endregion

        #region Properties

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if(value != _description)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                if(value != _price)
                {
                    _price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
            }
        }

        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if(value != _isToggled)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsToggled)));
                }
            }
        }

        public DateTime LastPurchase
        {
            get
            {
                return _lastPurchase;
            }
            set
            {
                if(value != _lastPurchase)
                {
                    _lastPurchase = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastPurchase)));
                }
            }
        }

        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                if(value != _remarks)
                {
                    _remarks = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Remarks)));
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunnig;
            }
            set
            {
                if(value != _isRunnig)
                {
                    _isRunnig = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if(value != _isEnabled)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

        #endregion

        #region Costructor

        public NewProductViewModel()
        {
            //  Activa los controles del formulario
            SetEnabledDisable(false, true);

            //  Define la fecha actual
            LastPurchase = DateTime.Now;

            //  Instancia los servicios
            dialogService = new DialogService();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Metodo asincrono que guarda los datos del producto
        /// </summary>
        private async void SaveProduct()
        {
            //  Valida los controles del formularios
            if(string.IsNullOrEmpty(Description))
            { 
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a product description...!!!");
                return;
            }

            if(string.IsNullOrEmpty(Price))
            {
                await dialogService.ShowMessage("Error", "You must enter a product price numeric...!!!");
                return;
            }
            decimal price;
            price = 0.0m;
            if(!decimal.TryParse(Price, out price))
            {
                await dialogService.ShowMessage("Error", "You must enter a product price numeric...!!!");
                return;
            }

        }

        /// <summary>
        /// Metodo qua habilita o deshabilita los controles del formulario
        /// </summary>
        /// <param name="isRunning">Bool que indica si el ActivityIndicator esta activo o no</param>
        /// <param name="isEnabled">Bool que indica si los controles estan activo o no</param>
        private void SetEnabledDisable(bool isRunning, bool isEnabled)
        {
            IsRunning = isRunning;
            IsEnabled = isEnabled;
        }

        #endregion
    }
}
