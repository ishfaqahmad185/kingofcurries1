using API.Utility;
using KingofCurries.Models;
using System.Data.SqlClient;
using System.Data;
using ExtensionMethods;
using HarrysRestro.Services;
using MailKit.Search;
using _Helper;

namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
  
  


        public IEnumerable<OrdersMain> GetAllOrdersSearch(string UserName, string OrderNo,string DeliveryType,DateTime FromDate,DateTime ToDate,int Status)
        {
            List<OrdersMain> listOrders = new List<OrdersMain>();
            SqlParameter[] param =
             {
            new SqlParameter("@Username", UserName),
            new SqlParameter("@OrderNo", OrderNo),
            new SqlParameter("@DeliveryType", DeliveryType),
            new SqlParameter("@FromDate", FromDate),

            new SqlParameter("@ToDate", ToDate),
            new SqlParameter("@StatusId", Status),

                };


            DataTable datatable = SqlHelper.GetTableFromSP("spOrderMain_Search", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                OrdersMain ordersMain = new OrdersMain();
                ordersMain.OrderId = dataRow["OrderId"].ToInt32();
                ordersMain.OrderNo = dataRow["OrderNo"].ToString();
                ordersMain.OrderDate = dataRow["OrderDate"].ToDate();
                ordersMain.UserId = dataRow["UserId"].ToInt32();
                ordersMain.UserName = dataRow["UserName"].ToString();
                ordersMain.Email = dataRow["Email"].ToString();
                ordersMain.Country = dataRow["Country"].ToString();
                ordersMain.County = dataRow["County"].ToString();
                ordersMain.PostalCode = dataRow["PostalCode"].ToString();
                ordersMain.ContactNo = dataRow["ContactNo"].ToString();
                ordersMain.StatusId = dataRow["OrderStatusId"].ToInt32();
                ordersMain.StatusName = dataRow["OrderStatusName"].ToString();
                ordersMain.Remarks = dataRow["Remarks"].ToString();
                ordersMain.Address = dataRow["Address"].ToString();
                ordersMain.Town = dataRow["Town"].ToString();
                ordersMain.PaymentId = dataRow["PaymentId"].ToInt32();
                ordersMain.paymentType = dataRow["paymentType"].ToString();
                ordersMain.DeliveryType = dataRow["DeliveryType"].ToString();
                ordersMain.DeliveryId = dataRow["DeliveryId"].ToInt32();
                ordersMain.TotalAmount = dataRow["TotalAmount"].ToDecimal();
                ordersMain.DeliveryChargesId = dataRow["DeliveryChargesId"].ToInt32();
                ordersMain.DeliveryCharges = dataRow["DeliveryCharges"].ToDecimal();
                ordersMain.GrandTotal = dataRow["GrandTotal"].ToDecimal();
                ordersMain.Systime = ordersMain.OrderDate.ToString("dd MMM HH:mm");
                ordersMain.DeliveryTime = dataRow["DeliveryTime"].ToString();
                




                listOrders.Add(ordersMain);
            }
            return listOrders;

            //return datatable;


        }

        public IEnumerable<OrdersMain> GetAllOrderStatus()
        {
            List<OrdersMain> listOrderMain = new List<OrdersMain>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("sp_OrderStatus_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                OrdersMain ordersMain = new OrdersMain();

                ordersMain.StatusId = dataRow["OrderStatusId"].ToInt32();
                ordersMain.StatusName = dataRow["OrderStatusName"].ToString();
              

                listOrderMain.Add(ordersMain);
            }
            return listOrderMain;

            //return datatable;


        }



        public int InsertInvoiceMain(PlaceOrder placeOrder)
        {

            DataTable datatable = new DataTable();

            SqlParameter[] param =
                  {
                //new SqlParameter("UserId", Users.UserId),
            new SqlParameter("@UserId", placeOrder.UserId),
            new SqlParameter("@Email", placeOrder.Email),
            new SqlParameter("@Country", placeOrder.Country),
            new SqlParameter("@County", placeOrder.County),
            new SqlParameter("@PostalCode", placeOrder.EIR),
            new SqlParameter("@ContactNo", placeOrder.PhoneNo),
            new SqlParameter("@Remarks", placeOrder.Notes),
            new SqlParameter("@Address", placeOrder.Address),
            new SqlParameter("@Town", placeOrder.Town),
            new SqlParameter("@PaymentId", placeOrder.PaymentId),
            new SqlParameter("@paymentType", placeOrder.PaymentType),
            new SqlParameter("@DeliveryType", placeOrder.DeliveryType),
            new SqlParameter("@DeliveryId", placeOrder.DeliveryId),
            new SqlParameter("@TotalAmount", placeOrder.TotalAmount),
            new SqlParameter("@DeliveryChargesId", placeOrder.DeliveryChargesId),
            new SqlParameter("@DeliveryCharges", placeOrder.DeliveryCharges),
            new SqlParameter("@DeliveryTime", placeOrder.DeliveryTime),



                };


            return SqlHelper.ExecuteProcedureReturnString("spOrderMain_Insert", param).ToInt32();


        }

        public bool InsertInvoiceDetail(int invoiceId, CartProducts placeOrder)
        {
            DataTable datatable = new DataTable();

            SqlParameter[] param =
                  {
                //new SqlParameter("UserId", Users.UserId),
            new SqlParameter("@InvoiceId", invoiceId),
            new SqlParameter("@ItemId", placeOrder.ProductId),
            new SqlParameter("@SubItemId", placeOrder.SubProductId),
            new SqlParameter("@FreeItemId", placeOrder.FreeProductId),

            new SqlParameter("@Price", placeOrder.Price),
            new SqlParameter("@Quantity", placeOrder.Quantity),




                };


            return SqlHelper.ExecuteProcedureNonQuery("spOrdersDetail_Insert", param);
        }


        public IEnumerable<OrdersMain> GetAllOrdersMain(int OrderId)
        {
            List<OrdersMain> Balobj = new List<OrdersMain>();
            SqlParameter[] param =
            {
                  new SqlParameter("@OrderId", OrderId),

                };


           DataTable datatable = SqlHelper.GetTableFromSP("spOrderMain_GetByOrderId", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                OrdersMain ordersMain = new OrdersMain();

                ordersMain.StatusId = dataRow["StatusId"].ToInt32();
                ordersMain.OrderId = dataRow["OrderId"].ToInt32();
                ordersMain.DeliveryChargesId = dataRow["DeliveryChargesId"].ToInt32();
                ordersMain.DeliveryId = dataRow["DeliveryId"].ToInt32();
                ordersMain.PaymentId = dataRow["PaymentId"].ToInt32();
                ordersMain.UserId = dataRow["UserId"].ToInt32();
                ordersMain.OrderNo = dataRow["OrderNo"].ToString();
                ordersMain.OrderDate = dataRow["OrderDate"].ToDate();
                ordersMain.Email = dataRow["Email"].ToString();
                ordersMain.Country = dataRow["Country"].ToString();
                ordersMain.County = dataRow["County"].ToString();
                ordersMain.PostalCode = dataRow["PostalCode"].ToString();
                ordersMain.ContactNo = dataRow["ContactNo"].ToString();
                ordersMain.Remarks = dataRow["Remarks"].ToString();
                ordersMain.Address = dataRow["Address"].ToString();
                ordersMain.Town = dataRow["Town"].ToString();
                ordersMain.paymentType = dataRow["paymentType"].ToString();
                ordersMain.DeliveryType = dataRow["DeliveryType"].ToString();
                ordersMain.TotalAmount = dataRow["TotalAmount"].ToDecimal();
                ordersMain.DeliveryCharges = dataRow["DeliveryCharges"].ToDecimal();
                ordersMain.GrandTotal = dataRow["GrandTotal"].ToDecimal();
                ordersMain.UserName = dataRow["UserName"].ToString();
                ordersMain.PraperationDuration = dataRow["PrepareDuration"].ToInt32();
                ordersMain.RemainingTime = dataRow["RemainingTime"].ToInt32();
                ordersMain.OrderStatus = dataRow["OrderStatusName"].ToString();
                ordersMain.Systime = ordersMain.OrderDate.ToString("dd MMM HH:mm");
                ordersMain.DeliveryTime = dataRow["DeliveryTime"].ToString();
                ordersMain.DeliveryLocation = dataRow["DeliveryLocation"].ToString();



                Balobj.Add(ordersMain);
            }
            return Balobj;




        }


        public IEnumerable<OrdersDetail> GetAllOrderDetail(int id)
        {
            List<OrdersDetail> ordersDetails = new List<OrdersDetail>();
            SqlParameter[] param =
           {
          new SqlParameter("@OrderId",id),
               };

            DataTable dataTable = SqlHelper.GetTableFromSP("spOrdersDetail_GetByOrderId", param);

            if (dataTable == null)
            {
                return ordersDetails;
            }

            foreach (DataRow item in dataTable.Rows)
            {
                OrdersDetail detail = new OrdersDetail();

                detail.OrderDetailId = item["OrderDetailId"].ToInt32();
                detail.InvoiceId = item["InvoiceId"].ToInt32();
                detail.ItemId = item["ItemId"].ToInt32();
                detail.FreeItemId = item["FreeItemId"].ToInt32();

                detail.SubItemId = item["SubItemId"].ToInt32();
                detail.Quantity = item["Quantity"].ToInt32();
                detail.Price = item["Price"].ToDecimal();
                detail.TotalPrice = item["TotalPrice"].ToDecimal();
                detail.InvoiceNo = item["OrderNo"].ToString();
                detail.ItemName = item["ItemTitle"].ToString();
                detail.SubName = item["SubItemTitle"].ToString();
                detail.ItemImage = item["ItemImage"].ToString();
                detail.FreeName = item["FreeItemTitle"].ToString();
                detail.CreatedAt = item["CreatedAt"].ToDate();

                ordersDetails.Add(detail);

            }

            return ordersDetails;

        }

        public Boolean OrdersConfirm(int id, int statusId)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@OrderId",id),
          new SqlParameter("@StatusId",statusId),


       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_Orders_Confirm", param);




            return data;


        }


        public Boolean ChangeOrderTime(int id, int orderTime, int OrderType)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@OrderId",id),
          new SqlParameter("@OrderTime",orderTime),
          new SqlParameter("@OrderType",OrderType),


       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spOrderMain_ChangeOrderTime", param);




            return data;


        }


        public Boolean OrderMainOrderConfirmation(int id, int statusId, string duration)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@OrderId",id),
          new SqlParameter("@StatusId",statusId),
          new SqlParameter("@Duration",duration),


       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("spOrderMain_OrderConfirmation", param);




            return data;


        }

        public Boolean InvoiceMainGetNewOrder()
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {



       };


            var data = SqlHelper.ExecuteProcedureReturnString("spInvoiceMain_GetNewOrder", param);




            return data.ToBool();


        }

        public IEnumerable<OrdersMain> GetAllOrders()
        {
            return GetAllOrdersMain(-1);
        }
    }
}
