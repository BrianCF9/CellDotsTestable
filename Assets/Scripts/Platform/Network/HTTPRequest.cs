//-----------------------------------------------------------------------
// <copyright file="HTTPRequest.cs" company="Riolab">
//     Copyright (c) 2023 Riolab. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace RDx.Platform
{
    using System;
    using UnityEngine.Networking;
    using System.Threading.Tasks;

    internal sealed class HTTPRequest : IAsyncRequest
    {
        private UnityWebRequest _request;
        private HttpRequestType _requestType;
        private int _timeout;
        private readonly Action<Response> _callback;

        public bool IsCompleted { get; private set; }

        public HttpRequestType RequestType { get { return _requestType; } }

        public HTTPRequest(string url, Action<Response> callback, int timeout, HttpRequestType requestType = HttpRequestType.Get, string body = null)
        {
            IsCompleted = false;
            _requestType = requestType;
            switch (_requestType)
            {
                case HttpRequestType.Get:
                    _request = UnityWebRequest.Get(url);
                    break;
                case HttpRequestType.Head:
                    _request = UnityWebRequest.Head(url);
                    break;
                // case HttpRequestType.Post:
                //     _request = UnityWebRequest.PostWwwForm(url, body);
                //     break;
                default:
                    _request = UnityWebRequest.Get(url);
                    break;
            }
            _request.timeout = timeout;
            _callback = callback;

            DoRequestAsync();
        }

        public void Cancel()
        {
            if (_request != null)
            {
                _request.Abort();
            }
        }
        private async void DoRequestAsync()
        {
            // otherwise requests don't work in Edit mode, eg geocoding
            // also lot of EditMode tests fail otherwise
#pragma warning disable 0618
            _request.Send();
#pragma warning restore 0618
            while (!_request.isDone) { await Task.Yield(); }


            var response = Response.FromWebResponse(this, _request, null);

            _callback(response);
            _request.Dispose();
            _request = null;
            IsCompleted = true;
        }

    }
}