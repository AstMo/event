using PartyMaker.Common.Impl;
using IdmClinic.DataAccess;
using System.Collections.Generic;

namespace PartyMaker.DataAccess.Common
{
    public class GetEntitiesByPageQueryContext : IPageableQueryContext
    {
        public KeyValuePair<string, string>[] Filters { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public ESortDirection SortDirection { get; set; }

        public string SortField { get; set; }
    }
}
