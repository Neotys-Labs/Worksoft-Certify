namespace wsTransferToNeoLoad
{
    public class ActionResult
    {
        public ActionResult(bool status, string message, string value)
        {
            Status = status;
            Message = message;
            Value = value;
        }

        public bool Status { get; }
        public string Message { get; }
        public string Value { get; }
    }
}