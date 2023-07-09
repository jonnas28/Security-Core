namespace WebAPI.Response
{
    public class ApiResponse
    {
        public int StatusCode { get; }
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                //add new case to change defualt message status code
                case 404:
                    return "Resource not found";
                case 401:
                    return "Unauthorized Access!";
                case 500:
                    return "An unhandled error occurred";
                case 200:
                    return "Successful";
                case 204:
                    return "Successful, No Content.";
                default:
                    return "Something went wrong!";
            }
        }
    }
}
