using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Traffic.Utilities.Constants;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Traffic.Utilities.Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsync<T>(HttpClient httpClient, string endpoint, object data) where T : class, new()
        {
            try
            {
                var uri = data == null ? endpoint : $"{endpoint}?{GetQueryString(data)}";
                var response = httpClient.GetAsync(uri);
                var result = await GetHttpResponseMessage<T>(response.Result).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public static async Task<T> PostAsync<T>(HttpClient httpClient, string endpoint, object data) where T : class, new()
        {
            return await SendAsync<T>(httpClient, HttpMethod.Post, endpoint, data).ConfigureAwait(false);
        }

        public static async Task<T> PutAsync<T>(HttpClient httpClient, string endpoint, object data) where T : class, new()
        {
            return await SendAsync<T>(httpClient, HttpMethod.Put, endpoint, data).ConfigureAwait(false);
        }

        public static async Task<T> DeleteAsync<T>(HttpClient httpClient, string endpoint, object data) where T : class, new()
        {
            return await SendAsync<T>(httpClient, HttpMethod.Delete, endpoint, data).ConfigureAwait(false);
        }

        private static async Task<T> SendAsync<T>(HttpClient httpClient, HttpMethod httpMethod, string endpoint, object data) where T : class, new()
        {
            try
            {
                var content = Serialize(data);
                var response = httpClient.SendAsync(new HttpRequestMessage(httpMethod, new Uri(endpoint)) { Content = Serialize(data) });
                var result = await GetHttpResponseMessage<T>(response.Result).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        private static async Task<T> GetHttpResponseMessage<T>(HttpResponseMessage httpResponseMessage) where T : class, new()
        {
            var methodResult = new T();

            try
            {
                if (httpResponseMessage.IsSuccessStatusCode || httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    methodResult = JsonConvert.DeserializeObject<T>(responseString);
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }

            return methodResult;
        }

        private static HttpContent Serialize(object data)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            }), Encoding.UTF8, "application/json");

            return stringContent;
        }

        private static string GetQueryString(object data)
        {
            var properties = from p in data.GetType().GetProperties()
                                where p.GetValue(data, null) != null
                                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(data, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        public static HttpClient SMTClient(IConfiguration configuration)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var httpClient = new HttpClient(clientHandler);
            httpClient.BaseAddress = new Uri(configuration["SMT:BaseUrl"]);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CommonConstants.MediaTypeWithQualityHeaderValue));

            var userName = configuration["SMT:Credential:UserName"];
            var password = Cryptography.DecryptString(configuration["SMT:Credential:Password"]);
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{password}"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                CommonConstants.BasicSchema,
                base64String);

            return httpClient;
        }

        public static HttpClient ARMClient(IConfiguration configuration, string token)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var httpClient = new HttpClient(clientHandler);
            httpClient.BaseAddress = new Uri(configuration["ARM:BaseUrl"]);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CommonConstants.MediaTypeWithQualityHeaderValue));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                CommonConstants.BearerSchema,
                token);

            return httpClient;
        }
    }
}
