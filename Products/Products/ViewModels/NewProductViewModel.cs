namespace Products.ViewModels
{
    using System.ComponentModel;
    using System;

    public class NewProductViewModel : INotifyPropertyChanged
    {

        #region Attributes

        public event PropertyChangedEventHandler PropertyChanged;
        private string _description;
        private string _price;
        private bool _isToggled;
        private DateTime _lastPurchase;

        #endregion

        #region Properties

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if(value != _description)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                if(value != _price)
                {
                    _price = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                }
            }
        }

        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if(value != _isToggled)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsToggled)));
                }
            }
        }
        #endregion

        #region Costructor

        public NewProductViewModel()
        {
        }

        #endregion
    }
}
