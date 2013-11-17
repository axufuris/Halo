using System.Web;

namespace Halo.Utilities
{
    public class Mobile
    {
        /// <summary>
        /// Determines whether [is mobile device] [the specified request].
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>
        ///   <c>true</c> if [is mobile device] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsMobileDevice(HttpRequest request)
        {
            return IsMobileDevice(request.UserAgent.ToString(), request.Browser.IsMobileDevice);
        }

        /// <summary>
        /// Determines whether [is mobile device] [the specified user agent].
        /// </summary>
        /// <param name="userAgent">The user agent. "Request.UserAgent"</param>
        /// <param name="requestBrowserIsMobileDevice">if set to <c>true</c> [request browser is mobile device]. "Request.Browser.IsMobileDevice."</param>
        /// <returns>
        ///   <c>true</c> if [is mobile device] [the specified user agent]; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsMobileDevice(string userAgent, bool requestBrowserIsMobileDevice)
        {
            string strUserAgent = userAgent.ToLower().Trim();

            if (strUserAgent != null)
            {
                if (requestBrowserIsMobileDevice == true || strUserAgent.Contains("iphone") ||
                    strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
                    strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") ||
                    strUserAgent.Contains("palm") || strUserAgent.Contains("android") ||
                    strUserAgent.Contains("ipad") || strUserAgent.Contains("moto") ||
                    strUserAgent.Contains("htc") || strUserAgent.Contains("sony") ||
                    strUserAgent.Contains("panasonic") || strUserAgent.Contains("midp") ||
                    strUserAgent.Contains("cldc") || strUserAgent.Contains("avant") ||
                    strUserAgent.Contains("windows ce") || strUserAgent.Contains("nokia") ||
                    strUserAgent.Contains("pda") || strUserAgent.Contains("hand") ||
                    strUserAgent.Contains("mobi") || strUserAgent.Contains("240x320") ||
                    strUserAgent.Contains("voda"))
                {
                    return true;
                }
            }

            return false;
        }
    }  /// End of Class
}
