using System;

namespace Products.Api.Models
{
    public class ActionResult
    {
        public bool WasSuccessful { get; }
        public int ErrorCode { get; }
        public string ErrorMessage { get; }

        public ActionResult()
        {
            WasSuccessful = true;
        }
        public ActionResult(int errorCode,string errorMessage)
        {
            WasSuccessful = false;

            if (errorCode != 400 || errorCode != 404)
                throw new NotImplementedException($"Error code {errorCode} is not implemented.");

            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}