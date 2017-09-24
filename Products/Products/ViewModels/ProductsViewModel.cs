namespace Products.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using Products.Models;

    public class ProductsViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private List<Product> products;
        private ObservableCollection<Product> _productsList;

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

        #endregion

        #region Constructor

        public ProductsViewModel(List<Product> products)
        {
            this.products = products;

            //  Genra el ObservableCollection de ProductsList y ordena los datos
            ProductsList = 
                new ObservableCollection<Product>(products.OrderBy(p => p.Description));
        }

        #endregion
    }
}
