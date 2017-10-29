using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Products.Droid
{
    [Activity(Label = "Products", 
        Icon = "@drawable/icon", 
        Theme = "@style/MainTheme",
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            //  Esta linea se agrega para inicializar las librerias de Maps
            //  Xamarin.FormsMaps.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

