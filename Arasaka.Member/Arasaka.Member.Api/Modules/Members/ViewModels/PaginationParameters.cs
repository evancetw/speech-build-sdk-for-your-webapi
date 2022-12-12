using System.Reflection;

namespace Arasaka.Member.Api.Modules.Members.ViewModels
{
    public class PaginationParameters
    {
        const int maxPageSize = 30;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public static ValueTask<PaginationParameters?> BindAsync(HttpContext context, ParameterInfo parameter)
        {
            int.TryParse(context.Request.Query["pageSize"], out var pageSize);
            int.TryParse(context.Request.Query["pageNumber"], out var pageNumber);

            var result = new PaginationParameters
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            };

            return ValueTask.FromResult<PaginationParameters?>(result);
        }

        public static bool TryParse(string value, out PaginationParameters paginationParameters)
        {
            paginationParameters = null;

            return false;
        }
    }
}
