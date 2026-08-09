using Adani.Solution.DTO;
using System.Collections.Generic;

namespace Adani.Solution.MVC.Models
{
    public class ResultViewModel : ResultDto
    {
        public List<string> ImageFileList { get; set; }
        public List<string> VideoFileList { get; set; }

        public ResultViewModel()
        {
            ImageFileList = new List<string>();
            VideoFileList = new List<string>();
        }
    }
}