namespace Palmmedia.Common.Net.PingBack
{
    /// <summary>
    /// Result of a pingback request.
    /// </summary>
    public class PingBackResult
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pingback was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
