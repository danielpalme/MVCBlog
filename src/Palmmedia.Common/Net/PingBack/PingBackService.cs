using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using CookComputing.XmlRpc;
using log4net;

namespace Palmmedia.Common.Net.PingBack
{
    /// <summary>
    /// Provides methods to perform pingback requests and to validate received pingback requests.
    /// </summary>
    public static class PingBackService
    {
        /// <summary>
        /// Pattern used to identify URLs.
        /// </summary>
        private const string URIPATTERN = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PingBackService));

        /// <summary>
        /// Performs a pingback request to all links in the given <paramref name="document"/>.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <param name="document">The document.</param>
        /// <returns>A list of <see cref="PingBackResult">PingBackResults</see>.</returns>
        public static IEnumerable<PingBackResult> PerformPingBack(string sourceUrl, string document)
        {
            if (sourceUrl == null)
            {
                throw new ArgumentNullException("sourceUrl");
            }

            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            var result = new List<PingBackResult>();

            foreach (var link in GetHyperlinks(document))
            {
                var pingBackResult = new PingBackResult();
                pingBackResult.Url = link;
                result.Add(pingBackResult);

                string pingbackURL = null;
                try
                {
                    pingbackURL = AutoDiscoverPingbackUrl(link);
                }
                catch (Exception ex)
                {
                    pingBackResult.Message = ex.Message;
                    continue;
                }

                if (pingbackURL == null)
                {
                    pingBackResult.Message = "Could not determine Pingback URL.";
                    continue;
                }

                try
                {
                    var proxy = (IPingbackRequest)XmlRpcProxyGen.Create(typeof(IPingbackRequest));
                    proxy.Url = pingbackURL;
                    proxy.XmlRpcMethod = "pingback.ping";
                    pingBackResult.Message = proxy.Pingback(sourceUrl, link);
                    pingBackResult.Success = true;
                }
                catch (XmlRpcFaultException ex)
                {
                    pingBackResult.Message = ex.FaultCode + " " + ex.FaultString;
                }
                catch (Exception ex)
                {
                    pingBackResult.Message = ex.Message;
                }
            }

            return result;
        }

        /// <summary>
        /// Validates a pingback request. If anything is wrong with the request, the corresponding <see cref="XmlRpcFaultException"/> is thrown (according to the Pingback specification: http://www.hixie.ch/specs/pingback/pingback).
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <param name="targetUri">The target URI.</param>
        /// <param name="isTargetPingbackEnabled">Determines whether the target URI supports receiving pingbacks.</param>
        /// <param name="doesPingbackAlreadyExist">Determines whether the source URI has already registered a pingback.</param>
        /// <param name="registerPingback">Registers the pingback.</param>
        /// <exception cref="XmlRpcFaultException"> if request is invalid.</exception>
        public static void ValidatePingbackRequest(
            string sourceUri, 
            string targetUri, 
            Func<string, bool> isTargetPingbackEnabled,
            Func<string, bool> doesPingbackAlreadyExist,
            Action<string> registerPingback)
        {
            if (string.IsNullOrEmpty(sourceUri) || !Regex.IsMatch(sourceUri, URIPATTERN, RegexOptions.Compiled))
            {
                throw new XmlRpcFaultException(16, "The source URI does not exist.");
            }

            if (string.IsNullOrEmpty(targetUri) || !Regex.IsMatch(targetUri, URIPATTERN, RegexOptions.Compiled))
            {
                throw new XmlRpcFaultException(32, "The specified target URI does not exist.");
            }

            try
            {
                if (!DoesSourceContainLinkToTarget(sourceUri, targetUri))
                {
                    Logger.Error("Failed to validate pingback. Source URI does not contain a link to the target (Source: " + sourceUri + ", Target: " + targetUri + ").");
                    throw new XmlRpcFaultException(17, "The source URI does not contain a link to the target URI, and so cannot be used as a source.");
                }

                if (!isTargetPingbackEnabled(targetUri))
                {
                    Logger.Error("Failed to validate pingback. Target is not pingback enabled (Source: " + sourceUri + ", Target: " + targetUri + ").");
                    throw new XmlRpcFaultException(33, "The specified target URI cannot be used as a target.");
                }

                if (doesPingbackAlreadyExist(sourceUri))
                {
                    throw new XmlRpcFaultException(48, "The pingback has already been registered.");
                }

                registerPingback(sourceUri);
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to validate pingback (Source: " + sourceUri + ", Target: " + targetUri + ").", ex);
                throw new XmlRpcFaultException(0, "Failed to register the pingback.");
            }
        }

        /// <summary>
        /// Tries to discover the pingback URL from the given website
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The Pingback URL if available, otherwise <c>null</c>.</returns>
        private static string AutoDiscoverPingbackUrl(string url)
        {
            var request = HttpWebRequest.Create(url);

            var stream = request.GetResponse().GetResponseStream();

            string pingbackUrlFromHeader = request.GetResponse().Headers["X-Pingback"];

            if (!string.IsNullOrEmpty(pingbackUrlFromHeader))
            {
                return pingbackUrlFromHeader;
            }
            else
            {
                using (var reader = new StreamReader(stream))
                {
                    string htmlText = reader.ReadToEnd();

                    var match = Regex.Match(htmlText, "<link rel=\"pingback\" href=\"([^\"]+)\" ?/?>", RegexOptions.Compiled);

                    if (match.Success)
                    {
                        return match.Groups[1].Value.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"");
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Extracts all links from a <see cref="string"/>.
        /// </summary>
        /// <param name="document">The <see cref="string"/>.</param>
        /// <returns>The links.</returns>
        private static IEnumerable<string> GetHyperlinks(string document)
        {
            var result = new HashSet<string>();

            var matches = Regex.Matches(document, "<a .*?href=\"(http.+?)\"", RegexOptions.Compiled);

            for (int i = 0; i < matches.Count; i++)
            {
                result.Add(matches[i].Groups[1].Value);
            }

            return result;
        }

        /// <summary>
        /// Checks if the website with the sourceUri contains a link to the targetUri.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <param name="targetUri">The target URI.</param>
        /// <returns><c>true</c> if website with the sourceUri contains a link to the targetUri, otherwise <c>false</c>.</returns>
        private static bool DoesSourceContainLinkToTarget(string sourceUri, string targetUri)
        {
            var request = HttpWebRequest.Create(sourceUri);

            var stream = request.GetResponse().GetResponseStream();
            using (var reader = new StreamReader(stream))
            {
                string htmlText = reader.ReadToEnd();

                return htmlText.Contains(targetUri);
            }
        }
    }
}
