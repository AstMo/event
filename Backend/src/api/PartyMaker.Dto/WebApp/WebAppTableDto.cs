using System.Collections.Generic;

namespace PartyMaker.Dto.WebApp
{
    public class WebAppTableDto<T>
    {
        public WebAppTableDto()
        {
            Items = new List<T>();
        }

        public int TotalItems { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
