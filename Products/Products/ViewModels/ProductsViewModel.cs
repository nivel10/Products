namespace Products.ViewModels
{
    using Products.Models;
    using Products.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    public class ProductsViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private List<Product> products;
        private ObservableCollection<Product> _productsList;
        private NavigationService navigationService;
        private static ProductsViewModel _instance;
        private bool _isRefreshing;

        #region Eventns

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<Product> ProductsList
        {
            get
            {
                return _productsList;
            }
            set
            {
                if (value != _productsList)
                {
                    _productsList = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(ProductsList)));
                }
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (value != _isRefreshing)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
        }

        #endregion

        #region Constructor

        public ProductsViewModel(List<Product> products)
        {
            this.products = products;

            //  Carga una instancia de la clace 
            _instance = this;

            //  Instancia un objeto de cada clase (Service)
            navigationService = new NavigationService();

            //  Genra el ObservableCollection de ProductsList y ordena los datos
            ProductsList =
                new ObservableCollection<Product>(
                    products.OrderBy(p => p.Description));
        }

        #endregion Constructor

        #region Methods

        #region Sigleton

        /// <summary>
        /// Metodo que optiene una instancia de la clase ProductsViewModel
        /// </summary>
        /// <param name="products">Objeto List-Product-</param>
        /// <returns></returns>
        public static ProductsViewModel GetInstance(List<Product> products)
        {
            if (_instance == null)
            {
                _instance = new ProductsViewModel(products);
            }

            return _instance;
        }
        //public static ProductsViewModel GetInstance()
        //{
        //    return _instance;
        //}

        #endregion  Sigleton

        /// <summary>
        /// Metodo que actualiza el Objeto Product
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(Product product)
        {
            //  ActivityIndicator del View
            IsRefreshing = true;

            var oldProduct = products
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();
            oldProduct = product;

            ProductsList = new ObservableCollection<Product>(
                products.OrderBy(p => p.Description));

            //  ActivityIndicator del View
            IsRefreshing = false;
        }

        /// <summary>
        /// Metodo que agrega un producto al List-Product-
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            //  ActivityIndicator del View
            IsRefreshing = true;

            //  Agrega un product al List<Product>
            products.Add(product);

            //  Agrega todos los List<Product> al ObservableCollection<Product> y ordena
            ProductsList = new ObservableCollection<Product>(
                products.OrderBy(p => p.Description));

            //  ActivityIndicator del View
            IsRefreshing = false;
        }

        #endregion Methods
    }
}
