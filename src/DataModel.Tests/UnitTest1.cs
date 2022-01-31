using Xunit;
using DataModel;

namespace DataModel.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var item = new Item { ContentType = ContentType.Default };
            Assert.NotNull(item);
        }
    }
}