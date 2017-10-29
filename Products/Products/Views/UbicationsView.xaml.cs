namespace Products.Views
{
    using Products.Services;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    //  using Xamarin.Forms.Maps;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UbicationsView : ContentPage
    {

        #region Attributes

        private GeolocatorService geolocatorService;

        #endregion Attributes

        #region Constructor

        public UbicationsView()
        {
            InitializeComponent();

            //  Genera una instancia del Objeto (Services)
            geolocatorService = new GeolocatorService();

            //  Invoca el metodo que le asigna al mapa al ubicacion actual
            MoveMapToCurrentPosition();
        }

        private async void MoveMapToCurrentPosition()
        {
            //  Invoca el metodo en el servicio GeoLocatorService
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 ||
                geolocatorService.Longitude != 0)
            {
                //  Asigna la posicion a la variable position
                //var position = new Position(
                //geolocatorService.Latitude,
                //geolocatorService.Longitude);

                //  Mueve el mapa a la ubicacion asignada
                //MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                //    position,
                //    Distance.FromKilometers(.5)));

            }
        }

        #endregion Constructor
    }
}