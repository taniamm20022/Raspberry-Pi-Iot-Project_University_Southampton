namespace LupenM.WebSite.Models
{
  public class ListingPageModel
  {
    public ListingPageModel()
    {
    }

    public MessageModel messageModel { get; set; }
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int ItemsCount { get; set; }
    public int TotalPages
    {
      get
      {
        if (PageSize == 0)
          return 0;

        return TotalRecords / PageSize + (TotalRecords % PageSize > 0 ? 1 : 0);
      }
    }

    private int _pageSize;
    public int PageSize
    {
      get
      {
        return _pageSize;
      }
      set
      {
        _pageSize = value;
      }
    }

    private int _pageOrder = 1;
    public int PageOrder
    {
      get
      {
        return _pageOrder;
      }
      set
      {
        _pageOrder = value;
      }
    }

    public int Position
    {
      get
      {
        return PageSize * (CurrentPage - 1);
      }
    }
  }
}