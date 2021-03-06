﻿namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using System.ComponentModel;
    using System.Windows.Input;
    using Products.Helpers;
    using System;

    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private string _email;
        private string _password;
        private bool _isToggled;
        private bool _isRunning;
        private bool _isEnabled;

        #region Services

        private DataService dataService;
        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        #endregion

        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        public ICommand RegistreNewUserCommand
        {
            get { return new RelayCommand(RegistreNewUser); }
        }

        public ICommand LoginWithFacebookCommand
        {
            get
            {
                return new RelayCommand(LoginWithFacebook);
            }         } 
        public ICommand RecoverPasswordCommand
        {
            get { return new RelayCommand(RecoverPassword); }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

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

        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if (value != _isToggled)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsToggled)));
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

        #endregion

        #region Constructor

        public LoginViewModel()
        {
            //  Eliminar
            //  Email = "carlos.e.herrera.j@gmail.com";
            //  Password = "123456";

            //  Inicializacion de datos
            IsRunning = false;
            IsEnabled = true;
            IsToggled = true;

            //  Instancia de los services
            dataService = new DataService();
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();
        }

        #endregion

        #region Methods

        private async void Login()
        {
            //  Valida los campos del formulario
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Your must enter an email...!!!");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Your must enter a password...!!!");
                return;
            }

            //  Activa el ActivityIndicator
            SetEnabledDisable(true, false);

            //  Valida si el dispositivo tiene conexion 
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Valida si se puede optenet el Token
            var tokenResponse = await apiService.GetToken(
                MethodsHelper.GetUrlAPI(),
                Email,
                Password);
            if (tokenResponse == null)
            {
                SetEnabledDisable(false, true);
                Password = null;
                await dialogService.ShowMessage(
                    "Error",
                    "The service not available, plase try latter...!!!");
                return;
            }

            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                SetEnabledDisable(false, true);
                Password = null;
                await dialogService.ShowMessage(
                    "Error",
                    tokenResponse.ErrorDescription);
                return;
            }

            //  Garda los datos del response en las tablas de SQLite
            tokenResponse.IsRemembered = IsToggled;
            tokenResponse.Password = Password;
            //  Elimina los registros y solo guarda el ultimo Token
            dataService.DeleteAllAndInsert(tokenResponse);

            //  Valida si hubo o no error en los metodos anterior
            SetEnabledDisable(false, true);
            Email = null;
            Password = null;

            //  Invoca el metodo aue hace la instancia de la MainViewModel
            //  Crea una instancia del Categories y la vincula con la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();

            //  Asigna a la propiedad Token del MainViewModel el objeto tokenResponse
            mainViewModel.Token = tokenResponse;

            //  Crea una instancia de la clase Categories y la vincula con la MainViewModel
            mainViewModel.Categories = new CategoriesViewModel();

            //  Invoca el formulario Categories
            //  PushAsync = Apilar
            //  PopAsync = Desapilar
            //  await Application.Current.MainPage.Navigation.PushAsync(new CategoriesView());
            //  await navigationService.Navigate("CategoriesView");
            navigationService.SetMainPage("MasterView");
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
        /// Metodo que invoca el formulario NewCustomerView
        /// </summary>
        private async void RegistreNewUser()
        {
            //  Genera una instancia de la clase NewCustomerViewModel
            //  A traves del Sigleton
            MainViewModel.GetInstance().NewCustomer = new NewCustomerViewModel();

            //  Navega a la NewCustomerView
            await navigationService.NavigateOnLogin("NewCustomerView");
        }

        //  Invoca el metodo de navegacion
        async void LoginWithFacebook()
        {
            await navigationService.NavigateOnLogin("LoginFacebookView");
        }

        private async void RecoverPassword()
        {
            //  Crea una instancia del PasswordRecovery
            MainViewModel.GetInstance().PasswordRecovery = 
                new PasswordRecoveryViewModel();

            //  Navega a la pagina PasswordRecovery
            await navigationService.NavigateOnLogin("PasswordRecoveryView");
        }

        #endregion
    }
}
