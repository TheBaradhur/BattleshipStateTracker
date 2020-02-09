using System.Collections.Generic;

namespace BattleshipStateTracker.Api.Models
{
    public class ApiErrorResponse
    {
        public const string BadRequestType = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        public const string NotFoundType = "https://tools.ietf.org/html/rfc7231#section-6.5.4";

        public const string InternalServerErrorType = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

        public string Type { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string TraceId { get; set; }

        public List<string> Errors { get; set; }

        public static ApiErrorResponse GetCustomBadRequest(string title, string traceId, List<string> errors)
        {
            return new ApiErrorResponse
            {
                Status = Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest.ToString(),
                Type = BadRequestType,
                Title = title,
                TraceId = traceId,
                Errors = errors
            };
        }

        public static ApiErrorResponse GetCustomNotFound(string title, string traceId, List<string> errors)
        {
            return new ApiErrorResponse
            {
                Status = Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound.ToString(),
                Type = NotFoundType,
                Title = title,
                TraceId = traceId,
                Errors = errors
            };
        }

        public static ApiErrorResponse GetCustomInternalServerError(string title, string traceId, List<string> errors)
        {
            return new ApiErrorResponse
            {
                Status = Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError.ToString(),
                Type = InternalServerErrorType,
                Title = title,
                TraceId = traceId,
                Errors = errors
            };
        }
    }
}