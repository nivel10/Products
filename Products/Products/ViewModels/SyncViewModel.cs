namespace Products.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Helpers;
    using Products.Models;
    using Products.Services;

    public class SyncViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private string _message;
        private bool _isEnabled;
        private bool _isRunning;

        public event PropertyChangedEventHandler PropertyChanged;

        private ApiService apiService;
        private DataService dataService;
        private DialogService dialogService;
        private NavigationService navigationService;

        #endregion Attributes

        #region Properties

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if(value != _message)
                {
                    _message = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(Message));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(IsEnabled)));
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
                if(value != _isRunning)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        #region Commands

        public ICommand SyncCommand
        {
            get { return new RelayCommand(Sync); }
        }

        public ICommand BackCommand
        {
            get { return new RelayCommand(Back); }
        }

        #endregion Commands

        #endregion

        #region Constructor

        public SyncViewModel()
        {
            //  Instancia las clase de los services
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            //  Mensaje de sistema
            Message = "Press sync button to start";

            //  Invoca el metodo que habilita - deshabilita los controles
            SetEnabledDisable(false, true);
        }

        #endregion Construnctor

        #region Methods

        private async void Sync()
        {
            //  Invoca el metodo que habilita - deshabilita los controles
            SetEnabledDisable(true, false);

            //  Valida que haya conexion del dispositivo
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //  Invoca el metodo que habilita - deshabilita los controles
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Optiene la List<Product>
            var products = dataService.Get<Product>(false)
                                      .Where(p => p.PendingToSave == true)
                                      .ToList();
            if (products.Count == 0)
            {
                //  Invoca el metodo que habilita - deshabilita los controles
                SetEnabledDisable(false, true);

                await dialogService.ShowMessage(
                    "Information",
                    "There are not products to sync...!!! ");
                Back();
            }
            else
            {
                SaveProducts(products);
            }

            //  Invoca el metodo que habilita - deshabilita los controles
            SetEnabledDisable(false, true);
        }

        private async void SaveProducts(List<Product> products)
        {
            //  Recorre el List<Product>
            foreach (var product in products)
            {

                //  Invoca el metodo que hace el insert de datos (Api)
                var response = await apiService.Post(
                    MethodsHelper.GetUrlAPI(),
                    "/api",
                    "/Products",
                    MainViewModel.GetInstance().Token.TokenType,
                    MainViewModel.GetInstance().Token.AccessToken,
                    products);

                //  Valida si hubo o no error en el WepAPI
                if (response.IsSuccess)
                {
                    //  Actualiza el registro cargado en el API
                    product.PendingToSave = false;
                    dataService.Update(product);
                }
            }

            await dialogService.ShowMessage(
                "Confirmation",
                "Sync Status Ok...!!!");
            
            Back();
        }

        private async void Back()
        {
            await navigationService.BackOnMaster();
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
