using Newtonsoft.Json;

namespace Adani.Solution.DTO
{
    public class ErrorDto
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "errorcode")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "response")]
        public object Response { get; set; }

        public ErrorDto()
        {
            Message = string.Empty;
            ErrorCode = string.Empty;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
