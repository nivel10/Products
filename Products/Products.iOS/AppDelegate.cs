namespace Products.iOS
{
    using Foundation;
    using UIKit;
   
    [Register("AppDelegate")]
    public partial class AppDelegate :
    global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(
            UIApplication app, 
            NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Xamarin.FormsMaps.Init();

            //  Esta linea se agrega para inicializar las librerias de Maps
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
