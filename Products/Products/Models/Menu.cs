namespace Products.Models
{
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.ViewModels;
    using System.Windows.Input;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Products.Helpers;

    public class Menu
    {
        #region Attributes

        private ApiService apiService;
        private DataService dataService;
        private DialogService dialogService;
        private NavigationService navigationService;

        #endregion Attributes

        #region Commands

        public ICommand NavigateCommand
        {
            get { return new RelayCommand(Navigate); }
        }

        #endregion Commands

        #region Properties

        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }

        #endregion Properties

        #region Constructor

        public Menu()
        {
            //  Instancia un objeto de la clase (Service)
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }

        #endregion Constructor

        #region Methods

        private async void Navigate()
        {
            switch (PageName)
            {
                case "LoginView":
                    //  Inicializa el objeto Login (Email / Password)
                    MainViewModel.GetInstance().Login =
                        new LoginViewModel();
                    MainViewModel.GetInstance().Login.Email = null;
                    MainViewModel.GetInstance().Login.Password = null;
                    MainViewModel.GetInstance().Token.IsRemembered = false;
                    //  Actualiza el token del SQLite
                    dataService.Update(MainViewModel.GetInstance().Token);
                    //  Navega al LoginView
                    //  navigationService.SetMainPage("LoginView");
                    navigationService.SetMainPage(PageName);
                    break;

                case "UbicationsView":
                    //  Genera una nueva instancia de la UbicationsViewModel
                    MainViewModel.GetInstance().Ubications = 
                        new UbicationsViewModel();
                    //  await navigationService.NavigateOnMaster("UbicationsView");
                    await navigationService.NavigateOnMaster(PageName);
                    break;

                case "SyncView":
                    //  Crea una instancia del Sync y Navega a la pagina
                    MainViewModel.GetInstance().Sync = new SyncViewModel();
                    //  await navigationService.NavigateOnMaster("SyncView");
                    await navigationService.NavigateOnMaster(PageName);
                    //  SyncData();
                    break;

                case "MyProfileView":
                    //  Crea una istancia del MyProfileVew
                    MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                    //  Navega a la pagina del MyProfile
                    await navigationService.NavigateOnMaster(PageName);
                    break;
            }
        }

        //private async void SyncData()
        //{
        //    //  Valida que haya conexion del dispositivo
        //    var connection = await apiService.CheckConnection();
        //    if(!connection.IsSuccess)
        //    {
        //        await dialogService.ShowMessage("Error", connection.Message);
        //        return;
        //    }

        //    //  Optiene la List<Product>
        //    var products = dataService.Get<Product>(false)
        //                              .Where(p => p.PendingToSave = true)
        //                              .ToList();
        //    if(products.Count == 0)
        //    {
        //        await dialogService.ShowMessage(
        //            "Information", 
        //            "There are not products to sync...!!! ");
        //        return;
        //    }
        //    else
        //    {
        //        SaveProducts(products);
        //    }
        //}

        //private async void SaveProducts(List<Product> products)
        //{
        //    //  Recorre el List<Product>
        //    foreach (var product in products)
        //    {

        //        //  Invoca el metodo que hace el insert de datos (Api)
        //        var response = await apiService.Post(
        //            MethodsHelper.GetUrlAPI(),
        //            "/api",
        //            "/Products",
        //            MainViewModel.GetInstance().Token.TokenType,
        //            MainViewModel.GetInstance().Token.AccessToken,
        //            products);

        //        //  Valida si hubo o no error en el WepAPI
        //        if (response.IsSuccess)
        //        {
        //            //  Actualiza el registro cargado en el API
        //            product.PendingToSave = false;
        //            dataService.Update(product);
        //        }
        //    }

        //    await dialogService.ShowMessage(
        //        "Confirmation", 
        //        "Sync Status Ok...!!!");
        //}

        #endregion Methods

    }
}
