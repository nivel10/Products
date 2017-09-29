namespace Products.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using Products.Models;
    using Products.Services;

    public class CategoriesViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private ApiService apiService;
        private DialogService dialogService;
        private ObservableCollection<Category> _categoriesList;
		private static CategoriesViewModel _instance;
        private List<Category> categories;

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<Category> CategoriesList
        {
            get
            {
                return _categoriesList;
            }
			set
            { 
                if(value != _categoriesList) 
                {
                    _categoriesList = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(CategoriesList)));
                }
			}
		}

        #endregion

        #region Constructor

        public CategoriesViewModel()
        {
            //  Optiene una instancia del CategoryViewModel
            _instance = this;

            //  Genera la instancia de los servicios
            apiService = new ApiService();
            dialogService = new DialogService();

            //  Invoca al metodo que hace la carga de las categorias
            LoadCategories();
        }

        #endregion

        #region Methods

        private async void LoadCategories()
        {
            //  Verifica si hay conexion a internet
            var connection = await apiService.CheckConnection();
            if(!connection.IsSuccess)
            {
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Optiene una instancia de la Main ViewModel
            var mainViewModel = MainViewModel.GetInstance();

            //  Optine una lista de categorias List<Category>
            var response = await apiService.GetList<Category>(
                "http://productszuluapi.azurewebsites.net",
                "/api", 
                "/Categories", 
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken);

            //  Valida si hubo o no error en el metodo anterior
            if(!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //  Castea el objeto en response como un List<Category>
            categories = (List<Category>)response.Result;

            //  Carga y ordena los datos en el ObservableCollection
            CategoriesList = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description));
		}
       
        /// <summary>
        /// Metodo que retorna una instancia del CategoryViewModel
        /// </summary>
        /// <returns>Objeto de instancia</returns>
        public static CategoriesViewModel GetInstance()
        {
            if(_instance == null)
            {
                return new CategoriesViewModel();
            }
            return _instance;
        }

        public void AddCategory(Category category)
        {
            //  Agrega el objeto al List<> pero no en orden
            categories.Add(category);

			//  Carga y ordena los datos en el ObservableCollection
			CategoriesList = new ObservableCollection<Category>(
				categories.OrderBy(c => c.Description));
        }

		#endregion
    }
}
