using System;

namespace AppMetadataAPIServer.Exceptions
{
    public class InvalidPayloadException : Exception
    {
        public InvalidPayloadException(string errMsg): base(errMsg)
        {
        }
    }
}