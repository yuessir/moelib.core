using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MoeLibCore
{
    public static class HostServer
    {
        public static int CurrentManagedThreadId
        {
            get { return Environment.CurrentManagedThreadId; }
        }

        public static string IP
        {
            get { return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString(); }
        }

        public static bool Is64BitOperatingSystem
        {
            get { return Environment.Is64BitOperatingSystem; }
        }

        public static bool Is64BitProcess
        {
            get { return Environment.Is64BitProcess; }
        }

        public static string MachineName
        {
            get { return Environment.MachineName; }
        }

        public static string OSVersion
        {
            get { return Environment.OSVersion.VersionString; }
        }

        public static int ProcessorCount
        {
            get { return Environment.ProcessorCount; }
        }

        public static string RuntimeVersion
        {
            get { return Environment.Version.ToString(); }
        }
    }
}