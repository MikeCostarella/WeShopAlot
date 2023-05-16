namespace WeShopAlot.WebAPI.Helpers
{
    public class Pagination<T> where T : class
    {
        #region Member Variables

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        #endregion Member Variables

        #region Constructors

        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        #endregion Constructors
    }
}
