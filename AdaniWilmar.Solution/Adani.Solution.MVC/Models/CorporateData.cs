using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.Solution.MVC.Models
{
    public class CorporateData
    {
        public CorporateData(string FirstName, string LastName, string Title, string Image, string ColorScheme, long RoleId, long? HierarchyId)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Title = Title;
            this.Image = Image;
            this.ColorScheme = ColorScheme;
            this.RoleId = RoleId;
            this.HierarchyId = HierarchyId;
            Items = new List<CorporateData>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string ColorScheme { get; set; }
        public long RoleId { get; set; }
        public long? HierarchyId { get; set; }
        public List<CorporateData> Items;
    }
}