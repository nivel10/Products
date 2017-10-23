namespace Products.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;

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
            await navigationService.Navigate("NewCategoryView");
        }

        private async void GoNewProduct()
        {
            //  Se invoca una instancia del NewProduct
            NewProduct = new NewProductViewModel();

            //  Invoca el servicio de nqvegacion
            await navigationService.Navigate("NewProductView");
        }

        #endregion
    }
}
