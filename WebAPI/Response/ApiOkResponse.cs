namespace WebAPI.Response
{
    public class ApiOkResponse<T> : ApiResponse
    {
        public T Result { get; }
        public PageMetadata PageMetadata { get; }

        public ApiOkResponse(T result, PageMetadata metadata = null)
            : base(200)
        {
            Result = result;
            PageMetadata = metadata;
        }
    }

    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }
        public PageMetadata PageMetadata { get; }

        public ApiOkResponse(object result, PageMetadata metadata = null)
            : base(200)
        {
            Result = result;
            PageMetadata = metadata;
        }
    }
}
