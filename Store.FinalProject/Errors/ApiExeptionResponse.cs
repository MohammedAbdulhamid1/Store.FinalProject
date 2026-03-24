namespace Store.FinalProject.Errors
{
    public class ApiExeptionResponse:ApiErrorResponse
    {
        public string? Details { get; set; }
        public ApiExeptionResponse(int statuscode,string? message=null,string?details=null):base(statuscode,message)
        {
            Details = details;
        }

    }
}
