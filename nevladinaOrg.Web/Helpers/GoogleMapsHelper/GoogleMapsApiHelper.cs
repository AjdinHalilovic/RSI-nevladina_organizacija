using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Helpers.GoogleMapsHelper
{
    public class GoogleMapsApiHelper
    {
        private static HttpClient _client = new HttpClient() { BaseAddress = new Uri("https://710dec45.ngrok.io") };
        private string _route { get; set; }
        private string _appId { get; set; }

        public GoogleMapsApiHelper(string route = "https://maps.googleapis.com/maps/api")
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json").Build();
            _route = route;
            _appId = config["AppId"];
        }

        public HttpResponseMessage GetResponse(string parameter = "")
        {
            try
            {
                return _client.GetAsync($"{_route}/{parameter}/?key={_appId}").Result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetResponseAsync(string parameter = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Remove("appId");
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                return await _client.GetAsync($"{_route}/{parameter}/?key={_appId}");
            }
            catch
            {
                return null;
            }
        }

        public HttpResponseMessage GetActionResponse(string action, string parameter = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                return _client.GetAsync($"{_route}/{action}/{parameter}").Result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> GetActionResponseAsync(string action, string parameter = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                return await _client.GetAsync($"{_route}/{action}/{parameter}");
            }
            catch
            {
                return null;
            }
        }

        public HttpResponseMessage PostResponse(Object newObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                StringContent jsonObject = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = _client.PostAsync(_route, jsonObject).Result;

                return responseMessage;
            }
            catch
            {
                return null;
            }
        }

        public HttpResponseMessage PostActionResponse(string action, Object newObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                StringContent jsonObject = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = _client.PostAsync($"{_route}/{action}", jsonObject).Result;

                return responseMessage;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> PostActionResponseAsync(string action, Object newObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                StringContent jsonObject = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = await _client.PostAsync($"{_route}/{action}", jsonObject);

                return responseMessage;
            }
            catch
            {
                return null;
            }
        }

        public HttpResponseMessage PutResponse(int id, Object existingObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                StringContent jsonObject = new StringContent(JsonConvert.SerializeObject(existingObject), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = _client.PutAsync($"{_route}/{id}", jsonObject).Result;

                return responseMessage;
            }
            catch
            {
                return null;
            }
        }

        public HttpResponseMessage PutActionResponse(int id, string action, Object existingObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                StringContent jsonObject = new StringContent(JsonConvert.SerializeObject(existingObject), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = _client.PutAsync($"{_route}/{id}/{action}", jsonObject).Result;

                return responseMessage;
            }
            catch
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> PutActionResponseAsync(int id, string action, Object existingObject)
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                StringContent jsonObject = new StringContent(JsonConvert.SerializeObject(existingObject), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = await _client.PutAsync($"{_route}/{id}/{action}", jsonObject);

                return responseMessage;
            }
            catch
            {
                return null;
            }
        }


        public HttpResponseMessage DeleteResponse(int id)
        {
            try
            {
                if (!string.IsNullOrEmpty(_appId))
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("appId", _appId);
                }

                return _client.DeleteAsync($"{_route}/{id}").Result;
            }
            catch
            {
                return null;
            }
        }
    }
}
