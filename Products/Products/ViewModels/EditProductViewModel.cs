namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Products.Services;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditProductViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private Product product;
        private string _description;
        private string _price;
        private bool _isActive;
        private DateTime _lastPurchase;
        private string _stock;
        private string _remarks;
        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;
        private bool _isRunning;
        private bool _isEnabled;
        private decimal stock;
        private decimal price;
        private ImageSource _imageSource;

        #endregion Attributes

        #region Commands

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand BackCommand
        {
            get { return new RelayCommand(Back); }
        }

        #endregion  Commands

        #region Event

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Event

        #region Properties

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
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
                if (value != _price)
                {
                    _price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
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
                if (value != _lastPurchase)
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
                if (value != _stock)
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
                if (value != _remarks)
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

        public List<Product> Products { get; set; }

        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                if (value != _imageSource)
                {
                    _imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
        }

        #endregion Properties

        #region Constructor

        public EditProductViewModel(Product product)
        {
            //  Habilita el ActivityIndicator
            SetEnabledDisable(true, false);

            this.product = product;

            //  Genera la instancia de cada clase (Service)
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            //  Invoca el metodo que hace la carga de datos en cada control
            LoadProduct(this.product);

            //  Habilita el ActivityIndicator
            SetEnabledDisable(false, true);
        }

        #endregion  Constructor

        #region Methods

        /// <summary>
        /// Metodo que hace la carga de datos en los controles
        /// </summary>
        /// <param name="product">Objeto Product</param>
        private void LoadProduct(Product product)
        {
            Description = product.Description;
            Price = product.Price.ToString().Trim();
            ImageSource = product.ImageFullPath;
            IsActive = product.IsActive;
            LastPurchase = product.LastPurchase;
            Stock = product.Stock.ToString().Trim();
            Remarks = product.Remarks;
        }

        /// <summary>
        /// Captura todos los datos en el objeto Product
        /// </summary>
        /// <param name="product">Objeto Product</param>
        private void LoadValueInProduct(Product product)
        {
            product.Description = Description;
            product.Price = price;
            product.IsActive = IsActive;
            product.LastPurchase = LastPurchase;
            product.Stock = (double)stock;
            product.Remarks = Remarks;
        }

        /// <summary>
        /// Metodo que guarda las modificaciones del producto
        /// </summary>
        private async void Save()
        {
            //  Valida los controles del formularios
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a product description...!!!");
                return;
            }

            if (string.IsNullOrEmpty(Price))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a product price...!!!");
                return;
            }

            price = 0.0m;
            if (!decimal.TryParse(Price, out price))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a product price in numeric value...!!!");
                return;
            }

            if (string.IsNullOrEmpty(Stock))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a product stock...!!!");
                return;
            }

            stock = 0.0m;
            if (!decimal.TryParse(Stock, out stock))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a product stock in numeric value...!!!");
                return;
            }

            if (string.IsNullOrEmpty(Remarks))
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
            if (!connection.IsSuccess)
            {
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Asigna todos los valores al objeto Product
            LoadValueInProduct(product);

            //  Optiene una instancia de la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();

            //  Invoca el metodo Apiservice Update
            var response = await apiService.Put(
                "http://productszuluapi.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            //  Valida si hubo o no error en el metodo anterior
            if (!response.IsSuccess)
            {
                //  Habilita el ActivityIndicator
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            //  Invoca una instancia del ProductViewModel
            var productsViewModel = ProductsViewModel.GetInstance(Products);
            //  var productsViewModel = ProductsViewModel.GetInstance();
            productsViewModel.UpdateProduct((Product)response.Result);

            //  Habilita el ActivityIndicator
            SetEnabledDisable(false, true);

            await dialogService.ShowMessage(
                "Information",
                string.Format(
                    "Product {0} is updated....!!!",
                    product.Description.Trim()));

            //  Navega a la pagina anterior
            await navigationService.Back();
        }

        /// <summary>
        /// Metodo que hace la navegacion hacia atras
        /// </summary>
        private async void Back()
        {
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

        #endregion Methods
    }
}
