namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Products.Helpers;
    using Products.Models;
    using Products.Services;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class NewProductViewModel : INotifyPropertyChanged
    {
        #region Attributes

        public event PropertyChangedEventHandler PropertyChanged;
        private string _description;
        private string _price;
        private bool _isActive;
        private DateTime _lastPurchase;
        private string _stock;
        private string _remarks;
        private bool _isRunnig;
        private bool _isEnabled;
        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;
        private string _image;
        private ImageSource _imageSource;
        private MediaFile file;
        private decimal price;
        private double stock;

        #endregion

        #region Commands

        public ICommand SaveProductCommand
        {
            get
            {
                return new RelayCommand(SaveProduct);
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return new RelayCommand(Back);
            }
        }

        public ICommand ChangeImageCommand
        {
            get { return new RelayCommand(ChangeImage); }
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
                if (value != _description)
                {
                    _description = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Description)));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Price)));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(IsActive)));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(LastPurchase)));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Stock)));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Remarks)));
                }
            }
        }

        public string Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Image)));
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
                if (value != _isRunnig)
                {
                    _isRunnig = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(IsRunning)));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }

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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(ImageSource)));
                }
            }
        }

        public List<Product> Products { get; set; }

        #endregion

        #region Costructor

        public NewProductViewModel()
        {
            //  Activa los controles del formulario
            SetEnabledDisable(false, true);

            //  Define la fecha actual
            LastPurchase = DateTime.Now;

            //  Coloca activo el producto
            IsActive = true;

            //  Coloca el recurso (Imagen) NoImage
            //  Image = "noimage.png";
            ImageSource = "NoImage.png";

            //  Instancia los servicios
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Metodo asincrono que guarda los datos del producto
        /// </summary>
        private async void SaveProduct()
        {
            //  Valida los controles del formularios
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a product description...!!!");
                return;
            }

            // Valida si esta activo o no el producto
            if(IsActive == true)
            {
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

                stock = 0.00;
                if (!double.TryParse(Stock, out stock))
                {
                    await dialogService.ShowMessage(
                        "Error",
                        "You must enter a product stock in numeric value...!!!");
                    return;
                }
            }

            //if (string.IsNullOrEmpty(Remarks))
            //{
            //    await dialogService.ShowMessage(
            //        "Error",
            //        "You must enter a product remarks...!!!");
            //    return;
            //}

            //  Habilita el ActivityIndicator
            SetEnabledDisable(true, false);

            //  Verifica si el dispositio tiene o no acceso a internet
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //  Habilita el ActivityIndicator
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Optiene una instancia del MainViewModel para extraer los datos del Token
            var mainViewModel = MainViewModel.GetInstance();

            //  Envia el objeto MediaFile (file) al metodo para que devuelva un Array de byte[]
            byte[] imageArray = null;
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            //  Crea un objeto de tipo product
            var product = new Product
            {
                CategoryId = mainViewModel.Category.CategoryId,
                Description = Description,
                Price = price,
                ImageArray = imageArray,
                IsActive = IsActive,
                LastPurchase = LastPurchase,
                Stock = stock,
                Remarks = Remarks,
            };

            //  Invoca el metodo que hace el insert de datos (Api)
            var response = await apiService.Post(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/Products",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                product);

            //  Valida si hubo o no error en el WepAPI
            if (!response.IsSuccess)
            {
                //  Habilita el ActivityIndicator
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //  Crea una instancia del ProductViewModel
            var productViewModel = ProductsViewModel.GetInstance(Products);
            //  var productViewModel = ProductsViewModel.GetInstance();
            productViewModel.AddProduct((Product)response.Result);

            //  Habilita el ActivityIndicator
            SetEnabledDisable(false, true);

            //  Mensaje de sistema
            await dialogService.ShowMessage(
                "Information",
                string.Format(
                    "Product {0} add successful...!!!",
                    product.Description.Trim()));

            //  Hace el Back del NavigationService
            Back();
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

        /// <summary>
        /// Metodo que hace la navegacion hacia atras
        /// </summary>
        private async void Back()
        {
            //  await navigationService.Back();
            await navigationService.BackOnMaster();
        }

        /// <summary>
        /// Metodo que hace el browse de la camara o directorio de imagen
        /// </summary>
        private async void ChangeImage()
        {
            //  Inicializacion de Nuget
            await CrossMedia.Current.Initialize();

            //  Valida si tiene camara el dispositivo
            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                //  Llama al metodo que hace DisplayActionSheet
                var source = await dialogService.ShowImageOptions();

                //  Cancela el proceso
                if (source == "Cancel")
                {
                    file = null;
                    return;
                }

                //  Optiene la foto de la camera
                if (source == "From camera")
                {
                    file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    //  Optiene la foto de la galeria
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                //  Optiene la foto de la galeria
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        #endregion
    }
}
