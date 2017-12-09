namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Products.Helpers;
    using Products.Models;
    using Products.Services;
    using System.ComponentModel;
    using System.Windows.Input;

    public class NewCustomerViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _address;
        private string _password;
        private string _confirm;
        private bool _isEnabled;
        private bool _isRunning;

        #region Event

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Event

        #region Services

        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;

        #endregion Services

        #endregion Attributes

        #region Properties

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Phone)));
                }
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != _address)
                {
                    _address = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Address)));
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        public string Confirm
        {
            get
            {
                return _confirm;
            }
            set
            {
                if (value != _confirm)
                {
                    _confirm = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Confirm)));
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
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        #endregion Properties

        #region Commands

        public ICommand SaveCommand
        {
            get { return new RelayCommand(save); }
        }

        public ICommand BackCommand
        {
            get { return new RelayCommand(Back); }
        } 

        #endregion Commands

        #region Constructor

        public NewCustomerViewModel()
        {
            //  Genera un objeto de las clases (Services)
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            //  Invoca el metodo que habilita / deshabilita controles
            SetEnabledDisable(false, true);
        }

        #endregion Constructor

        #region Metods

        /// <summary>
        /// Metodo que hace el guardar (Save) del Customer
        /// </summary>
        private async void save()
        {
            //  Valida los controles del formulario
            if (string.IsNullOrEmpty(FirstName))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a first name...!!!");
                return;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a last name...!!!");
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must a email...!!!");
                return;
            }
            if (!RegexUtilities.IsValidEmail(Email))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a valid email...!!!");
                return;
            }
            //  Son datos no obligatorios
            //if (string.IsNullOrEmpty(Phone))
            //{
            //    await dialogService.ShowMessage("Error", "You must enter a phone...!!!");
            //    return;
            //}
            //if (string.IsNullOrEmpty(Address))
            //{
            //    await dialogService.ShowMessage("Error", "You must enter a address...!!!");
            //    return;
            //}
            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a password...!!!");
                return;
            }
            if (Password.Length < 6)
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "The password must have at least 6 characters length...!!!");
                return;
            }
            if (string.IsNullOrEmpty(Confirm))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must enter a password confirm...!!!");
                return;
            }
            if (Confirm.Length < 6)
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "The password confirm must hace at least 6 characters length...!!!");
                return;
            }
            if (!Password.Equals(Confirm))
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "The password and confirm, does not match...!!!");
                return;
            }

            //  Inicia el ActivityIndicator
            SetEnabledDisable(true, false);

            //  Valida si el dispositivo tiene conexion
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //  Detiene el ActivityIndicator
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("", connection.Message);
                return;
            }

            //  Crea un objeto de la clase Customer
            var customer = new Customer
            {
                Address = Address,
                CustomerType = 1,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                Phone = Phone,
            };

            //  Invoca el Post (Insert) del API
            var response = await apiService.Post(
                "http://chejconsultor.ddns.net:9015",
                "/api",
                "/Customers",
                customer);

            //  Valida el resultado del metodo anterior
            if (!response.IsSuccess)
            {
                //  Detiene el ActivityIndicator
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //  Optiene el Token de acceso del usuario ya crreado
            var response2 = await apiService.GetToken(
                "http://chejconsultor.ddns.net:9015",
                Email,
                Password);

            //  Valida si hubo o no error en el metodo anterior
            if (response2 == null)
            {
                //  Detiene el ActivityIndicator
                SetEnabledDisable(false, true);
                Password = null;
                await dialogService.ShowMessage(
                    "Error", 
                    "The service is not available, please try latter...!!!");
                return;
            }

            //  Valida si el token viene vacion pero no nulo
            if (string.IsNullOrEmpty(response2.AccessToken))
            {
                //  Detiene el ActivityIndicator
                SetEnabledDisable(false, true);
                Password = null;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //  Asigna el Token a la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = response2;
            //  Genera un objeto de la clase CategoriesViewModel
            mainViewModel.Categories = new CategoriesViewModel();
            //  Hace una navegacion Back (PopAsync)
            await navigationService.BackOnLogin();
            //  Navega al CategoryView
            //  await navigationService.NavigateOnMaster("CategoriesView");
            navigationService.SetMainPage("MasterView");

            //  Detiene el ActivityIndicator
            SetEnabledDisable(false, true);
        }

        /// <summary>
        /// Metodo que hace la navegacion Back (PopAsync)
        /// </summary>
        private async void Back()
        {
            await navigationService.BackOnLogin();
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
