namespace Products.ViewModels
{
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;

    public class NewCategoryViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private string _description;
        private bool _isRunning;
        private bool _isEnabled;
        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #endregion

        #region Commands

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand BackCommand
        {
            get { return new RelayCommand(Back); }
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

        public NewCategoryViewModel()
        {
            //  Genera una instancia de la clase de los services
            dialogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            //  Activa / Desactiva el ActivityIndicator y el boton
            SetEnabledDisable(false, true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Metodo que genera una nueva Category
        /// </summary>
        private async void Save()
        {
            //  Valida si los controles del formulario
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                    "Error",
                    "You must enter a category description...!!!");
                return;
            }

            //  Habilita el ActivityIndicator
            SetEnabledDisable(true, false);

            //  Valida que haya conexion a internet
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Se crea un objeto Category
            //  Se carga de datos el objeto Category
            var category = new Category
            {
                Description = Description,
            };

            //  Invoca una instancia del Sigleton
            var mainViewModel = MainViewModel.GetInstance();

            //  Invoca el metodo aue hqce el insert de datos (Post)
            //  Post = Verbo que hacer referencia agregar, adicionar
            var response = await apiService.Post(
                "http://chejconsultor.ddns.net:9015",
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                category);

            //  Valida si hubo o no error en el metodo anterior
            if (!response.IsSuccess)
            {
                //  Habilita el ActivityIndicator
                SetEnabledDisable(false, true);
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //  Se crea una categoria a partir del objeto response para 
            //  Actuzizar el objeto categoria (CategoryVieModel)
            category = (Category)response.Result;

            //  Invoca una instancia del Category
            var categoryVieModel = CategoriesViewModel.GetInstance();
            //  Invoca el metodo que actualiza el objeto de categoria
            categoryVieModel.AddCategory(category);

            //  Habilita el ActivityIndicator
            SetEnabledDisable(false, true);

            await dialogService.ShowMessage(
                "Information",
                string.Format(
                    "Category {0} created...!!!",
                    category.Description));

            //  Retorna a la pagina anterior
            //  await navigationService.Back();
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

        private async void Back()
        {
            //  await navigationService.Back();
            await navigationService.BackOnMaster();
        }

        #endregion
    }
}
