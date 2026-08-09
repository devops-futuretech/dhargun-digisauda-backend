using Newtonsoft.Json;

namespace Adani.Solution.DTO
{
    public class SuccessDto
    {
        [JsonProperty(PropertyName = "response")]
        public object Response { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public SuccessDto()
        {
            Response = string.Empty;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
