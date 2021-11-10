namespace Products.Api.Models
{
    public class NotFoundErrorResponse
    {
        public const string ErrorFormat = "{0} not found";

        public string ErrorMessage { get; }
        public string TrackId { get; }

        public NotFoundErrorResponse(string entityName, string trackId)
        {
            ErrorMessage = string.Format(ErrorFormat, entityName);
            TrackId = trackId;
        }
    }
}