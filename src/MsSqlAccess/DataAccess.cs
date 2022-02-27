using DataModel;
using Microsoft.Extensions.DependencyInjection;

namespace MsSqlAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SqlDataAccess(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        public Task<IEnumerable<T>> TryGet<T>(T filter, DateTime from, DateTime to) where T : ITimeStamped
        {
            switch (filter)
            {
                case Item itemToRetrieve:
                    {
                        using var scope = _scopeFactory.CreateScope();

                        var itemContext = scope.ServiceProvider.GetRequiredService<ItemContext>();

                        var query = itemContext.Items.Where(x => (x.TimeStamp > from) && (x.TimeStamp <= to));

                        if (itemToRetrieve.ContentType != null)
                            query = query.Where(x => x.ContentType == itemToRetrieve.ContentType);
                        if (itemToRetrieve.Description != null)
                            query = query.Where(x => x.Description == itemToRetrieve.Description);
                        if (itemToRetrieve.Name != null)
                            query = query.Where(x => x.Name == itemToRetrieve.Name);

                        var result = (IEnumerable<T>)query.ToList();

                        return Task.FromResult(result);

                    }
                default:
                    throw new NotImplementedException($"No context available for item {typeof(T)}");
            }
        }


        public async Task<bool> TryInsert<T>(T item) where T : ITimeStamped
        {

            switch (item)
            {
                case Item itemToInsert:
                    {
                        using var scope = _scopeFactory.CreateScope();

                        var itemContext = scope.ServiceProvider.GetRequiredService<ItemContext>();

                        await itemContext.AddAsync<Item>(itemToInsert).ConfigureAwait(false);

                        var items = await itemContext.SaveChangesAsync().ConfigureAwait(false);

                        return (items != 0);
                    }
                default:
                    throw new NotImplementedException($"No context available for item {typeof(T)}");
            }


        }
    }
}