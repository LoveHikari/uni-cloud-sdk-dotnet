using Hikari.UniCloud.Sdk;
using Xunit.Abstractions;

namespace TestProject2
{
    public class UnitTest1
    {
        private ITestOutputHelper _output;
        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public async void Test1()
        {
            var dataBaseClient = new DataBaseClient("mp-6b46bbf4-5e74-44a6-97f0-b3a7ea457d8f", "QqHMPRDqxY60TEap2kwVhA==");
            Task.Run(dataBaseClient.GetAccessTokenAsync).Wait();
            QueryParameter parameter = new QueryParameter()
            {
                //Where = $"_id=='b23bdccc-ce68-4e12-8e43-ecc701fe165f' && isDel!=true",
            };

            await dataBaseClient.UpdateAsync("building", $"'_id' == '6497adab0c801c4baac87701'", new Dictionary<string, object>()
                    {
                        { "salePrice", 20 }
                    });
            //_output.WriteLine(model.Count.ToString());

            Assert.True(true);
        }
    }
}