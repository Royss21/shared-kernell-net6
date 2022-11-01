
namespace SharedKernell.Exceptions
{
    using Microsoft.AspNetCore.Http;
    public class ControllerExcepcion : Exception
    {
        public int HttpStatusCode { get; }

        public ControllerExcepcion(string message, int httpStatusCode = StatusCodes.Status400BadRequest)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public ControllerExcepcion(string mensaje, Exception innerException,
            int httpStatusCode = StatusCodes.Status400BadRequest)
            : base(mensaje, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}