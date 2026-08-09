using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.DTO
{
    public class DeleteListCreateDto :LoginUserIdDto
    {
        public long Id { get; set; }
        public long DeleteListId { get; set; }
        public string DeleteListName { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }
    public class AddDeleteListRemarks : LoginUserIdDto
    {
        public List<DeleteListCreateDto> DeleteListRemark { get; set; }
        public AddDeleteListRemarks()
        {
            DeleteListRemark = new List<DeleteListCreateDto>();
        }
        public bool PostStatus { get; set; }
        public string PostMessage { get; set; }
    }
}
