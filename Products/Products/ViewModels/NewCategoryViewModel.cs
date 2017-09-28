namespace Products.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;

    public class NewCategoryViewModel : INotifyPropertyChanged
    {
        #region Attributes

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        private string _description;
        private bool _isRunning;
        private bool _isEnabled;
        private DialogService dialogService;

        #endregion

        #endregion

        #region Commands

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
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

        public NewCategoryViewModel()
        {
            //  Genera una instancia de la clase de los services
            dialogService = new DialogService();

			//  Activa / Desactiva el ActivityIndicator y el boton
			SetEnabledDisable(false, true);
        }

        #endregion

        #region Methods

        private async void Save()
        {
            
            //  Valida si los controles del formulario
            if(string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage("Error", "Entry a valid description category...!!!");
                return;
            }

            await dialogService.ShowMessage("Information", "Nikole H.");
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
