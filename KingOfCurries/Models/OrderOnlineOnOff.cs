namespace KingofCurries.Models
{
    public class OrderOnlineOnOff
    {
        public int OnlineOrderId { get; set; } = 0;
        public Boolean IsOrderOnline { get; set; } = false;
        public string OrderText { get; set; } = string.Empty;
    }
}
