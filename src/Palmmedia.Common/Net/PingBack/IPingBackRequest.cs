using CookComputing.XmlRpc;

namespace Palmmedia.Common.Net.PingBack
{
    /// <summary>
    /// Interface which is used to invoke a pingback request.
    /// </summary>
    [XmlRpcUrl("")]
    public interface IPingbackRequest : IXmlRpcProxy
    {
        /// <summary>
        /// Pingbacks the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns>The success message.</returns>
        [XmlRpcMethod]
        string Pingback(string source, string target);
    } 
}
