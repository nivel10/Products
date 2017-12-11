using System;
using Xamarin.Forms;

namespace Products.Helpers
{
    public class MethodsHelper
    {
        public static string GetUrlAPI()
        {
            return Application.Current.Resources["UrlAPI"].ToString().Trim();
        }
    }
}
