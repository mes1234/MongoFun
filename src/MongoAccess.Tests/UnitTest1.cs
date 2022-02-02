using DataModel;
using Xunit;

namespace MongoAccess.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void FilterDefintionBuilder_Test()
        {
            var filterObj = new Item
            {
                Name = "Item 1",
                Description = "This is itme 1"

            };
            var filterBuilder = new FilterBuilder<Item>();

            var filters = filterBuilder.BuildFilter(filterObj);
        }
    }
}