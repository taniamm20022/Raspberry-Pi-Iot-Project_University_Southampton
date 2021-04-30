using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CSDI.WebAPIClient;
using LupenM.WebSite.Attributes;
using LupenM.WebSite.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;




namespace LupenM.WebSite.Controllers
{
  public class ReportsController : Controller
  {
    IHttpClientFactory httpClientFactory;
    private readonly TokenContainer tokenContainer;

    private readonly IDeviceClient deviceClient;
    private readonly IIndicationsClient indicationsClient;
    private readonly IEmergencySituationsClient emergencySituationsClient;

    public ReportsController()
    {
      httpClientFactory = new HttpClientFactory();
      this.tokenContainer = new TokenContainer();

      var apiClient = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      deviceClient = new DeviceClient(apiClient);

      var apiClient2 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      indicationsClient = new IndicationsClient(apiClient2);

      var apiClient3 = new ApiClient(httpClientFactory.CreateClient(), tokenContainer);
      emergencySituationsClient = new EmergencySituationsClient(apiClient3);
    }

    [AuthenticationAttributeReports]
    public async Task<ActionResult> Indications(IndicationsModel model)
    {
      if (model.PageSize == 0)
      {
        model.PageOrder = 1;
        model.CurrentPage = 1;
        model.PageSize = 15;
      }

      if ((int?)Session["DeviceID"] != model.SelectedDeviceId ||
          (DateTime?)Session["DateFrom"] != model.DateFrom ||
          (DateTime?)Session["DateTo"] != model.DateTo)
      {
        model.CurrentPage = 1;
      }

      Session["DeviceID"] = model.SelectedDeviceId;
      Session["DateFrom"] = model.DateFrom;
      Session["DateTo"] = model.DateTo;
      Session["CurrentPage"] = model.CurrentPage;

      int deciveID;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveID);

      var devices = await deviceClient.GetDevices();
      var indications = await indicationsClient.GetIndications(model.DateFrom, model.DateTo, deciveID, model.Position, model.PageSize);

      model.ListDevices = devices.Data;
      model.Indications = indications.Data.ListItems;
      model.TotalRecords = indications.Data.TotalRecords;
      model.ItemsCount = indications.Data.ListItems.Count();

      return View(model);
    }

    [AuthenticationAttributeReports]
    public async Task<ActionResult> EmergencySituations(EmergencySituationsModel model)
    {
      if (model.PageSize == 0)
      {
        model.PageOrder = 1;
        model.CurrentPage = 1;
        model.PageSize = 15;
      }

      if ((int?)Session["DeviceID"] != model.SelectedDeviceId ||
          (DateTime?)Session["DateFrom"] != model.DateFrom ||
          (DateTime?)Session["DateTo"] != model.DateTo)
      {
        model.CurrentPage = 1;
      }

      Session["DeviceID"] = model.SelectedDeviceId;
      Session["DateFrom"] = model.DateFrom;
      Session["DateTo"] = model.DateTo;

      int deciveID;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveID);

      var devices = await deviceClient.GetDevices();
      var emergencySituations = await emergencySituationsClient.GetEmergencySituations(model.DateFrom, model.DateTo, deciveID, model.Position, model.PageSize);

      model.ListDevices = devices.Data;
      model.EmergencySituations = emergencySituations.Data.ListItems;
      model.TotalRecords = emergencySituations.Data.TotalRecords;
      model.ItemsCount = emergencySituations.Data.ListItems.Count();

      return View(model);
    }

    #region Export Signals (Indications)

    public async Task<FileContentResult> ExportCsvIndications(int? rowToExport, IndicationsModel model)
    {
      int deciveId;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

      var indications = await indicationsClient.GetIndications(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

      StringBuilder sb = new StringBuilder();
      sb.Append(String.Format("Date {0}; Device name {1}; IP {2}; Sensor name {3}; Value {4}; Unit {5}\n", rowToExport, rowToExport, rowToExport, rowToExport, rowToExport, rowToExport));

      foreach (var item in indications.Data.ListItems)
      {
        sb.AppendFormat("{0};{1};{2};{3};{4};{5}\r\n", Helpers.StringUtils.FormatDate(item.Date), item.DeviceName, item.IP, item.SensorName, item.Value, item.UnitName);
      }

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1251), Encoding.UTF8.GetBytes(sb.ToString()));

      return new FileContentResult(ba, "application/octet-stream")
      {
        FileDownloadName = "export_signals_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".csv"
      };
    }

    public async Task<FileContentResult> ExportExcelIndications(IndicationsModel model)
    {
      MemoryStream stream = new MemoryStream();

      using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
      {
        WorkbookPart workbookPart = document.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new Worksheet();

        // Adding style
        WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
        stylePart.Stylesheet = GenerateStylesheet();
        stylePart.Stylesheet.Save();

        // Setting up columns
        Columns columns = new Columns(
                new Column // Date column
                {
                  Min = 1,
                  Max = 1,
                  Width = 20,
                  CustomWidth = true
                },
                new Column // Device name, IP, Sensor name columns
                {
                  Min = 2,
                  Max = 4,
                  Width = 25,
                  CustomWidth = true
                },
                new Column // Value, Unit columns
                {
                  Min = 5,
                  Max = 6,
                  Width = 10,
                  CustomWidth = true
                });

        worksheetPart.Worksheet.AppendChild(columns);

        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Indications" };

        sheets.Append(sheet);

        workbookPart.Workbook.Save();

        int deciveId;
        var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

        var indications = await indicationsClient.GetIndications(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

        SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

        //Constructing header
        Row row = new Row();

        row.Append(
            ConstructCell("Date", CellValues.String, 2),
            ConstructCell("Device name", CellValues.String, 2),
            ConstructCell("IP", CellValues.String, 2),
            ConstructCell("Sensor name", CellValues.String, 2),
            ConstructCell("Value", CellValues.String, 2),
            ConstructCell("Unit", CellValues.String, 2));

        //Insert the header row to the Sheet Data
        sheetData.AppendChild(row);

        //Inserting each indications
        foreach (var item in indications.Data.ListItems)
        {
          row = new Row();

          row.Append(
              ConstructCell(item.Date.ToString("dd.MM.yyyy HH:mm:ss"), CellValues.String, 0),
              ConstructCell(item.DeviceName.ToString(), CellValues.String, 0),
              ConstructCell(item.IP.ToString(), CellValues.String, 0),
              ConstructCell(item.SensorName.ToString(), CellValues.String, 0),
              ConstructCell(item.Value.ToString(), CellValues.String, 3),
              ConstructCell(item.UnitName.ToString(), CellValues.String, 0));

          sheetData.AppendChild(row);
        }

        worksheetPart.Worksheet.Save();
      }
      
      Response.ContentType = "application/force-download";
      Response.AddHeader("content-disposition", "attachment;filename=" + "export_signals_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".xlsx");
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.BinaryWrite(stream.ToArray());
      Response.End();

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1251), Encoding.UTF8.GetBytes(stream.ToString()));

      return new FileContentResult(ba, "application/force-download");
    }

    public async Task<FileContentResult> ExportHtmlIndications(IndicationsModel model)
    {
      int deciveId;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

      var indications = await indicationsClient.GetIndications(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

      StringBuilder sb = new StringBuilder();
      sb.Append("<html>");
      sb.Append("<head>");
      sb.Append("<meta charset='UTF-8'>");
      sb.Append("</head>");
      sb.Append("<body>");
      sb.Append("<table border='1px' cellpadding='2' cellspacing='0'>");

      sb.Append("<tr style='color: #fff; background-color: #337ab7;'>");
      sb.Append("<td>");
      sb.Append("Date");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Device name");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("IP");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Sensor name");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Value");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Unit");
      sb.Append("</td>");
      sb.Append("</tr>");

      foreach (var item in indications.Data.ListItems)
      {
        sb.Append("<tr>");
        sb.Append("<td>");
        sb.Append(Helpers.StringUtils.FormatDate(item.Date));
        sb.Append("</td>");

        sb.Append("<td>");
        sb.Append(item.DeviceName);
        sb.Append("</td>");

        sb.Append("<td>");
        sb.Append(item.IP);
        sb.Append("</td>");

        sb.Append("<td>");
        sb.Append(item.SensorName);
        sb.Append("</td>");

        sb.Append("<td style='text-align: right;'>");
        sb.Append(item.Value);
        sb.Append("</td>");

        sb.Append("<td>");
        sb.Append(item.UnitName);
        sb.Append("</td>");
        sb.Append("</tr>");
      }

      sb.Append("</table>");
      sb.Append("</body>");
      sb.Append("</html>");

      string HtmlText = sb.ToString();

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sb.ToString()));

      return new FileContentResult(ba, "text/html")
      {
        FileDownloadName = "export_signals_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".html"
      };
    }

    public async Task<FileContentResult> ExportPdfIndications(IndicationsModel model)
    {
      int deciveId;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

      var indications = await indicationsClient.GetIndications(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

      Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
      PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

      pdfDoc.Open();

      Paragraph p = new Paragraph("Indications");
      p.Alignment = Element.ALIGN_CENTER;
      p.Font.Size = 20;
      pdfDoc.Add(p);
      pdfDoc.Add(new Phrase("\n"));

      PdfPTable table = new PdfPTable(6);

      AddHeaderIndications(table);

      for (int i = 0; i < indications.Data.TotalRecords; i++)
      {
        table.AddCell(StyleCell(Helpers.StringUtils.FormatDate(indications.Data.ListItems[i].Date), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(indications.Data.ListItems[i].DeviceName.ToString(), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(indications.Data.ListItems[i].IP.ToString(), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(indications.Data.ListItems[i].SensorName.ToString(), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(indications.Data.ListItems[i].Value.ToString(), Element.ALIGN_RIGHT));
        table.AddCell(StyleCell(indications.Data.ListItems[i].UnitName.ToString(), Element.ALIGN_LEFT));
      }

      pdfDoc.Add(table);
      pdfDoc.Close();
      
      Response.ContentType = "application/pdf";
      Response.AddHeader("content-disposition", "attachment;filename=" + "export_signals_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".pdf");
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.Write(pdfDoc);
      Response.End();

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1251), Encoding.UTF8.GetBytes(pdfDoc.ToString()));

      return new FileContentResult(ba, "application/pdf");
    }

    private static void AddHeaderIndications(PdfPTable table)
    {
      float regular = 120;

      table.SetTotalWidth(new float[6] { 110, regular, 100, regular, 60, 50 });
      table.WidthPercentage = 100;

      table.AddCell(StyleCell("Date", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Device name", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("IP", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Sensor name", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Value", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Unit", Element.ALIGN_LEFT, Colors.Blue, Colors.White));

      table.HeaderRows = 1;
    }

    #endregion

    #region Export Emergency Situations

    public async Task<FileContentResult> ExportCsvEmergencySituations(int? rowToExport, EmergencySituationsModel model)
    {
      int deciveId;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

      var emergencySituations = await emergencySituationsClient.GetEmergencySituations(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

      StringBuilder sb = new StringBuilder();
      sb.Append(String.Format("Date {0}; Device name {1}; Sensor name {2}; Min value {3}; Value {4}; Unit {5}; Max value {6}\n", rowToExport, rowToExport, rowToExport, rowToExport, rowToExport, rowToExport, rowToExport));

      foreach (var item in emergencySituations.Data.ListItems)
      {
        sb.AppendFormat("{0};{1};{2};{3};{4};{5};{6}\r\n", Helpers.StringUtils.FormatDate(item.Date), item.DeviceName, item.SensorName, item.MinValue, item.Value, item.UnitName, item.MaxValue);
      }

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1251), Encoding.UTF8.GetBytes(sb.ToString()));

      return new FileContentResult(ba, "application/octet-stream")
      {
        FileDownloadName = "export_emergencySituations_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".csv"
      };
    }

    public async Task<FileContentResult> ExportExcelEmergencySituations(EmergencySituationsModel model)
    {
      MemoryStream stream = new MemoryStream();

      using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
      {
        WorkbookPart workbookPart = document.AddWorkbookPart();
        workbookPart.Workbook = new Workbook();

        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new Worksheet();

        // Adding style
        WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
        stylePart.Stylesheet = GenerateStylesheet();
        stylePart.Stylesheet.Save();

        // Setting up columns
        Columns columns = new Columns(
                new Column // Date column
                {
                  Min = 1,
                  Max = 1,
                  Width = 20,
                  CustomWidth = true
                },
                new Column // Device name, Sensor name columns
                {
                  Min = 2,
                  Max = 3,
                  Width = 25,
                  CustomWidth = true
                },
                new Column // Min value, Value, Unit, Max value columns
                {
                  Min = 4,
                  Max = 7,
                  Width = 10,
                  CustomWidth = true
                });

        worksheetPart.Worksheet.AppendChild(columns);

        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Indications" };

        sheets.Append(sheet);

        workbookPart.Workbook.Save();

        int deciveId;
        var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

        var emergencySituations = await emergencySituationsClient.GetEmergencySituations(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

        SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

        //Constructing header
        Row row = new Row();

        row.Append(
            ConstructCell("Date", CellValues.String, 2),
            ConstructCell("Device name", CellValues.String, 2),
            ConstructCell("Sensor name", CellValues.String, 2),
            ConstructCell("Min value", CellValues.String, 2),
            ConstructCell("Value", CellValues.String, 2),
            ConstructCell("Unit", CellValues.String, 2),
            ConstructCell("Max value", CellValues.String, 2));

        //Insert the header row to the Sheet Data
        sheetData.AppendChild(row);

        //Inserting each indications
        foreach (var item in emergencySituations.Data.ListItems)
        {
          row = new Row();

          row.Append(
              ConstructCell(item.Date.ToString("dd.MM.yyyy HH:mm:ss"), CellValues.String, 0),
              ConstructCell(item.DeviceName.ToString(), CellValues.String, 0),
              ConstructCell(item.SensorName.ToString(), CellValues.String, 0),
              ConstructCell(item.MinValue.ToString(), CellValues.String, 3),
              ConstructCell(item.Value.ToString(), CellValues.String, 4),
              ConstructCell(item.UnitName.ToString(), CellValues.String, 0),
              ConstructCell(item.MaxValue.ToString(), CellValues.String, 3));

          sheetData.AppendChild(row);
        }

        worksheetPart.Worksheet.Save();
      }

      Response.ContentType = "application/force-download";
      Response.AddHeader("content-disposition", "attachment;filename=" + "export_emergencySituations_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".xlsx");
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.BinaryWrite(stream.ToArray());
      Response.End();

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1251), Encoding.UTF8.GetBytes(stream.ToString()));

      return new FileContentResult(ba, "application/force-download");
    }

    public async Task<FileContentResult> ExportHtmlEmergencySituations(EmergencySituationsModel model)
    {
      int deciveId;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

      var emergencySituations = await emergencySituationsClient.GetEmergencySituations(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

      StringBuilder sb = new StringBuilder();
      sb.Append("<html >");
      sb.Append("<head>");
      sb.Append("<meta charset='UTF-8'>");
      sb.Append("</head>");
      sb.Append("<body>");
      sb.Append("<table border='1px' cellpadding='2' cellspacing='0'>");

      sb.Append("<tr style='color: #fff; background-color: #337ab7;'>");
      sb.Append("<td>");
      sb.Append("Date");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Device name");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Sensor name");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Min value");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Value");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Unit");
      sb.Append("</td>");

      sb.Append("<td>");
      sb.Append("Max value");
      sb.Append("</td>");
      sb.Append("</tr>");

      foreach (var item in emergencySituations.Data.ListItems)
      {
        sb.Append("<tr>");
        sb.Append("<td>");
        sb.Append(Helpers.StringUtils.FormatDate(item.Date));
        sb.Append("</td>");

        sb.Append("<td>");
        sb.Append(item.DeviceName);
        sb.Append("</td>");

        sb.Append("<td>");
        sb.Append(item.SensorName);
        sb.Append("</td>");

        sb.Append("<td style='text-align: right;'>");
        sb.Append(item.MinValue);
        sb.Append("</td>");

        sb.Append("<td style='text-align:right; color:red;'>");
        sb.Append(item.Value);
        sb.Append("</td>");

        sb.Append("<td>");
        sb.Append(item.UnitName);
        sb.Append("</td>");

        sb.Append("<td style='text-align: right;'>");
        sb.Append(item.MaxValue);
        sb.Append("</td>");
        sb.Append("</tr>");
      }

      sb.Append("</table>");
      sb.Append("</body>");
      sb.Append("</html>");

      string HtmlText = sb.ToString();

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.UTF8, Encoding.UTF8.GetBytes(sb.ToString()));

      return new FileContentResult(ba, "text/html")
      {
        FileDownloadName = "export_emergencySituations_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".html"
      };
    }

    public async Task<FileContentResult> ExportPdfEmergencySituations(EmergencySituationsModel model)
    {
      int deciveId;
      var tryParse = Int32.TryParse(model.SelectedDeviceId.ToString(), out deciveId);

      var emergencySituations = await emergencySituationsClient.GetEmergencySituations(model.DateFrom, model.DateTo, deciveId, model.Position, 1000000);

      Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
      PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

      pdfDoc.Open();

      Paragraph p = new Paragraph("Emergency Situations");
      p.Alignment = Element.ALIGN_CENTER;
      p.Font.Size = 20;
      pdfDoc.Add(p);
      pdfDoc.Add(new Phrase("\n"));

      PdfPTable table = new PdfPTable(7);

      AddHeaderEmergencySituations(table);

      //CultureInfo ci = new CultureInfo("bg-BG");

      for (int i = 0; i < emergencySituations.Data.TotalRecords; i++)
      {
        table.AddCell(StyleCell(Helpers.StringUtils.FormatDate(emergencySituations.Data.ListItems[i].Date), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(emergencySituations.Data.ListItems[i].DeviceName.ToString(), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(emergencySituations.Data.ListItems[i].SensorName.ToString(), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(emergencySituations.Data.ListItems[i].MinValue.ToString(), Element.ALIGN_RIGHT));
        table.AddCell(StyleCell(emergencySituations.Data.ListItems[i].Value.ToString(), Element.ALIGN_RIGHT, Colors.White, Colors.Red));
        table.AddCell(StyleCell(emergencySituations.Data.ListItems[i].UnitName.ToString(), Element.ALIGN_LEFT));
        table.AddCell(StyleCell(emergencySituations.Data.ListItems[i].MaxValue.ToString(), Element.ALIGN_RIGHT));
        //table.AddCell(new PdfPCell(new Phrase(emergencySituations.Data.ListItems[i].MaxValue.ToString())));
      }

      pdfDoc.Add(table);
      pdfDoc.Close();

      Response.ContentType = "application/pdf";
      Response.AddHeader("content-disposition", "attachment;filename=" + "export_emergencySituations_" + Helpers.StringUtils.FormatDateExport(DateTime.Now) + ".pdf");
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.Write(pdfDoc);
      Response.End();

      byte[] ba = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(1251), Encoding.UTF8.GetBytes(pdfDoc.ToString()));

      return new FileContentResult(ba, "application/pdf");
    }

    private static void AddHeaderEmergencySituations(PdfPTable table)
    {
      float regular = 52;

      table.SetTotalWidth(new float[7] { 100, 120, 110, regular, regular, regular, regular });
      table.WidthPercentage = 100;

      table.AddCell(StyleCell("Date", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Device name", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Sensor name", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Min value", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Value", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Unit", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      table.AddCell(StyleCell("Max value", Element.ALIGN_LEFT, Colors.Blue, Colors.White));
      //table.AddCell(new PdfPCell(new Phrase("Max value")));

      table.HeaderRows = 1;
    }

    #endregion

    #region Excel Helpers

    private Stylesheet GenerateStylesheet()
    {
      Stylesheet styleSheet = null;

      Fonts fonts = new Fonts(
              new DocumentFormat.OpenXml.Spreadsheet.Font(), // Index 0 - default
              new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 1 - header
                  new Bold(),
                  new Color() { Rgb = "FFFFFF" }),
              new DocumentFormat.OpenXml.Spreadsheet.Font(), // Index 2
              new DocumentFormat.OpenXml.Spreadsheet.Font(), // Index 3
              new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 4 - body /FontId = 4 cell "Value" font red/
                  new Color() { Rgb = "FF0000" })
          );

      Fills fills = new Fills(
              new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
              new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
              new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "3366cc" } })
              { PatternType = PatternValues.Solid }) // Index 2 - header
          );

      Borders borders = new Borders(
              new Border(), // index 0 default
              new Border( // index 1 black border
                  new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                  new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                  new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                  new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                  new DiagonalBorder()),
              new Border(new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }) // index 2 header
          );

      CellFormats cellFormats = new CellFormats(
              new CellFormat(), // default
              new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
              new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true }, // header
              new CellFormat { Alignment = new Alignment { Horizontal = HorizontalAlignmentValues.Right } }, // Index 3 - cell "Value" alignment
              new CellFormat { Alignment = new Alignment { Horizontal = HorizontalAlignmentValues.Right }, FontId = 4 } // Index 4 - cell "Value" alignment and font red
          );

      styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

      return styleSheet;
    }

    private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
    {
      return new Cell()
      {
        CellValue = new CellValue(value),
        DataType = new EnumValue<CellValues>(dataType),
        StyleIndex = styleIndex
      };
    }

    private Cell ConstructNumberCell(string value, CellValues dataType)
    {
      return new Cell()
      {
        CellValue = new CellValue(value),
        DataType = new EnumValue<CellValues>(dataType)
      };
    }

    public static decimal? ConvertDecimal(string value)
    {
      value = value.Replace(',', '.');
      decimal outValue;

      if (!decimal.TryParse(value, NumberStyles.Any, new NumberFormatInfo() { NumberDecimalSeparator = "." }, out outValue))
      {
        return (Nullable<decimal>)null;
      }

      return outValue;
    }

    #endregion

    #region Pdf Helpers

    public class Colors
    {
      public static iTextSharp.text.BaseColor Blue = new BaseColor(51, 122, 183);
      public static iTextSharp.text.BaseColor White = new BaseColor(255, 255, 255);
      public static iTextSharp.text.BaseColor Red = new BaseColor(255, 0, 0);
    }

    private static PdfPCell StyleCell(string text, int? alignment, BaseColor color)
    {
      return StyleCell(text, null, alignment, null, color, null, null);
    }

    private static PdfPCell StyleCell(string text, int? alignment, BaseColor color, BaseColor fontColor)
    {
      return StyleCell(text, null, alignment, null, color, null, fontColor);
    }

    private static PdfPCell StyleCell(string text, int? alignment)
    {
      return StyleCell(text, null, alignment, null, null, null, null);
    }

    private static PdfPCell StyleCell(string text, int? rotation, int? alignment, iTextSharp.text.Font font,
  iTextSharp.text.BaseColor color, int? vAlignment, BaseColor fontColor)
    {
      Paragraph p = new Paragraph(text);
      p.Alignment = alignment.HasValue ? alignment.Value : Element.ALIGN_CENTER;
      p.Font.Size = 10;
      if (fontColor != null) p.Font.Color = fontColor;

      var cell = new PdfPCell();
      cell.AddElement(p);
      cell.UseAscender = true;
      cell.Padding = 5;

      if (rotation.HasValue) cell.Rotation = rotation.Value;
      if (color != null) cell.BackgroundColor = color;
      if (vAlignment.HasValue) cell.VerticalAlignment = vAlignment.Value;
      else cell.VerticalAlignment = Element.ALIGN_MIDDLE;

      return cell;
    }

    #endregion

    public new RedirectToRouteResult RedirectToAction(string action, string controller)
    {
      return base.RedirectToAction(action, controller);
    }
  }
}