namespace Products.Views
{
    using System.Threading.Tasks;
    using Products.Services;
    using Products.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Xamarin.Forms.Maps;

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

        #endregion Constructor

        #region Methods

        private async void MoveMapToCurrentPosition()
        {
            //  Invoca el metodo en el servicio GeoLocatorService
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 ||
                geolocatorService.Longitude != 0)
            {
                //  Asigna la posicion a la variable position
                var position = new Position(
                    geolocatorService.Latitude,
                    geolocatorService.Longitude);

                //  Mueve el mapa a la ubicacion asignada
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(1)));

                //  Metodo que carga los Pin's
                // await LoadPins();
            }
        }

        /// <summary>
        /// Metodo que hace la carga de pines
        /// </summary>
        /// <returns></returns>
        private async Task LoadPins()
        {
            //  Optiene una instancia del UbicationsViewModel
            var ubicationsViewModel = UbicationsViewModel.GetInstance();
            //  Invoca el metodo que hace la carga de los Pin's
            await ubicationsViewModel.LoadPins();
            //  Se pintan los pines en el mapa
            foreach (var pin in ubicationsViewModel.Pins)
            {
                MyMap.Pins.Add(pin);
            }
        }

        #endregion Methods
    }
}