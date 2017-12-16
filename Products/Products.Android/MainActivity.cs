namespace Products.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;

    //  Esta linea se comenta para colocar el MainLauncher en false ya que la
    //  Aplicacion inicia en el Activity Splash
    //[Activity(
        //Label = "Products", 
        //Icon = "@drawable/ic_launcher", 
        //Theme = "@style/MainTheme",
        //MainLauncher = true, 
        //ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    [Activity(
        Label = "Products",
        Icon = "@drawable/ic_launcher",
        Theme = "@style/MainTheme",
        MainLauncher = false,
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
            Xamarin.FormsMaps.Init(this, bundle);

            LoadApplication(new App());
        }
    }
}

