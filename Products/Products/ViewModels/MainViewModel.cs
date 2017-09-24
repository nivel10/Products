namespace Products.ViewModels
{
    using Products.Models;

    public class MainViewModel
    {
        #region Atributtes

        static MainViewModel _instance;

        #endregion;

        #region Properties

        public LoginViewModel Login
        {
            get;
            set;
        }

        public CategoriesViewModel Categories
        {
            get;
            set;
        }

        public TokenResponse Token
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

            //  Instancia del MainViewModel (Se aplica para el Sigleton)
            _instance = this;

        }

		#endregion

		#region Methods

		/// <summary>
		/// Metodo (Sigleton) que invoca una instancia de la MainViewModel
		/// </summary>
		/// <returns>Objeto MainViewModel</returns>
		public static MainViewModel GetInstance()
        {
            if (_instance == null)
            {
                return new MainViewModel();
            }
            else
            {
                return _instance;
            }
        }

        #endregion
    }
}
