namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MainViewModel
    {
        #region Atributtes

        static MainViewModel _instance;
        private NavigationService navigationService;

        #endregion;

        #region Properties

        public LoginViewModel Login
        {
            get;
            set;
        }

        public CategoriesViewModel Categories
        {
            get;
            set;
        }

        public TokenResponse Token
        {
            get;
            set;
        }

        public ProductsViewModel Products
        {
            get;
            set;
        }

        public NewCategoryViewModel NewCategory
        {
            get;
            set;
        }

        public NewProductViewModel NewProduct
        {
            get;
            set;
        }

        public EditCategoryViewModel EditCategory
        {
            get;
            set;
        }

        public Category Category
        {
            get;
            set;
        }

        public EditProductViewModel EditProduct
        {
            get;
            set;
        }

        public NewCustomerViewModel NewCustomer
        {
            get;
            set;
        }

        public ObservableCollection<Menu> MyMenu
        {
            get;
            set;
        }

        public UbicationsViewModel Ubications
        {
            get;
            set;
        }

        public SyncViewModel Sync
        {
            get;
            set;
        }
        #endregion

        #region Commands

        public ICommand NewCategoryCommand
        {
            get { return new RelayCommand(GoNewCategory); }
        }

        public ICommand NewProductCommand
        {
            get { return new RelayCommand(GoNewProduct); }
        }

        #endregion

        #region Construtor

        public MainViewModel()
        {
            //  Instancia de los services
            navigationService = new NavigationService();

            //  Instancia de los objetos ViewModel
            Login = new LoginViewModel();

            //  Instancia del MainViewModel (Se aplica para el Sigleton)
            _instance = this;

            //  Crea el menu del App
            LoadMenu();

        }

        #endregion

        #region Methods

        /// <summary>
        /// Metodo (Sigleton) que invoca una instancia de la MainViewModel
        /// </summary>
        /// <returns>Objeto MainViewModel</returns>
        public static MainViewModel GetInstance()
        {
            if (_instance == null)
            {
                return new MainViewModel();
            }
            else
            {
                return _instance;
            }
        }

        private async void GoNewCategory()
        {
            //  Genera una instancia de la NewCategory para bindar con la View
            NewCategory = new NewCategoryViewModel();

            //  Invoca el servicio de navegacion
            await navigationService.NavigateOnMaster("NewCategoryView");
        }

        private async void GoNewProduct()
        {
            //  Se invoca una instancia del NewProduct
            NewProduct = new NewProductViewModel();

            //  Invoca el servicio de nqvegacion
            await navigationService.NavigateOnMaster("NewProductView");
        }

        /// <summary>
        ///     Metodo que dibuja el menu
        /// </summary>
        private void LoadMenu()
        {   
            //  Instancia el objeto del Observable Collection<Menu>
            MyMenu = new ObservableCollection<Menu>();

            //  Asigna los Menu al Objeto
            MyMenu.Add(new Menu
            {
                Icon = "ic_settings.png",
                PageName = "MyProfileView",
                Title = "My Profile",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_map.png",
                PageName = "UbicationsView",
                Title = "Ubications",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_sync.png",
                PageName = "SyncView",
                Title = "Sync Off Line Operations",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_exit_to_app.png",
                PageName = "LoginView",
                Title = "Close sesion",
            });
        }

        #endregion
    }
}
