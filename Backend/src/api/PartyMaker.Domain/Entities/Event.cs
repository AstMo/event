using bgTeam.DataAccess.Impl.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PartyMaker.Domain.Entities
{
    [Schema("public")]
    [TableName("events")]
    public class Event : Entity
    {
        [ColumnName("name")]
        public string Name { get; set; }

        [ColumnName("date")]
        public DateTime Date { get; set; }

        [ColumnName("address")]
        public string Address { get; set; }

        [ColumnName("latitude")]
        public decimal Latitude { get; set; }

        [ColumnName("longitude")]
        public decimal Longitude { get; set; }

        [ColumnName("typeevent")]
        public ETypeEvent TypeEvent { get; set; }

        [ColumnName("totalbudget")]
        public decimal TotalBudget { get; set; }

        public int Count()
        {
            throw new NotImplementedException();
        }
    }
}
