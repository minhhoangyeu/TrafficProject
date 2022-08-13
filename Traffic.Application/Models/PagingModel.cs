using System.Collections.Generic;

namespace Traffic.Application.Models
{
    public class PagingModel<T> where T : class
    {
        public int TotalItems { get; set; }

        public string Link { get; set; }

        public IList<T> Results { get; set; }

        public PagingModel()
        {
            Results = new List<T>();
        }
    }
}