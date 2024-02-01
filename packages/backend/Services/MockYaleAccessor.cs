using YaleAccess.Models;
using YaleAccess.Services.Interfaces;

namespace YaleAccess.Services
{
    public class MockYaleAccessor : IYaleAccessor
    {
        private readonly MockYaleData _data;

        public MockYaleAccessor(MockYaleData data)
        {
            _data = data;
        }

        public Task<YaleUserCode> GetCodeInformationAsync(int userCodeId)
        {
            return Task.FromResult(_data.GetUserCode(userCodeId));
        }

        public Task<bool> SetCodeAsAvailable(int userCode)
        {
            return Task.FromResult(_data.SetUserCodeAsAvailable(userCode));
        }

        public Task<bool> SetUserCode(int userCodeId, string code)
        {
            return Task.FromResult(_data.SetUserCode(userCodeId, code));
        }
    }
}
