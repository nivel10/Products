namespace Products.Models
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.ViewModels;

    public class Category
    {
        #region Attributes

        private NavigationService navigationService;

        #endregion

        #region Commands

        public ICommand SelectCategoryCommand
        {
            get
            {
                return new RelayCommand(SelectCategory);
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(Edit);
            }
        }

        #endregion

        #region Constructor

        public Category()
        {
            navigationService = new NavigationService();
        }

        #endregion

        #region Properties

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public List<Product> Products { get; set; }

        #endregion

        #region Methods

        private async void SelectCategory()
        {
            //  Genera una instancia del Products() en la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Products = new ProductsViewModel(Products);

			//  Genera la navegacion de la pagina 
            //  PushAsync() = Apilar Paginas
			//  PopAsync() = Desapila paginas
			//  Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
            await navigationService.Navigate("ProductsView");
        }

        private async void Edit()
        {
            //  Genera una instancia del Products() en la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditCategory = new EditCategoryViewModel(this);

            //  Genera la navegacion de la pagina 
            //  PushAsync() = Apilar Paginas
            //  PopAsync() = Desapila paginas
            //  Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
            await navigationService.Navigate("EditCategoryView");
        }

        public override int GetHashCode()
        {
            return CategoryId;
        }
        #endregion
    }
}
