namespace Products.Services
{
    using System;
    using System.Threading.Tasks;
    using Products.Views;
    using Xamarin.Forms;

    public class NavigationService
    {
        /// <summary>
        /// Metodo asincrono que se emplea para la navegacion de los View (Page)
        /// </summary>
        /// <returns>Navegacion asincrona de pagina</returns>
        /// <param name="pageName">String que almacena el nombre del view a navegar</param>
        public async Task Navigate(string pageName)
        {
            switch (pageName)
            {
                case "CategoriesView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new CategoriesView());
                    break;

                case "ProductsView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new ProductsView());
                    break;

                case "NewCategoryView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new NewCategoryView());
                    break;

                case "NewProductView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new NewProductView());
                    break;

                case "EditCategoryView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new EditCategoryView());
                    break;

                case "EditProductView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new EditProductView());
                    break;

            };
        }

        /// <summary>
        /// Metodo que hace desapilar la navegacion
        /// </summary>
        /// <returns>Retorna la navegacion</returns>
        public async Task Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
