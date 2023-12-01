//-----------------------------------------------------------------------
// <copyright file="Response.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if UNITY_2017_1_OR_NEWER
#define UNITY
#endif

namespace RDx.Platform
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    // using Utils;

    using UnityEngine.Networking;

    /// <summary> A response from a <see cref="IFileSource" /> request. </summary>
    public class Response
    {


        private Response() { }


        public IAsyncRequest Request { get; private set; }


        public bool RateLimitHit
        {
            get { return StatusCode.HasValue ? 429 == StatusCode.Value : false; }
        }


        /// <summary>Flag to indicate if the request was successful</summary>
        public bool HasError
        {
            get { return _exceptions == null ? false : _exceptions.Count > 0; }
        }

        /// <summary>Flag to indicate if the request was fullfilled from a local cache</summary>
        public bool LoadedFromCache;

        /// <summary>Flag to indicate if the request was issued before but was issued again and updated</summary>
        public bool IsUpdate = false;

        public string RequestUrl;


        public int? StatusCode;


        public string ContentType;


        /// <summary>Length of rate-limiting interval in seconds. https://www.mapbox.com/api-documentation/#rate-limit-headers </summary>
        public int? XRateLimitInterval;


        /// <summary>Maximum number of requests you may make in the current interval before reaching the limit. https://www.mapbox.com/api-documentation/#rate-limit-headers </summary>
        public long? XRateLimitLimit;


        /// <summary>Timestamp of when the current interval will end and the ratelimit counter is reset. https://www.mapbox.com/api-documentation/#rate-limit-headers </summary>
        public DateTime? XRateLimitReset;


        private List<Exception> _exceptions;
        /// <summary> Exceptions that might have occured during the request. </summary>
        public ReadOnlyCollection<Exception> Exceptions
        {
            get { return null == _exceptions ? null : _exceptions.AsReadOnly(); }
        }


        /// <summary> Messages of exceptions otherwise empty string. </summary>
        public string ExceptionsAsString
        {
            get
            {
                if (null == _exceptions || _exceptions.Count == 0) { return string.Empty; }
                return string.Join(Environment.NewLine, _exceptions.Select(e => e.Message).ToArray());
            }
        }


        /// <summary> Headers of the response. </summary>
        public Dictionary<string, string> Headers;


        /// <summary> Raw data fetched from the request. </summary>
        public byte[] Data;

        public void AddException(Exception ex)
        {
            if (null == _exceptions) { _exceptions = new List<Exception>(); }
            _exceptions.Add(ex);
        }

        // TODO: we should store timestamp of the cache!
        public static Response FromCache(byte[] data)
        {
            Response response = new Response();
            response.Data = data;
            response.LoadedFromCache = true;
            return response;
        }


        // within Unity or UWP build from Unity
        public static Response FromWebResponse(IAsyncRequest request, UnityWebRequest apiResponse, Exception apiEx)
        {

            Response response = new Response();
            response.Request = request;

            if (null != apiEx)
            {
                response.AddException(apiEx);
            }

            // additional string.empty check for apiResponse.error:
            // on UWP isNetworkError is sometimes set to true despite all being well
            if (apiResponse.isNetworkError || !string.IsNullOrEmpty(apiResponse.error))
            {
                response.AddException(new Exception(apiResponse.error));
            }

            if (request.RequestType != HttpRequestType.Head)
            {
                if (null == apiResponse.downloadHandler.data)
                {
                    response.AddException(new Exception("Response has no data."));
                }
            }

            StringComparison stringComp = StringComparison.InvariantCultureIgnoreCase;

            Dictionary<string, string> apiHeaders = apiResponse.GetResponseHeaders();
            if (null != apiHeaders)
            {
                response.Headers = new Dictionary<string, string>();
                foreach (var apiHdr in apiHeaders)
                {
                    string key = apiHdr.Key;
                    string val = apiHdr.Value;
                    response.Headers.Add(key, val);
                    // if (key.Equals("X-Rate-Limit-Interval", stringComp))
                    // {
                    //     int limitInterval;
                    //     if (int.TryParse(val, out limitInterval)) { response.XRateLimitInterval = limitInterval; }
                    // }
                    // else if (key.Equals("X-Rate-Limit-Limit", stringComp))
                    // {
                    //     long limitLimit;
                    //     if (long.TryParse(val, out limitLimit)) { response.XRateLimitLimit = limitLimit; }
                    // }
                    // else if (key.Equals("X-Rate-Limit-Reset", stringComp))
                    // {
                    //     double unixTimestamp;
                    //     if (double.TryParse(val, out unixTimestamp))
                    //     {
                    //         response.XRateLimitReset = UnixTimestampUtils.From(unixTimestamp);
                    //     }
                    // } else 
                    if (key.Equals("Content-Type", stringComp))
                    {
                        response.ContentType = val;
                    }
                }
            }

            int statusCode = (int)apiResponse.responseCode;
            response.StatusCode = statusCode;

            if (statusCode != 200)
            {
                response.AddException(new Exception(string.Format("Status Code {0}", apiResponse.responseCode)));
            }
            if (429 == statusCode)
            {
                response.AddException(new Exception("Rate limit hit"));
            }

            if (request.RequestType != HttpRequestType.Head)
            {
                response.Data = apiResponse.downloadHandler.data;
            }

            return response;
        }



    }
}
