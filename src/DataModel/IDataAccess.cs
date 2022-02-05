namespace DataModel
{
    public interface IDataAccess
    {
        public Task<bool> TryInsert<T>(T item)
            where T : ITimeStamped;

        public Task<IEnumerable<T>> TryGet<T>(T filter, DateTime from, DateTime to)
             where T : ITimeStamped;
    }
}