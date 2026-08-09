namespace Adani.Solution.Data.Entities
{
    public class DerivedGroupMargin : Auditable
    {        
        public long BaseGroupMarginId { get; set; }
        public long CustomerGroupId { get; set; }
        public string Formula { get; set; }
        public decimal Margin { get; set; }
        
        public virtual CustomerGroups CustomerGroup { get; set; }
        public virtual BaseGroupMargin BaseGroupMargin { get; set; }         
    }
}
