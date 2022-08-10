using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Http
{
    public static class RequestExtensions
    {
        public static string GetClientIPAddress(this HttpRequest request, bool withPort = false)
        {
            string clientIpAddress = string.Empty;
            if (!string.IsNullOrEmpty(request.Headers["X-Forwarded-For"]))
            {
                clientIpAddress = request.Headers["X-Forwarded-For"];
            }
            else
            {
                var ipAddress = request.HttpContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress;
                if (ipAddress != null)
                {
                    clientIpAddress = ipAddress.ToString();
                }
            }
            return withPort ? clientIpAddress : RemovePort(clientIpAddress);
        }

        static string RemovePort(string ipAddressWithPort)
        {
            if (ipAddressWithPort == "::1")
            {
                return ipAddressWithPort;
            }
            if (ipAddressWithPort.IndexOf(":") > -1)
            {
                return ipAddressWithPort.Substring(0, ipAddressWithPort.IndexOf(":"));
            }
            return ipAddressWithPort;
        }
    }
}
