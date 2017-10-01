namespace Products.ViewModels
{
    using System.ComponentModel;
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.Models;

    public class NewProductViewModel : INotifyPropertyChanged
    {
        #region Attributes

        public event PropertyChangedEventHandler PropertyChanged;
        private string _description;
        private string _price;
        private bool _isToggled;
        private DateTime _lastPurchase;
        private string _stock;
        private string _remarks;
        private bool _isRunnig;
        private bool _isEnabled;
        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

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

        public string Stock
        {
            get
            {
                return _stock;
            }
            set
            {
                if(value != _stock)
                { 
                    _stock = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stock)));
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
            apiService = new ApiService();
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
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a product price...!!!");
                return;
            }

            decimal price = 0.0m;
            if(!decimal.TryParse(Price, out price))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a product price in numeric value...!!!");
                return;
            }

            if(string.IsNullOrEmpty(Stock))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a product stock...!!!");
                return;
            }

            decimal stock = 0.0m;
            if(!decimal.TryParse(Stock, out stock))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a product stock in numeric value...!!!");
                return;
            }

            if(string.IsNullOrEmpty(Remarks))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a product remarks...!!!");
                return;
            }

            //  Habilita el ActivityIndicator
            SetEnabledDisable(true, false);

            //  Verifica si el dispositio tiene o no acceso a internet
            var connection = await apiService.CheckConnection();
            if(!connection.IsSuccess)
            {
                SetEnabledDisable(false, true); 
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Optiene una instancia del MainViewModel para extraer los datos del Token
            var mainViewModel = MainViewModel.GetInstance();

            //  Crea un objeto de tipo product
            var product = new Product
            {   
                Description = Description, 
                Image = "",
                Price = price,
                IsActive = IsToggled, 
                LastPurchase = LastPurchase,
                Stock = (double)stock,
                Remarks = Remarks,
            };

            //  Invoca el metodo que hace el insert de datos (Api)
            var response = await apiService.Post(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            //  Valida si hubo o no error en el WepAPI
            if(!response.IsSuccess)
            {
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //  Mensaje de sistema
            await dialogService.ShowMessage(
                "Information", 
                "Product add successful...!!!");

            //  Hace el Back del NavigationService
            await navigationService.Back();
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
