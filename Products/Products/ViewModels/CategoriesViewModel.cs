namespace Products.ViewModels
{
	using System;
	using System.Collections.ObjectModel;
	using Products.Models;

    public class CategoriesViewModel
    {
        #region Properties

        public ObservableCollection<Category> Categories
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public CategoriesViewModel()
        {
            LoadCategories();
        }

        #endregion

        #region Methods

        private void LoadCategories()
        {
            //  throw new NotImplementedException();
        }

		#endregion
    }
}
