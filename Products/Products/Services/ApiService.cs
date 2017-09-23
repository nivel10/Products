﻿namespace Products.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Products.Models;

    public class ApiService
    {
        /// <summary>
        /// Metodo que verifica la conexion y acceso a internet
        /// </summary>
        /// <returns>Objeto Response IsSuccess, Message, Result</returns>
        public async Task<Response> CheckConnection()
        {
            if(!CrossConnectivity.Current.IsConnected)
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
            if(!isRemoteReachable)
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
        public async Task<TokenResponse> GetToken(string urlBase, 
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
            string urlPefix, 
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
                var urlAccess = string.Format("{0}{1}", urlPefix, urlController);
                //  Consume el servicio del WebAPI
                var response = await client.GetAsync(urlAccess);
                //  Se lee la respuesta
                var result = await response.Content.ReadAsStringAsync();

                if(!response.IsSuccessStatusCode)
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
    }
}