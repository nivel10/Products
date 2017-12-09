namespace Products.ViewModels
{
    using Models;
    using Services;
    using System.Collections.Generic;
    //  using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    //  using Xamarin.Forms.Maps;

    public class UbicationsViewModel
    {
        #region Services

        private ApiService apiService;
        private DialogService dialogService;
        static UbicationsViewModel instance;

        #endregion

        #region Properties

        //public ObservableCollection<Pin> Pins
        //{
        //    get;
        //    set;
        //}

        #endregion

        #region Constructors

        public UbicationsViewModel()
        {
            //  Carga la instancia para el Sigleton
            instance = this;

            //  Instancia las las clases de Servicio
            apiService = new ApiService();
            dialogService = new DialogService();
        }

        #endregion

        #region Sigleton

        public static UbicationsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new UbicationsViewModel();
            }

            return instance;
        }

        #endregion

        #region Methods

        public async Task LoadPins()
        {
            //  Verifica la conexion a internet
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    connection.Message);
                return;
            }

            //  Genera una inatsancia de la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();

            //  Consume el ApiService
            var response = await apiService.GetList<Ubication>(
                "http://chejconsultor.ddns.net:9015",
                "/api",
                "/Ubications",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken);

            //  Valida si hubo error en el metodo anterior
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            //  Caega los datos de los Pins en el objeto
            var ubications = (List<Ubication>)response.Result;
            //Pins = new ObservableCollection<Pin>();
            //foreach (var ubication in ubications)
            //{
            //    Pins.Add(new Pin
            //    {
            //        Address = ubication.Address,
            //        Label = ubication.Description,
            //        Position = new Position(
            //            ubication.Latitude,
            //            ubication.Longitude),
            //        Type = PinType.Place,
            //    });
            //}
        }

        #endregion Methods
    }
}
