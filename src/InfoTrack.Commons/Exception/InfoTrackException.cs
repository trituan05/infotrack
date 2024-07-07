using System.Net;

namespace InfoTrack.Commons.Exception
{
    public abstract class InfoTrackException : System.Exception
    {
        public string ErrorCode { get; set; }

        public HttpStatusCode StatusCode { get; }

        protected InfoTrackException(string errorCode, string message, HttpStatusCode statusCode, System.Exception? inner = null) : base(message, inner)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
