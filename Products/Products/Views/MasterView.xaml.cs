
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
            App.Navigator = Navigator;
        }
    }
}