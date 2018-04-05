
namespace QTB3.Test.LabResultPatterns.ClientModel.Pagebar
{
    public static class PagingTextCalculator
    {
        public static string Calculate(int skip, int take, int totalCount)
        {
            var pageNo = skip / take + 1;
            var totalPages = 0;
            if (totalCount % take != 0)
            {
                totalPages += 1;
            }

            totalPages += totalCount / take;

            return $"On page {pageNo} of {totalPages}";
        }
    }
}
