using Newtonsoft.Json;

namespace PaidGame.Server.Models.Responses
{
    public class ErrorResponse
    {
        /// <summary>
        /// Error description
        /// </summary>
        public string Error { get; set; }

        public ErrorResponse(string error)
        {
            Error = error;
        }
    }
}