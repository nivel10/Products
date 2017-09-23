namespace Products.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;

    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private string _email;
        private string _password;
        private bool _isToggled;
        private bool _isRunning;
        private bool _isEnabled;

        #region Services

        private DialogService dialogService;

        #endregion

        #endregion

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

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
                if(value != _email)
                {
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
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
                if(value != _password)
                {
                    _password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
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

        #region Constructor

        public LoginViewModel()
        {
            //  Inicializacion de datos
            IsRunning = false;
            IsEnabled = true;
            IsToggled = true;

            //  Instancia de los services
            dialogService = new DialogService();
        }

        #endregion

        #region Methods

        private async void Login()
        {
            if(string.IsNullOrEmpty(Email))
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
        }

		#endregion
    }
}
