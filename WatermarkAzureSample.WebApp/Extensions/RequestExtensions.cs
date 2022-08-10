using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.Http
{
    public static class RequestExtensions
    {
        public static string GetClientIPAddress(this HttpRequest request)
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
            return clientIpAddress;
        }
    }
}
