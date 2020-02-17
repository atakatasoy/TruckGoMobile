using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TruckGoMobile.Services
{
    public enum RequestType
    {
        Post,
        Get
    }

    public enum ControllerType
    {
        Home,
        User,
        App
    }

    public static class Helper
    {
        public static async Task<T> ApiCall<T>(RequestType type, ControllerType controller, string actionName, string inputParams = null) 
            where T : class
        {
            if (type == RequestType.Post && string.IsNullOrWhiteSpace(inputParams))
                throw new ArgumentNullException();
            else if (string.IsNullOrWhiteSpace(actionName))
                throw new ArgumentNullException();
            else if (type == RequestType.Get && !string.IsNullOrWhiteSpace(inputParams))
                throw new ArgumentNullException();

            T response = default(T);

            string url = $"{Utility.BaseURL}/api/{controller}/{actionName}";

            switch (type)
            {
                case RequestType.Get:
                    response = JsonConvert
                        .DeserializeObject<T>(
                        await HttpGetAsync(url));
                    break;
                case RequestType.Post:
                    response = JsonConvert
                        .DeserializeObject<T>(
                        await HttpPostAsync(url, inputParams));
                    break;
            }
            return response;
        }

        #region httpPostAsync
        public static async Task<string> HttpPostAsync(string url, string inputParams)
        {
            string response_val = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 1000 * 60 * 60 * 24;

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(inputParams);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response_val = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                var asd = e.Message;
                response_val = JsonConvert.SerializeObject(new BaseResponseModel { responseText = "İnternet bağlantınız da sorun olabilir", responseVal = -3 });
            }
            return response_val;
        }
        #endregion

        #region httpGetAsync
        public static async Task<string> HttpGetAsync(string url)
        {
            string responseVal = default(string);
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseVal = streamReader.ReadToEnd();
                }
            }
            catch(Exception e)
            {
                responseVal = JsonConvert.SerializeObject(new BaseResponseModel { responseText = "İnternet bağlantınız da sorun olabilir.", responseVal = -3 });
            }

            return responseVal;
        }
        #endregion

        public static async Task<string> GenericGetRequestAsync(string actionName,
            Dictionary<string, string> parameters,
            ControllerType controllerName = ControllerType.User)
        {
            var requestUrl = $"{Utility.BaseURL}/api/{controllerName}/{actionName}";
            if (parameters != null)
            {
                requestUrl += "?";
                var Keys = new List<string>(parameters.Keys);

                for (int i = 0; i < Keys.Count; i++)
                {
                    if (i == Keys.Count - 1)
                    {
                        requestUrl += $"{Keys[i]}={parameters[Keys[i]]}";
                        continue;
                    }
                    requestUrl += $"{Keys[i]}={parameters[Keys[i]]}&";
                }
            }

            var result = await HttpGetAsync(requestUrl);

            return result;
        }

        public static string ConvertCollectionToString<T>(List<T> element, char seperator)
        {
            string buffer = null;
            for (int i = 0; i < element.Count; i++)
            {
                if (i == element.Count - 1)
                {
                    buffer += $"{element[i]}";
                    continue;
                }
                buffer += $"{element[i]}{seperator}";
            }
            return buffer;
        }
    }
}
