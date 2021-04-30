using CSDI.WebAPIClient.DataModels;

namespace CSDI.WebAPIClient
{
  public class DiagnosticsResponse : ApiResponse<ListingPageModel<DiagnosticItem>>
  {
  }
  public class DiagnosticResponse : ApiResponse<DiagnosticItem>
  {
  }
}