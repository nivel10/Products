namespace Products.ViewModels
{
    public class MainViewModel
    {

        #region Properties

        public LoginViewModel Login
        {
            get;
            set;
        }

        #endregion

        #region Construtor

        public MainViewModel()
        {
            //  Instancia de los objetos ViewModel
            Login = new LoginViewModel();
        }

        #endregion
    }
}
