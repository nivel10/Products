namespace Products.Services
{
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Products.Models;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiService
    {
        /// <summary>
        /// Metodo que verifica la conexion y acceso a internet
        /// </summary>
        /// <returns>Objeto Response IsSuccess, Message, Result</returns>
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Plase turn on your internet settings...!!!",
                    Result = null,
                };
            }

            var isRemoteReachable =
                await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isRemoteReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Please check your internet connection...!!!",
                    Result = null,
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Connection to internet is ok...!!!",
                Result = null,
            };
        }

        /// <summary>
        /// Metodo que optiene el token del WebSite
        /// </summary>
        /// <returns>Objeto TokenResponse</returns>
        /// <param name="urlBase">String Url base</param>
        /// <param name="userName">String user name</param>
        /// <param name="userPassword">String user password</param>
        public async Task<TokenResponse> GetToken(
            string urlBase,                 
            string userName,                  
            string userPassword)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync(
                    "Token",
                    new StringContent(
                        string.Format(
                        "grant_type=password&username={0}&password={1}",
                        userName,
                        userPassword),
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var resul = JsonConvert.DeserializeObject<TokenResponse>(resultJSON);
                return resul;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string urlPrefix,
            string urlController,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient();
                //  Agrega la url base al objeto
                client.BaseAddress = new Uri(urlBase);
                //  Define el encabezado para la autenticacion por Token
                client.DefaultRequestHeaders.Authorization =
                          new AuthenticationHeaderValue(tokenType, accessToken);
                //  Concatena el prefijo y el controlador del WebAPI
                var urlAccess = string.Format("{0}{1}", urlPrefix, urlController);
                //  Consume el servicio del WebAPI
                var response = await client.GetAsync(urlAccess);
                //  Se lee la respuesta
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                        Result = null,
                    };
                }

                //  Deserealiza el objeto con Json y lo conviente en un List<T>
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok...!!!",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.Trim(),
                    Result = null,
                };
            }
        }

        public async Task<Response> Post<T>(
            string urlBase,
            string urlPrefix,
            string urlController,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                //  Deserealiza el objeto
                var request = JsonConvert.SerializeObject(model);
                //  Crea el StringContent
                var content = new StringContent(
                    request, 
                    Encoding.UTF8, 
                    "application/json");
                //  Crea el objeto HttpClient
                var client = new HttpClient();
                //  Define la autenticacion del Token
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                //  Le asigna al objeto client la UrlBase
                client.BaseAddress = new Uri(urlBase);
                //  Concatena en una variable la Url del Prfix y la Url del Controller
                var urlApi = string.Format("{0}{1}", urlPrefix, urlController);
                //  Consume el servicio del Post en el WebAPI
                var response = await client.PostAsync(urlApi, content);
                //  Lee la respuesta optenida
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    //  Optiene el el error del objeto serializado
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                //  Deserealiza la respuesta y la devuelve en el objeto Respponse
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record is ok...!!!",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.Trim(),
                };
            }
        }

        public async Task<Response> Put<T>(
            string urlBase,
            string urlPrefix,
            string urlController,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {
                //  Deserealiza el objeto
                var request = JsonConvert.SerializeObject(model);
                //  Crea el StringContent
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                //  Crea el objeto HttpClient
                var client = new HttpClient();
                //  Define la autenticacion del Token
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                //  Le asigna al objeto client la UrlBase
                client.BaseAddress = new Uri(urlBase);
                //  Concatena en una variable la Url del Prfix, la Url del Controller y el Id del objeto
                var urlApi = string.Format(
                    "{0}{1}/{2}",
                    urlPrefix,
                    urlController,
                    model.GetHashCode());
                //  Consume el servicio del Put en el WebAPI
                var response = await client.PutAsync(urlApi, content);
                //  Lee la respuesta optenida
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    //  Optiene el el error del objeto serializado
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                //  Deserealiza la respuesta y la devuelve en el objeto Respponse
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record was updated ok...!!!",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.Trim(),
                };
            }
        }

        public async Task<Response> Post<T>(
            string urlBase, 
            string urlPrefix, 
            string urlController, 
            T model)
        {
            try
            {
                //  Deserealiza el objeto
                var request = JsonConvert.SerializeObject(model);
                //  Crea el StringContent
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                //  Crea el objeto HttpClient
                var client = new HttpClient();
                //  Le asigna al objeto client la UrlBase
                client.BaseAddress = new Uri(urlBase);
                //  Concatena en una variable la Url del Prfix y la Url del Controller
                var urlApi = string.Format("{0}{1}", urlPrefix, urlController);
                //  Consume el servicio del Post en el WebAPI
                var response = await client.PostAsync(urlApi, content);
                //  Lee la respuesta optenida
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    //  Optiene el el error del objeto serializado
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                //  Deserealiza la respuesta y la devuelve en el objeto Respponse
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record insert is ok...!!!",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.Trim(),
                };
            }
        }

        public async Task<Response> Delete<T>(
            string urlBase,
            string urlPrefix,
            string urlController,
            string tokenType,
            string accessToken,
            T model)
        {
            try
            {

                //  Crea el objeto HttpClient
                var client = new HttpClient();
                //  Define la autenticacion del Token
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokenType, accessToken);
                //  Le asigna al objeto client la UrlBase
                client.BaseAddress = new Uri(urlBase);
                //  Concatena en una variable la Url del Prfix, la Url del Controller y el Id del objeto
                var urlApi = string.Format(
                    "{0}{1}/{2}",
                    urlPrefix,
                    urlController,
                    model.GetHashCode());
                //  Consume el servicio del Delete en el WebAPI
                var response = await client.DeleteAsync(urlApi);
                //  Lee la respuesta optenida
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    //  Optiene el el error del objeto serializado
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                //  Deserealiza la respuesta y la devuelve en el objeto Respponse
                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Record was deleted ok...!!!",
                    Result = newRecord,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.Trim(),
                };
            }
        }

        public async Task<TokenResponse> LoginFacebook(
        string urlBase,
        string servicePrefix,
        string controller,
        FacebookResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                //  Optiene el token del Facebook
                var tokenResponse = await GetToken(
                    urlBase, 
                    profile.Id, 
                    profile.Id);
                
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }
    }
}
