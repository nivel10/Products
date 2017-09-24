namespace Products.Models
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.ViewModels;
    using Products.Views;
    using Xamarin.Forms;

    public class Category
    {
        #region Commands

        public ICommand SelectCategoryCommand
        {
            get
            {
                return new RelayCommand(SelectCategory);
            }
        }

        #endregion

        #region Properties

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public List<Product> Products { get; set; }

        #endregion

        #region Methods

        private void SelectCategory()
        {
            //  Genera una instancia del Products() en la MainViewModel
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Products = new ProductsViewModel(Products);

			//  Genera la vanegacion de la pagina 
            //  PushAsync() = Apilar Paginas
			//  PopAsync() = Desapila paginas
			Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
        }

		#endregion
    }
}
