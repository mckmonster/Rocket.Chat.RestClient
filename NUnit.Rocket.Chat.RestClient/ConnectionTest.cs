using NSubstitute;
using NUnit.Framework;
using RestSharp;
using Rocket.Chat.RestAPI;
using Rocket.Chat.RestAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Rocket.Chat.RestClient
{
    [TestFixture]
    public class ConnectionTest
    {
        private RocketChatConnection Connection;
        private IRestClient Client;

        [SetUp]
        public void Setup()
        {
            Client = Substitute.For<IRestClient>();
            Client.BaseHost = "https://toto.truc.com";
            Client.Execute<RequestResult<InfoResult>>(new RestRequest()).ReturnsForAnyArgs<IRestResponse<RequestResult<InfoResult>>>(new RestResponse<RequestResult<InfoResult>>()
            {
                Content = "{\n\"info\": {\n\"version\": \"0.59.3\"\n},\n\"success\": true\n}",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed,
                Data = new RequestResult<InfoResult>
                {
                    Info = new InfoResult { VersionString = "0.59.0" }
                }
            });

            Client.Execute<RequestResult<Authoring>>(new RestRequest()).ReturnsForAnyArgs(new RestResponse<RequestResult<Authoring>>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed,
                Data = new RequestResult<Authoring>
                {
                    Data = new Authoring
                    {
                        AuthToken = "311V-jh2yfaR0ZSZNvQYuL4KgoOexhPi3bWh0xm3DV0",
                        UserId = "w4mLEjtCsSRNyF7uH"
                    }
                }
            });

            Client.Execute(new RestRequest()).ReturnsForAnyArgs(new RestResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed
            });
        }

        [Test, Order(1)]
        public void Constructor()
        {
            Connection = new RocketChatConnection(Client);
            Framework.Assert.AreEqual(new Version(0, 59, 0), Connection.ServerVersion);
        }

        [Test, Order(2)]
        public void Logon()
        {
            Connection.Logon("toto", "toto@test1");
            Framework.Assert.IsTrue(Connection.IsLogged);
        }

        [Test, Order(3)]
        public void Logoff()
        {
            Connection.LogOff();
            Framework.Assert.IsFalse(Connection.IsLogged);
        }
    }
}
