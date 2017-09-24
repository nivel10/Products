namespace Products.Models
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

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
            throw new NotImplementedException();
        }

		#endregion
    }
}
