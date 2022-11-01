namespace SharedKernell.Wrappers
{
    public class JsonResult<TData>
    {
        public JsonResult()
        {
        }

        public JsonResult(TData data, bool ok = true)
        {
            Data = data;
            Ok = ok;
        }

        public JsonResult(TData data, string message, bool ok = true)
        {
            Data = data;
            Message = message;
            Ok = ok;
        }
        
        public string Message { get; set; }
        public TData Data { get; set; }
        public bool Ok { get; set; }
    }
}