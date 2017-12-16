namespace Products.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Products.Helpers;
    using System;

    public class CategoriesViewModel : INotifyPropertyChanged
    {
        #region Attributes

        private ApiService apiService;
        private DataService dataService;
        private DialogService dialogService;
        private ObservableCollection<Category> _categoriesList;
        private static CategoriesViewModel _instance;
        private List<Category> categories;
        private bool _isRefreshing;
        private string _filter;

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #endregion

        #region Commands

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadCategories);
            }
        }

        public ICommand SearchCommand
        {
            get { return new RelayCommand(Search); }
        }

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
                if (value != _categoriesList)
                {
                    _categoriesList = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CategoriesList)));
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

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if(value != _filter)
                {
                    _filter = value;
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Filter)));
                }
                else
                {
                    PropertyChanged?.Invoke(
                        this, 
                        new PropertyChangedEventArgs(nameof(Filter)));
                }
                //  Invoca el metodo de busqueda
                Search();
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
            dataService = new DataService();
            dialogService = new DialogService();

            //  Invoca al metodo que hace la carga de las categorias
            LoadCategories();
        }

        #endregion

        #region Methods

        private async void LoadCategories()
        {
            //  ActivityIndicator del View
            IsRefreshing = true;

            //  Verifica si hay conexion a internet
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //  Optiene la List<Category> de la base de datos SQLite
                categories = dataService.Get<Category>(true);
                //  Valida si hay datos que procesar
                if(categories.Count == 0)
                {
                    //  ActivityIndicator del View
                    IsRefreshing = false;
                    await dialogService.ShowMessage("Error", connection.Message);
                    return;
                }
            }
            else
            {
                //  Optiene una instancia de la Main ViewModel
                var mainViewModel = MainViewModel.GetInstance();

                //  Optine una lista de categorias List<Category>
                var response = await apiService.GetList<Category>(
                    MethodsHelper.GetUrlAPI(),
                    "/api",
                    "/Categories",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken);

                //  Valida si hubo o no error en el metodo anterior
                if (!response.IsSuccess)
                {
                    IsRefreshing = false;
                    await dialogService.ShowMessage("Error", response.Message);
                    return;
                }

                //  Castea el objeto en response como un List<Category>
                categories = (List<Category>)response.Result;

                //  Guarda los datos en el SQLite
                SaveCategoriesOnDB();

                //  Carga y ordena los datos en el ObservableCollection
                CategoriesList = new ObservableCollection<Category>(
                    categories.OrderBy(c => c.Description));
    
            }

            //  ActivityIndicator del View
            IsRefreshing = false;
        }

        /// <summary>
        /// Metodo que retorna una instancia del CategoryViewModel
        /// </summary>
        /// <returns>Objeto de instancia</returns>
        public static CategoriesViewModel GetInstance()
        {
            if (_instance == null)
            {
                return new CategoriesViewModel();
            }
            return _instance;
        }

        public void AddCategory(Category category)
        {
            //  ActivityIndicator del View
            IsRefreshing = true;

            //  Agrega el objeto al List<> pero no en orden
            categories.Add(category);

            //  Carga y ordena los datos en el ObservableCollection
            CategoriesList = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description));

            //  ActivityIndicator del View
            IsRefreshing = false;
        }

        public void UpdateCategory(Category category)
        {
            //  ActivityIndicator del View
            IsRefreshing = true;

            //  Busca dentro del objeto List<Category> el registro a modificar
            var oldCategory = categories
                .Where(c => c.CategoryId == category.CategoryId)
                .FirstOrDefault();
            oldCategory = category;

            //  Carga y ordena los datos en el ObservableCollection
            CategoriesList = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description));

            //  ActivityIndicator del View
            IsRefreshing = false;
        }

        public async Task DeleteCategory(Category category)
        {
            //  ActivityIndicator del View
            IsRefreshing = true;

            //  Valida que haya conexion a internet
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", connection.Message);
                return;
            }

            //  Invoca una instancia del Sigleton
            var mainViewModel = MainViewModel.GetInstance();

            //  Invoca el metodo aue hqce el insert de datos (Put)
            //  Put = Verbo que hace referencia modificar
            var response = await apiService.Delete(
                MethodsHelper.GetUrlAPI(),
                "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken,
                category);

            //  Valida si hubo o no error en el metodo anterior
            if (!response.IsSuccess)
            {
                //  ActivityIndicator del View
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            //  Elimiina de la lista el objeto categoria
            categories.Remove(category);

            //  Carga y ordena los datos en el ObservableCollection
            CategoriesList = new ObservableCollection<Category>(
                categories.OrderBy(c => c.Description));

            //  ActivityIndicator del View
            IsRefreshing = false;

            await dialogService.ShowMessage(
                "Information", 
                "Category is deleted...!!!");
        }

        private void Search()
        {
            //  ActivityIndicator del View
            IsRefreshing = true;

            //  Hace la busqueda en el Categories
            CategoriesList = new ObservableCollection<Category>(
                categories
                .Where(c => c.Description.ToLower().Contains(Filter.ToLower()))
                .OrderBy(c => c.Description)
            );

            //  ActivityIndicator del View
            IsRefreshing = false;
        }

        private void SaveCategoriesOnDB()
        {
            //  Elimina los datos de Categorias y sus tablas relacionadas
            dataService.DeleteAll<Category>();

            foreach (var category in categories)
            {
                //  Guarda los datos de las caegorias
                dataService.Insert<Category>(category);
                //  Guarda los datos de los productos asociados a las categorias
                dataService.Save(category.Products);
            }
        }

        #endregion Methods
    }
}
