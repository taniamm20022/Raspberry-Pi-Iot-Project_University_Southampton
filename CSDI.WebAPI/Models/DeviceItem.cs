using System;

namespace CSDI.WebAPI.Models
{
  public class DeviceItem
  {
    public int DeviceId { get; set; }
    public string Name { get; set; }
    public string IP { get; set; }
    public string MPN { get; set; }
    public int SelectedDeviceTypeId { get; set; }
    public string TypeName { get; set; }
    public string SelectedUserId { get; set; }
    public string UserName { get; set; }
    public bool Active { get; set; }
    public DateTime Date { get; internal set; }
  }
}