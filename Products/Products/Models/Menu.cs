namespace Products.Models
{
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using System;
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

        #region Methods

        private void Navigate()
        {
            throw new NotImplementedException();
        } 

        #endregion Methods

    }
}
