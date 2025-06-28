using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Deeplinks.Help.Api.Infrastructure
{
    public static class Utils
    {
        /// <summary>
        /// Creates a safe URI from the input string, which is intended to be a domain (example.com).
        /// </summary>
        public static Uri? CreateSafeUri(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            input = input.Trim();

            // Try as-is (in case scheme is present)
            if (!Uri.TryCreate(input, UriKind.Absolute, out Uri? uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                // Prepend https:// if no valid scheme
                if (!Uri.TryCreate("https://" + input, UriKind.Absolute, out uri) ||
                    string.IsNullOrWhiteSpace(uri.Host))
                {
                    return null;
                }
            }

            // Block if host is an IP address
            if (IPAddress.TryParse(uri.Host, out var ip))
            {
                return null;
            }

            // Resolve host to IP
            try
            {
                var addresses = Dns.GetHostAddresses(uri.Host);
                foreach (var addr in addresses)
                {
                    if (IsPrivateOrLoopbackIp(addr))
                        return null;
                }
            }
            catch
            {
                // DNS resolution failed
                return null;
            }

            return uri;
        }

        /// <summary>
        /// Returns whether the given IP address is a private or loopback address.
        /// </summary>
        public static bool IsPrivateOrLoopbackIp(IPAddress ip)
        {
            if (IPAddress.IsLoopback(ip))
                return true;

            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                var bytes = ip.GetAddressBytes();
                return bytes[0] == 0 || // 0.0.0.0
                       bytes[0] == 10 || // 10.0.0.0/8
                       (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) || // 172.16.0.0/12
                       (bytes[0] == 192 && bytes[1] == 168); // 192.168.0.0/16
            }

            if (ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                return ip.IsIPv6LinkLocal || ip.IsIPv6SiteLocal || ip.IsIPv6Multicast;
            }

            return false;
        }

        /// <summary>
        /// Hash a string using MD5.
        /// </summary>
        public static string Hash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = MD5.HashData(inputBytes);

            var sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}
