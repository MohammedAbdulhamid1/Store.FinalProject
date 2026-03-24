namespace Store.FinalProject.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiErrorResponse(int statuscode,string message=null)
        {
            StatusCode = statuscode;
            Message = message??GetDeafultMessageForStatusCode(statuscode);
        }
        private string GetDeafultMessageForStatusCode(int statuscode)
        {
            var message = statuscode switch
            {
                400 => "a bad request,you have made",
                401 => " Authorized,you are not ",
                404 => "Resourse Not Found",
                500 => " ServerError",
                _ => null

            };
            return message;

        }
    }
}
