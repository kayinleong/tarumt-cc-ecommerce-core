namespace Tarumt.CC.Ecommerce.Extensions
{
    public static class AsyncEnumerableExtensions
    {
        public static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ExecuteAsync();

            async Task<List<T>> ExecuteAsync()
            {
                List<T> list = new();

                await foreach (T element in source)
                {
                    list.Add(element);
                }

                return list;
            }
        }
    }
}
