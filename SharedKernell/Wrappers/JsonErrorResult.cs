namespace SharedKernell.Wrappers
{
    public class JsonErrorResult
    {
        public int StatusCode { get; set; }
        public bool Ok { get; set; }
        public string? Message { get; set; }
        public JsonErrorResult(int codigoEstado, string? mensaje = null)
        {
            StatusCode = codigoEstado;
            Message = mensaje ?? GetDefaultMessageStatusCode(codigoEstado);
            Ok = GetSuccess();
        }
        private string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "El Request enviado tiene errores",
                401 => "No tienes autorization para este recurso",
                404 => "No se encontro el recurso solicitado",
                500 => "Se producieron errores en el servidor",
                _ => string.Empty
            };
        }

        private bool GetSuccess()
        {
            return StatusCode >= 200 && StatusCode < 300 ? true : false;
        }
    }
}