namespace Products
{
    using Products.Views;
    using Xamarin.Forms;

    public partial class App : Application
    {
        // Propiedad necesaria para navegar en el menu hamburguesa
        public static NavigationPage Navigator {
            get;
            internal set;
        }

        //  Propiedad que oculta el menu de manera automatica
        public static MasterView Master
        {
            get;
            internal set;
        }

        public App()
        {
            InitializeComponent();

            //  MainPage = new Products.MainPage();
            //  MainPage = new LoginView();
            MainPage = new NavigationPage(new LoginView());

            //  El inicio por el MasterView se aplica para usar el Menu Hamburguesa
            //  MainPage = new MasterView();
        }

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
    }
}
