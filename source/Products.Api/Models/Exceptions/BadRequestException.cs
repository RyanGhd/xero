using System;

namespace Products.Api.Models.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message, Exception innerException, string trackId) : base($"An error occurred. Please use trackId [{trackId}] to check the details", innerException)
        {

        }
    }
}