using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Chat.RestAPI.Data
{
    internal class InfoResult
    {
        [DeserializeAs(Name = "version")]
        public string VersionString
        {
            get
            {
                return Version.ToString();
            }
            set
            {
                Version = new Version(value);
            }
        }

        public Version Version;

    }
}
