namespace KingofCurries.Models
{
    public class GenericSubModel
    {
        public int ProductId { get; set; } = 0;
        public Boolean IsFreeItems  { get; set; }=  false;
        public Boolean IsSubItems  { get; set; }=false;
        public List<FreeItems> ListFreeItems  { get; set; }= new List<FreeItems>(); 
        public List<SubItems> ListSubItems  { get; set; }=new List<SubItems>();

        public Item Item { get; set; }= new Item();
    }
}
