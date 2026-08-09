using System;
namespace Adani.Solution.DTO
{
    public class TodayTickerListDto
    {
        public string Content { get; set; }
        public TimeSpan FromHours { get; set; }
        public TimeSpan ToHours { get; set; }
        public string ColorCode { get; set; }
    }
}
