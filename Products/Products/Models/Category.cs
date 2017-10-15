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
        private DialogService dialogService;

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

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(Delete); }
        }

        #endregion

        #region Constructor

        public Category()
        {
            //  Genera la instancia de los services
            navigationService = new NavigationService();
            dialogService = new DialogService();
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

            //  Asigna a la propiedad Category el objeto de la categoria
            mainViewModel.Category = this;

            //  Genera la navegacion de la pagina 
            //  PushAsync() = Apilar Paginas
            //  PopAsync() = Desapila paginas
            //  Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
            await navigationService.Navigate("ProductsView");
        }

        /// <summary>
        /// Metodo que edita los datos de las Category
        /// </summary>
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

        /// <summary>
        /// Metodo que elimina un registro de los Category
        /// </summary>
        private async void Delete()
        {
            var response = await dialogService.ShowConfirm(
                "Confirm",
                "Are you sure to deleted record...?");
            if (response)
            {
                //  Se borra en el CategoryViewModel porque esta clase no tiene
                //  Implementado el INptifyPropertyChanged
                //  Se optiene una instancia del CategoryViewModel
                var categoryViewModel = CategoriesViewModel.GetInstance();
                await categoryViewModel.DeleteCategory(this);
            }
            else
            {
                return;
            }
        }

        public override int GetHashCode()
        {
            return CategoryId;
        }

        #endregion
    }
}