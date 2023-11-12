namespace YaleAccess.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public object? Data { get; set; }

        /// <summary>
        /// If passing in data only then the resonse is successful
        /// </summary>
        /// <param name="data"></param>
        public ApiResponse(object data)
        {
            Success = true;
            Data = data;
        }

        /// <summary>
        /// If passing in an error then the response is not successful
        /// </summary>
        /// <param name="error"></param>
        public ApiResponse(string error)
        {
            Success = false;
            Error = error;
        }
    }
}
