using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Chat.RestAPI.Data
{
    internal class RequestResult<T>
    {
        public string Status { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public string ErrorType { get; set; }

        public T Data { get; set; }
        public T Info { get; set; }
        public T Channel { get; set; }
    }
}
