namespace Products.ViewModels
{
    using System.ComponentModel;     using System.Windows.Input;     using GalaSoft.MvvmLight.Command;     using Models;
    using Products.Helpers;
    using Services; 
    public class MyProfileViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion 
        #region Services

        ApiService apiService;
        DataService dataService;
        DialogService dialogService;
        NavigationService navigationService;

        #endregion 
        #region Attributes

        bool _isRunning;
        bool _isEnabled;

        #endregion 
        #region Properties

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
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
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public string CurrentPassword
        {
            get;
            set;
        }


        public string NewPassword
        {
            get;
            set;
        }

        public string ConfirmPassword
        {
            get;
            set;
        }

        #endregion 
        #region Constructors

        public MyProfileViewModel()
        {
            //  Instancia una clase de los Services
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            //  Invoca el metodo que habilita - deshabilita los controles
            SetEnabledDisable(false, true);
        }

        #endregion 
        #region Commands

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        public ICommand BackCommand
        {
            get { return new RelayCommand(Back); }
        }

        #endregion

        #region Methods

        async void Save()
        {
            if (string.IsNullOrEmpty(CurrentPassword))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter the current password...!!!");
                return;
            }

            //  Optiene una instancia del MainViewModel
            var mainViewModel = MainViewModel.GetInstance();

            if(mainViewModel.Token.Password == null)
            {
                await dialogService.ShowMessage(
                    "Error", 
                    "You must login again to change the password...!!!");
                return;
            }
                
            if (!mainViewModel.Token.Password.ToString().Trim().Equals(CurrentPassword))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "The current password is not valid...!!!");
                return;
            }

            if (string.IsNullOrEmpty(NewPassword))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a new password...!!!");
                return;
            }

            if (NewPassword.Length < 6)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "The new password must have at least 6 characters length...!!!");
                return;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a new password confirm...!!!");
                return;
            }

            if (!NewPassword.Equals(ConfirmPassword))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "The new password and confirm, does not match...!!!");
                return;
            }

            //  Invoca el metodo que habilita - deshabilita los controles
            SetEnabledDisable(true, false);

            //  Consume el API Service (Valida conexion)
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //  Invoca el metodo que habilita - deshabilita los controles
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Crea un objeto del ChangePasswordRequest
            var changePasswordRequest = new ChangePasswordRequest
            {
                CurrentPassword = CurrentPassword,
                Email = mainViewModel.Token.UserName,
                NewPassword = NewPassword,
            };

            //  Consume el API Service (Consume el metodo ChagePassword API)
            var response = await apiService.ChangePassword(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/Customers/ChangePassword",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                changePasswordRequest);

            if (!response.IsSuccess)
            {
                //  Invoca el metodo que habilita - deshabilita los controles
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            //  Actualiza el Token con el Nuevo Password
            mainViewModel.Token.Password = NewPassword;
            dataService.Update(mainViewModel.Token);

            //  Invoca el metodo que habilita - deshabilita los controles
            SetEnabledDisable(false, true);

            await dialogService.ShowMessage(
                "Confirm",
                "The password was changed successfully");
            
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