namespace Products
{
    using System;
    using Products.Helpers;
    using Products.Models;
    using Products.Services;
    using Products.ViewModels;
    using Products.Views;
    using Xamarin.Forms;

    public partial class App : Application
    {
        #region Attributes

        private static ApiService apiService;
        private static DialogService dialogService;

        #endregion Attributes

        #region Properties

        // Propiedad necesaria para navegar en el menu hamburguesa
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }

        //  Propiedad que oculta el menu de manera automatica
        public static MasterView Master
        {
            get;
            internal set;
        }

        #endregion Properties

        #region Constructor

        public App()
        {
            InitializeComponent();

            //  MainPage = new Products.MainPage();
            //  MainPage = new LoginView();
            MainPage = new NavigationPage(new LoginView());

            //  El inicio por el MasterView se aplica para usar el Menu Hamburguesa
            //  MainPage = new MasterView();

            //  Instancia la clase de los services
            apiService = new ApiService();
            dialogService = new DialogService();
        }

        #endregion Constructor

        #region Methods

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        //  Metodo cuando falla el LoginFacebook
        public static Action LoginFacebookFail
        {
            get
            {
                return new Action(() => Current.MainPage =
                  new NavigationPage(new LoginView()));
            }
        }

        //  Metodo cuando el LoginFacebook es Success
        //public static void LoginFacebookSuccess(FacebookResponse profile)
        //{
        //    Current.MainPage = new MasterView();
        //}

        public async static void LoginFacebookSuccess(FacebookResponse profile)
        {
            if (profile == null)
            {
                Current.MainPage = new NavigationPage(new LoginView());
                return;
            }

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            //  Verifica si ya se registro
            //  var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var token = await apiService.LoginFacebook(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/Customers/LoginFacebook",
                profile);

            if (token == null)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Problem ocurred retrieving user information, try latter.");
                Current.MainPage = new NavigationPage(new LoginView());
                return;
            }

            //  Optiene una instancia de la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();
            //  Asigna al Token de la MainViewModel
            mainViewModel.Token = token;
            //  Genera una instancia de la CategoriesViewModel
            mainViewModel.Categories = new CategoriesViewModel();
            Current.MainPage = new MasterView();
        }

        #endregion Methods
    }
}
