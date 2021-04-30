using CSDI.WebAPIClient.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace LupenM.WebSite.Models
{
  public class CreateDeviceModel
  {
    public CreateDeviceModel()
    {
      this.ListDeviceTypes = new List<TypeItem>();
    }

    public int Id { get; set; }

    [Required(ErrorMessage = "The 'Device name' field is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The 'IP' field is required.")]
    public string IP { get; set; }

    public string MPN { get; set; }

    [Required(ErrorMessage = "The 'Device type' field is required.")]
    public int SelectedDeviceTypeId { get; set; }
    public List<TypeItem> ListDeviceTypes { get; set; }

    public string SelectedUserId { get; set; }
    public List<UsersItem> ListUsers { get; set; }

    public string UserName { get; set; }

    public bool Active { get; set; }

    public DateTime Date { get; internal set; }
  }
}