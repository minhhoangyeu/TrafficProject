using System;

namespace Traffic.Utilities.Helpers
{
    public static class PaginationHelper
    {
        public static string GetLink(string baseUrl, int page, double size, double totalItems)
        {
            var link = string.Empty;
            var nextPage = page + 1;
            var previousPage = page - 1;
            var lastPage = Math.Ceiling(totalItems / size);

            // next page
            if (nextPage < lastPage)
            {
                link += $"{baseUrl}?page={nextPage}&size={size}; rel=\"next\",";
            }

            // previous page
            if (page > 0)
            {
                link += $"{baseUrl}?page={previousPage}&size={size}; rel=\"prev\",";
            }

            // last page
            link += $"{baseUrl}?page={lastPage}&size={size}; rel=\"last\",";

            // first page
            link += $"{baseUrl}?page=0&size={size}; rel=\"first\"";

            return link;
        }
    }
}
