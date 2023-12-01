namespace RDx.Platform
{
    using UnityEngine;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine.Networking;
    using System.Collections.Generic;

    /// <summary>
    /// Object for retrieving an API token and making http requests.
    /// </summary>
    public static class RDxRequest
    {
        private const int DefaultTimeout = 100;

        public static IAsyncRequest Get<T>(string url, Action<T> response, Action<float> progressCallback = null) where T : class
        {
            // string url = string.Format(
            //     "{0}/{1}"
            //     , Constants.BaseAPI
            //     , path
            // );
            if (string.IsNullOrEmpty(url))
                return null;


            return new HTTPRequest(
                url, (Response responseRequest) =>
                {
                    string json = Encoding.UTF8.GetString(responseRequest.Data);

    

                    var result = JsonUtility.FromJson<T>(json);
                    if (responseRequest.Exceptions != null)
                        foreach (var exception in responseRequest.Exceptions)
                        {
                            Debug.LogError($"Error: {exception.Message}");
                        }
                    // Debug.Log($"Success: {json}: " + result);
                    // TileJSONResponse tileJSONResponse = JsonConvert.DeserializeObject<TileJSONResponse>(json);
                    // if (result != null)
                    // {
                    //     result.Source = tilesetName;
                    // }
                    response(result);
                }, timeout: DefaultTimeout);
        }

    }
}