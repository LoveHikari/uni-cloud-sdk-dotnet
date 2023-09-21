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
            var dataBaseClient = new DataBaseClient("mp-9aad3507-485f-4787-ade7-a28ac438cff4", "TpfOrKS/zzORUMtxuw8qiQ==");
            await dataBaseClient.GetAccessTokenAsync();
            var vv = await dataBaseClient.QueryListAsync<Class1>("app-user", new QueryParameter()
            {
                //Where = "_id=='631d99354c978400011d747a'"
                Field = "nickname"
            });
            //_output.WriteLine(vv);

            Assert.True(true);
        }
    }
}