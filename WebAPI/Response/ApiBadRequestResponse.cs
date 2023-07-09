using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAPI.Response
{
    public class ApiBadRequestResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; }

        public ApiBadRequestResponse(ModelStateDictionary modelState)
            : base(400)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }
            Errors = modelState.SelectMany(x => x.Value.Errors.Select(y => x.Key + ": " + y.ErrorMessage));
        }
    }
}
