using Newtonsoft.Json;
using System;

namespace Adani.Solution.DTO
{
    public class SapDataSyncResultDto
    {
        public DateTime SyncStartedDateTime { get; set; }
        public DateTime SyncCompletedDateTime { get; set; }
        public DataSyncDto OutstandingResult { get; set; }

        [JsonProperty(PropertyName = "response")]
        public object ErrorDetailsResponse { get; set; }

        [JsonProperty(PropertyName = "response")]
        public object TotalInputRecordDetailsResponse { get; set; }
        [JsonProperty(PropertyName = "response")]
        public object SuccessRecordDetailsResponse { get; set; }
        public string ExceptionMessage { get; set; }

        public SapDataSyncResultDto()
        {
            OutstandingResult = new DataSyncDto();            
        }
    }

    public class DataSyncDto
    {
        public long DataRetrieved { get; set; }
        public long DataSynced { get; set; }
    }
}
