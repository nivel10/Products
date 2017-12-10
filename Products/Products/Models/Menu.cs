﻿namespace Products.Models
{
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.ViewModels;
    using System.Windows.Input;

    public class Menu
    {
        #region Attributes

        public NavigationService navigationService;

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
                    //  Navega al LoginView
                    navigationService.SetMainPage("LoginView");
                    break;

                case "UbicationsView":
                    MainViewModel.GetInstance().Ubications = 
                        new UbicationsViewModel();
                    await navigationService.NavigateOnMaster("UbicationsView");
                    break;
            }
        } 

        #endregion Methods

    }
}
