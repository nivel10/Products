namespace Products.Models
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

        private void Navigate()
        {
            switch (PageName)
            {
                case "LoginView":
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    navigationService.SetMainPage("LoginView");
                    break;
            }
        } 

        #endregion Methods

    }
}
