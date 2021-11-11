namespace Products.Api.Models.Exceptions
{
    public class ErrorResponse
    {
        public const string ErrorFormat = "An error occurred. Please trackId [{0}] to review the details.";

        public string ErrorMessage { get; }
        public string TrackId { get; }

        public ErrorResponse(string trackId)
        {
            ErrorMessage = string.Format(ErrorFormat, trackId);
            TrackId = trackId;
        }
    }
}