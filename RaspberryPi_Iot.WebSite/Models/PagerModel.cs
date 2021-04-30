namespace LupenM.WebSite.Models
{
  public class PagerModel
  {
    public PagerModel(int currentPage, int pageSize, int recordsCountCurrentPage, int totalRecords)
    {
      this._pageSize = pageSize;
      this.PageOrder = 1; // ?????????????????

      int totalPages = totalRecords / this._pageSize;

      if (totalRecords % (int)this._pageSize > 0) totalPages++;

      if (totalPages <= this.DisplayPagesCount)
      {
        this.StartPage = 1;
        this.EndPage = totalPages < this.StartPage ? this.StartPage : totalPages;
      }
      else
      {
        this.StartPage = currentPage - this.DisplayPagesCount / 2;

        if (this.StartPage < 1) this.StartPage = 1;

        this.EndPage = this.StartPage + this.DisplayPagesCount - 1;

        if (this.EndPage > totalPages)
        {
          int offset = EndPage - totalPages;

          this.StartPage -= offset;
          this.EndPage = totalPages;
        }
      }

      this.TotalRecords = totalRecords;
      this.TotalPagesReal = totalPages;
      this.TotalPages = totalPages > 0 ? totalPages : 1;
      this.CurrentPage = currentPage;
      this.NoResultsMessage = "Pager_NoResultsMessage";
    }

    // TODO:
    public PagerModel(int currentPage, int pageSize, int totalRecords, int recordsCountCurrentPage, int? pageOrder = 1)
    {
      this._pageSize = pageSize;
      this.PageOrder = pageOrder;

      int totalPages = totalRecords / this._pageSize;

      if (totalRecords % (int)this._pageSize > 0) totalPages++;

      if (totalPages <= this.DisplayPagesCount)
      {
        this.StartPage = 1;
        this.EndPage = totalPages < this.StartPage ? this.StartPage : totalPages;
      }
      else
      {
        this.StartPage = currentPage - this.DisplayPagesCount / 2;

        if (this.StartPage < 1) this.StartPage = 1;

        this.EndPage = this.StartPage + this.DisplayPagesCount - 1;

        if (this.EndPage > totalPages)
        {
          int offset = EndPage - totalPages;

          this.StartPage -= offset;
          this.EndPage = totalPages;
        }
      }

      this.TotalPagesReal = totalPages;
      this.TotalPages = totalPages > 0 ? totalPages : 1;
      this.CurrentPage = currentPage;
      this.NoResultsMessage = "Pager_NoResultsMessage";
      this.TotalRecords = totalRecords;
      this.RecordsCountCurrentPage = recordsCountCurrentPage;
    }

    public int _pageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalPagesReal { get; set; }
    public int CurrentPage { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public string NoResultsMessage { get; set; }
    public int? PageOrder { get; set; }
    public int TotalRecords { get; set; }
    public int RecordsCountCurrentPage { get; set; }
    private int _displayPagesCount = 5;
    public int DisplayPagesCount
    {
      get
      {
        return _displayPagesCount;
      }
      set
      {
        _displayPagesCount = value;
      }
    }
  }
}