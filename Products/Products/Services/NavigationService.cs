namespace Products.Services
{
    using Products.Views;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class NavigationService
    {
        ///// <summary>
        ///// Metodo asincrono que se emplea para la navegacion de los View (Page)
        ///// </summary>
        ///// <returns>Navegacion asincrona de pagina</returns>
        ///// <param name="pageName">String que almacena el nombre del view a navegar</param>
        //public async Task Navigate(string pageName)
        //{
        //    switch (pageName)
        //    {
        //        case "CategoriesView":
        //            await Application.Current.MainPage.Navigation.PushAsync(
        //                new CategoriesView());
        //            break;

        //        case "ProductsView":
        //            await Application.Current.MainPage.Navigation.PushAsync(
        //                new ProductsView());
        //            break;

        //        case "NewCategoryView":
        //            await Application.Current.MainPage.Navigation.PushAsync(
        //                new NewCategoryView());
        //            break;

        //        case "NewProductView":
        //            await Application.Current.MainPage.Navigation.PushAsync(
        //                new NewProductView());
        //            break;

        //        case "EditCategoryView":
        //            await Application.Current.MainPage.Navigation.PushAsync(
        //                new EditCategoryView());
        //            break;

        //        case "EditProductView":
        //            await Application.Current.MainPage.Navigation.PushAsync(
        //                new EditProductView());
        //            break;

        //        case "NewCustomerView":
        //            await Application.Current.MainPage.Navigation.PushAsync(
        //                new NewCustomerView());
        //            break;
        //    };
        //}

        /// <summary>
        /// Metodo asincrono que se emplea para la navegacion de los View (Page)
        /// </summary>
        /// <returns>Navegacion asincrona de pagina</returns>
        /// <param name="pageName">String que almacena el nombre del view a navegar</param>
        public async Task NavigateOnMaster(string pageName)
        {
            //  Aqui se oculta de manera automatica el Menu 
            //  En la Propiedad IsPresented
            App.Master.IsPresented = false;

            switch (pageName)
            {
                case "CategoriesView":
                    await App.Navigator.PushAsync(
                        new CategoriesView());
                    break;

                case "ProductsView":
                    await App.Navigator.PushAsync(
                        new ProductsView());
                    break;

                case "NewCategoryView":
                    await App.Navigator.PushAsync(
                        new NewCategoryView());
                    break;

                case "NewProductView":
                    await App.Navigator.PushAsync(
                        new NewProductView());
                    break;

                case "EditCategoryView":
                    await App.Navigator.PushAsync(
                        new EditCategoryView());
                    break;

                case "EditProductView":
                    await App.Navigator.PushAsync(
                        new EditProductView());
                    break;

                case "UbicationsView":
                    await App.Navigator.PushAsync(
                        new UbicationsView());
                    break;
            };
        }

        /// <summary>
        /// Metodo que navega dentro de la pagina del LoginView
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public async Task NavigateOnLogin(string pageName)
        {
            switch (pageName)
            {
                case "NewCustomerView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new NewCustomerView());
                    break;
            };
        }

        ///// <summary>
        ///// Metodo que hace desapilar la navegacion
        ///// </summary>
        ///// <returns>Retorna la navegacion</returns>
        //public async Task Back()
        //{
        //    await Application.Current.MainPage.Navigation.PopAsync();
        //}

        /// <summary>
        /// Metodo que hace desapilar la navegacion
        /// </summary>
        /// <returns>Retorna la navegacion</returns>
        public async Task BackOnMaster()
        {
            await App.Navigator.PopAsync();
        }

        /// <summary>
        /// Metodo que hace desapilar la navegacion
        /// </summary>
        /// <returns>Retorna la navegacion</returns>
        public async Task BackOnLogin()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Metodo que define cual sera la pagina principal
        /// </summary>
        /// <param name="pageName">String que almacena la pagina principal</param>
        public void SetMainPage(string pageName)
        {
            switch (pageName)
            {
                case "LoginView":
                    Application.Current.MainPage = new NavigationPage(
                        new LoginView());
                    break;

                case "MasterView":
                    Application.Current.MainPage = new MasterView();
                    break;
            }
        }
    }
}
