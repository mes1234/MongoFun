using DataModel;
using MongoDB.Driver;

namespace MongoAccess
{
    public interface IDataAccess
    {
        public Task<bool> TryInsert<T>(T item);
    }

    public class DataAccess : IDataAccess
    {
        private readonly static MongoClient client = new("mongodb://172.17.0.3:27017");

        public async Task<bool> TryInsert<T>(T item)
        {
            try
            {
                var db = client.GetDatabase("dummy");
                var collection = db.GetCollection<T>("items");

                await collection.InsertOneAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}