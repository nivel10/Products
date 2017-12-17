namespace Products.Models
{
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.ViewModels;
    using System;
    using System.Windows.Input;
    using Products.Helpers;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;

    public class Product
    {
        #region Attributes

        private NavigationService navigationService;

        #endregion Attributes

        #region Commands

        public ICommand EditCommand
        {
            get { return new RelayCommand(Edit); }
        }

        #endregion Commands

        #region Properties

        [PrimaryKey, AutoIncrement]    //  Codigo SQLite
        public int ProductId { get; set; }

        [ForeignKey(typeof(Category))]  //  Codigo SQLite
        public int CategoryId { get; set; }

        [ManyToOne] //  Codigo SQLite
        public Category Category { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastPurchase { get; set; }

        public double Stock { get; set; }

        public string Remarks { get; set; }

        public byte[] ImageArray { get; set; }

        public bool PendingToSave { get; set; }

        public string ImageFullPath
        {
            get
            {
                //  Se agrego la validacion if para que cuando no haya imagen carge
                //  Una imagen que esta en la carpeta de recursos locales
                if (string.IsNullOrEmpty(Image))
                {
                    return string.Format(
                        "{0}{1}", 
                        MethodsHelper.GetUrlAPI(),
                        "/Content/Images/NoImage.png");
                }

                return string.Format(
                    "http://chejconsultor.ddns.net:9015{0}",
                    Image.Substring(1));
            }
        }

        #endregion Properties

        #region Constructor

        public Product()
        {
            //  Genera una instancia de cada clase (Service)
            navigationService = new NavigationService();
        }

        #endregion Constructor

        #region Methods

        private async void Edit()
        {
            //  Optiene una instancia de la MainViewModel   \\
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditProduct = new EditProductViewModel(this);

            //  Genera la navegacion de la pagina 
            //  PushAsync() = Apilar Paginas
            //  PopAsync() = Desapila paginas
            //  Application.Current.MainPage.Navigation.PushAsync(new ProductsView());
            await navigationService.NavigateOnMaster("EditProductView");
        }

        public override int GetHashCode()
        {
            return ProductId;
        }

        #endregion Methods
    }
}
