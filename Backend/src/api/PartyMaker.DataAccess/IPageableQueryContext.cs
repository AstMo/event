using PartyMaker.Common.Impl;

namespace IdmClinic.DataAccess
{
    public interface IPageableQueryContext
    {
        int Page { get; set; }

        int PageSize { get; set; }

        ESortDirection SortDirection { get; set; }

        string SortField { get; set; }
    }
}
