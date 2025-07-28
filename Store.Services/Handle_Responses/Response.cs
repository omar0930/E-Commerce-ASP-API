using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Handle_Responses
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Response(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        private string GetDefaultMessageForStatusCode(int statusCode)
        // using pattern matching switch
            => statusCode switch
            {
                200 => "Request has succeeded",
                201 => "Resource has been created successfully",
                204 => "No content to send for this request",
                301 => "The resource has been moved permanently",
                302 => "The resource has been found but moved temporarily",
                304 => "Not modified since the last request",
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                403 => "Forbidden: Access denied",
                404 => "Not Found ! Page not found",
                408 => "Request timed out",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
                502 => "Bad gateway received",
                503 => "Service unavailable at the moment",
                504 => "Gateway timeout occurred",
                _ => "Unknown error occurred",
            };

    }
}
