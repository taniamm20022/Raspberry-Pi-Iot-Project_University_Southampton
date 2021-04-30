using CSDI.WebAPIClient.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace LupenM.WebSite.Models
{
  public class CreateSensorModel
  {
    public CreateSensorModel()
    {
      this.ListDevices = new List<DeviceItem>();
      this.ListUnits = new List<UnitItem>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "The 'Sensor name' field is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The 'Device name' field is required.")]
    public int SelectedDeviceId { get; set; }
    public List<DeviceItem> ListDevices { get; set; }

    public string MinValue { get; set; }
    public string MaxValue { get; set; }

    [Required(ErrorMessage = "The 'Topic' field is required.")]
    public string Topic { get; set; }

    [Required(ErrorMessage = "The 'Indication interval' field is required.")]
    [DisplayFormat(DataFormatString = @"{0:hh\:mm\:ss}", ApplyFormatInEditMode = true)]
    [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Indication interval must be hh:mm:ss")]
    public TimeSpan? IndicationInterval { get; set; }

    [Required(ErrorMessage = "The 'Unit' field is required.")]
    public int SelectedUnitId { get; set; }
    public List<UnitItem> ListUnits { get; set; }

    public bool Active { get; set; }
    public DateTime Date { get; internal set; }
    public string Location { get; set; }
  }
}