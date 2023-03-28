using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;
using Newtonsoft.Json.Linq;
using KingofCurries.Models;

namespace KingofCurries.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IOrderRepository _orderRepository;
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;
        public ReportController(IWebHostEnvironment webHostEnvironment, IOrderRepository orderRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            return View();
        }




        [HttpGet]
        [Route("/PrintPreview/{id}")]
        public IActionResult PrintPreview(int id,int type)
        {
            GenericOrder model = new GenericOrder();
            try
            {
                model.OrdersMain = _orderRepository.GetAllOrdersMain(id).FirstOrDefault();
                model.ListOrderDetail = _orderRepository.GetAllOrderDetail(id).ToList();

                return View("/Views/Clients/PrintPreview.cshtml", model);
            }
            catch (Exception exp)
            {
                return View("/Views/Clients/PrintPreview.cshtml", model);

            }
        }

        //public void PrintReceipt(int id,int type)
        //{
        //    var dt = new DataTable();
        //    using var report = new LocalReport();

            
        //    dt = GetItemList(orderDetail);
        //    ReportDataSource rds = new ReportDataSource();
        //    rds.Name = "dsItem";
        //    rds.Value = dt;
        //    report.DataSources.Clear();
        //    report.DataSources.Add(rds);
            
        //    report.ReportPath = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report.rdlc";

        //    report.Refresh();
        //    ReportParameter DeliveryType = new ReportParameter("DeliveryType", orderMain.DeliveryType);
        //    report.SetParameters(DeliveryType);
        //    ReportParameter PickingTime = new ReportParameter("PickingTime", orderMain.DeliveryTime);
        //    report.SetParameters(PickingTime);
        //    ReportParameter OrderPlacedDate = new ReportParameter("OrderPlacedDate", orderMain.Systime);
        //    report.SetParameters(OrderPlacedDate);
        //    ReportParameter InvoiceNo = new ReportParameter("InvoiceNo", orderMain.OrderNo);
        //    report.SetParameters(InvoiceNo);
        //    ReportParameter ResNotes = new ReportParameter("ResNotes", orderMain.Remarks);
        //    report.SetParameters(ResNotes);
        //    ReportParameter SubTotal = new ReportParameter("SubTotal", "€" + orderMain.TotalAmount.ToString());
        //    report.SetParameters(SubTotal);
        //    ReportParameter DeliveryCharges = new ReportParameter("DeliveryCharges", "€" + orderMain.DeliveryCharges.ToString());
        //    report.SetParameters(DeliveryCharges);
        //    ReportParameter GrandTotal = new ReportParameter("GrandTotal", "€" + orderMain.GrandTotal.ToString());
        //    report.SetParameters(GrandTotal);
        //    ReportParameter Name = new ReportParameter("Name",  orderMain.UserName.ToString());
        //    report.SetParameters(Name);
        //    ReportParameter Location = new ReportParameter("Location", "Location: " + orderMain.DeliveryLocation.ToString());
        //    report.SetParameters(Location);
        //    ReportParameter EirCode = new ReportParameter("EirCode", "Eir Code: " + orderMain.PostalCode.ToString());
        //    report.SetParameters(EirCode); 
        //    ReportParameter Payment = new ReportParameter("Payment",  orderMain.paymentType.ToString());
        //    report.SetParameters(Payment);
        //    ReportParameter ContactNo = new ReportParameter("ContactNo", orderMain.ContactNo.ToString());
        //    report.SetParameters(ContactNo);


        //    if (orderDetail.Count > 0)
        //    {
        //        string pageSize = "8.2in";
        //        if(orderDetail.Count == 1) {

        //        }
        //        else
        //        {
        //            var data = orderDetail.Count * 0.6;
        //            data = data + 7.7;

        //            pageSize = $"{Math.Round(data,2)}in";
        //        }

        //        if (type == 2)
        //        {
        //            PrintToPrinter(report, pageSize);
        //            PrintToPrinter(report, pageSize);
        //        }
        //        else
        //        {
        //            PrintToPrinter(report, pageSize);
        //        }

             
        //    }


         


        //}


        //public DataTable GetItemList(List<OrdersDetail> ordersDetails)
        //{
        //    var list = new DataTable();
        //    list.Columns.Add("ItemId");
        //    list.Columns.Add("ItemName");
        //    list.Columns.Add("ItemQty");
        //    list.Columns.Add("Price");
        //    list.Columns.Add("SubName");
        //    list.Columns.Add("FreeName");
        //    DataRow row;

        //    foreach (var item in ordersDetails)
        //    {
        //        row = list.NewRow();
        //        row["ItemId"] = item.ItemId;
        //        row["ItemName"] = item.ItemName;
        //        row["ItemQty"] = item.Quantity + " x ";
        //        row["Price"] = item.TotalPrice;
        //        row["SubName"] = item.SubName;
        //        row["FreeName"] = item.FreeName;


        //        list.Rows.Add(row);

        //    }

        //    return list;
        //}


        //public static void PrintToPrinter(LocalReport report, string pageHeight)
        //{
        //    Export(report, pageHeight,true);
        //}

        //public static void Export(LocalReport report, string pageHeight, bool print = true)
        //{
        //    string deviceInfo =
        //     @$"<DeviceInfo>
        //        <OutputFormat>EMF</OutputFormat>
        //        <PageWidth>3.5in</PageWidth>
        //        <PageHeight>{pageHeight}</PageHeight>
        //        <MarginTop>0in</MarginTop>
        //        <MarginLeft>0.1in</MarginLeft>
        //        <MarginRight>0.1in</MarginRight>
        //        <MarginBottom>0in</MarginBottom>
        //    </DeviceInfo>";
        //    Warning[] warnings;
        //    m_streams = new List<Stream>();
        //    report.Render("Image", deviceInfo, CreateStream, out warnings);
        //    foreach (Stream stream in m_streams)
        //        stream.Position = 0;

        //    if (print)
        //    {
        //        Print();
              
        //    }
        //}
        //public static void Print()
        //{
        //    if (m_streams == null || m_streams.Count == 0)
        //        throw new Exception("Error: no stream to print.");
        //    PrintDocument printDoc = new PrintDocument();
        //    if (!printDoc.PrinterSettings.IsValid)
        //    {
        //        throw new Exception("Error: cannot find the default printer.");
        //    }
        //    else
        //    {
        //        printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
        //        m_currentPageIndex = 0;
        //        printDoc.Print();
        //    }
        //}
        //public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        //{
        //    Stream stream = new MemoryStream();
        //    m_streams.Add(stream);
        //    return stream;
        //}
        //public static void PrintPage(object sender, PrintPageEventArgs ev)
        //{
        //    Metafile pageImage = new
        //       Metafile(m_streams[m_currentPageIndex]);

        //    // Adjust rectangular area with printer margins.
        //    Rectangle adjustedRect = new Rectangle(
        //        ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
        //        ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
        //        ev.PageBounds.Width,
        //        ev.PageBounds.Height);

        //    // Draw a white background for the report
        //    ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

        //    // Draw the report content
        //    ev.Graphics.DrawImage(pageImage, adjustedRect);

        //    // Prepare for the next page. Make sure we haven't hit the end.
        //    m_currentPageIndex++;
        //    ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        //}
        //public static void DisposePrint()
        //{
        //    if (m_streams != null)
        //    {
        //        foreach (Stream stream in m_streams)
        //            stream.Close();
        //        m_streams = null;
        //    }
        //}

		

	}
}
