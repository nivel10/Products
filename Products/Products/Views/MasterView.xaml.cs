namespace Products.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    //  public partial class MasterView : ContentPage
    public partial class MasterView : MasterDetailPage
    {
        public MasterView()
        {
            InitializeComponent();
        }

        //  Este medodo de SobreEscritura (Para el Menu Hamburguesa)
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //  Hilo principal de la aplicacion (para la navegacion)
            App.Navigator = Navigator;

            //  Esta propiedad hace que se oculte el menu de manera automatica
            //  Cuando se selecciona una opcion del mismo
            App.Master = this;
        }
    }
}