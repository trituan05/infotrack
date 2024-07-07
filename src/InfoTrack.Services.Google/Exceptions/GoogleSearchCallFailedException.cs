using InfoTrack.Commons.Exception;
using System.Net;

namespace InfoTrack.Services.Google.Exceptions
{
    internal class GoogleSearchCallFailedException: InfoTrackException
    {
        public GoogleSearchCallFailedException() :
            base("GoogleSearchCallFailed", $"Error while executing the google search.", HttpStatusCode.InternalServerError)
        {
        }
    }
}
