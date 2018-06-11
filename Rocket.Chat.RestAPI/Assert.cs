using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Chat.RestAPI
{
    internal static class Assert
    {
        public static void ThrowIfVersionLowerThan(int major, int minor, int build)
        {
            if (new Version(major, minor, build) > RocketChatConnection.Info.Version)
                throw new Exception($"Your Server version is lower than {major}.{minor}.{build}");
        }
    }
}
