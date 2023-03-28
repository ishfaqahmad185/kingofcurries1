namespace KingofCurries.Models
{
	public class ClientDashBoard
	{
		public int TotalOrders { get; set; } = 0;
		public int CollectionOrders { get; set; } = 0;
		public int DeliveryOrders { get; set; } = 0;
		public decimal TotalPayments { get; set; } = 0;
		public decimal DeliveryPayments { get; set; } = 0;
		public decimal Collectionpayments { get; set; } = 0;
		public decimal DeliveryCharges { get; set; } = 0;
		public decimal Cardpayments { get; set; } = 0;
		public int TotalCard { get; set; } = 0;
		public decimal Cashpayments { get; set; } = 0;
		public int TotalCash { get; set; } = 0;
		public int NewOrders { get; set; } = 0;
		public int CancelOrders { get; set; } = 0;
		public int TotalReservation { get; set; } = 0;
		public int ConfrimReservation { get; set; } = 0;
		public int CancelReservation { get; set; } = 0;
		public int NewReservation { get; set; } = 0;

		public List<int> counts { get; set; } =new List<int>();
		public int TotalProducts { get; set; } = 0;
		public List<string> keys { get; set; }  = new List<string>();
		public List<Top2Data> TopProducts { get; set; }  = new List<Top2Data>();
		public List<Top2Data> TopLocation { get; set; }  = new List<Top2Data>();

		public int NewReservationPercentage
		{
			get
			{
				if(TotalReservation> 0)
				{
					return (int)Math.Round((double)((NewReservation * 100) / TotalReservation));
				}
				else
				{
					return 0;
				}
				
			}
		}

		public int ConfrimReservationPercentage
		{
			get
			{
				if(TotalReservation> 0) {
				return (int)Math.Round((double)((ConfrimReservation * 100) / TotalReservation));
				}
				else
				{
					return 0;
				}
			}
		}

		public int CancelReservationPercentage
		{
			get
			{
				if (TotalReservation > 0)
				{
					return (int)Math.Round((double)((CancelReservation * 100) / TotalReservation));
				}
				else
				{
					return 0;
				}
			}
		}

		public int Totaldata
		{
			get
			{

				return DeliveryOrders + CollectionOrders + CancelOrders;
			}
		}

		public int DeliveryPercentage
		{
			get
			{
				if(Totaldata> 0) { 
				return (int)Math.Round((double)((DeliveryOrders * 100) / Totaldata));
			
			}
				else
				{
				return 0;
			}
		}
		}

		public int CollectionPercentage
		{
			get
			{
				if (Totaldata > 0)
				{
					return (int)Math.Round((double)((CollectionOrders * 100) / Totaldata));
			}
				else
				{
				return 0;
			}
		}
		}

		public int CancelledPercentage
		{
			get
			{
				if (Totaldata > 0)
				{
					return (int)Math.Round((double)((CancelOrders * 100) / Totaldata));
				}
				else
				{
					return 0;
				}
			}
		}

		



	}

	public class Top2Data
	{
		public int Count { get; set; } = 0;
		public string Key { get; set; } = string.Empty;


	}
}
