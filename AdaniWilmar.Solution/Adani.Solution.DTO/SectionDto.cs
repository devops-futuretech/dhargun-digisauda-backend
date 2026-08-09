using System;
using System.Collections.Generic;

namespace Adani.Solution.DTO
{
    public class SectionDto : LoginUserIdDto
    {
        public long SectionId { get; set; }
        public string SectionName { get; set; }
        public bool IsActive { get; set; }
        public IList<SubmittedFormQuestionViewDto> Questions { get; set; }
        public SectionDto()
        {
            Questions = new List<SubmittedFormQuestionViewDto>();
        }
        /// <summary>
        /// TO create unique name in hierarchical grid
        /// GUID used as Grid name
        /// </summary>
        public string GuidValue
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
    }

    public class SectionIdDto
    {
        public long SectionId { get; set; }
    }
}
