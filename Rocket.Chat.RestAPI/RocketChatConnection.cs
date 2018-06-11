using RestSharp;
using Rocket.Chat.RestAPI.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("RestSharp")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
[assembly: InternalsVisibleTo("NUnit.Rocket.Chat.RestClient")]

namespace Rocket.Chat.RestAPI
{
    public partial class RocketChatConnection
    {
        internal IRestClient Client;
            
        internal static InfoResult Info;
        internal static Authoring Authoring { get; private set; }

        public Version ServerVersion => Info.Version;
        public bool IsLogged => Authoring != null;

        public RocketChatConnection(IRestClient restClient)
        {
            Client = restClient;
            var request = new RestRequest("api/v1/info", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };

            var response = Client.Execute<RequestResult<InfoResult>>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Response is not successful");
            }

            Info = response.Data.Info;
        }

        public Task LogonAsync(string username, string password) => Task.Run(() => Logon(username, password));
        public void Logon(string username, string password)
        {
            var request = new RestRequest("api/v1/login", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            var json = $"{{ \"user\" : \"{username}\", \"password\" : \"{password}\" }}";
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Client.Execute<RequestResult<Authoring>>(request);    
            if (!response.IsSuccessful)
            {
                throw new Exception("Response is not successful");
            }

            Authoring = response.Data.Data;
        }

        public Task LogOffAsync() => Task.Run(() => LogOff());
        public void LogOff()
        {
            var request = new RestRequest("api/v1/logout", Method.GET)
            {
                RequestFormat = DataFormat.Json
            };

            request.AddHeader("X-Auth-Token", Authoring.AuthToken);
            request.AddHeader("X-User-Id", Authoring.UserId);

            var response = Client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Response is not successful");
            }

            Authoring = null;
        }
    }
}
