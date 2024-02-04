using YaleAccess.Models;

namespace YaleAccess.Services.Interfaces
{
    public interface IYaleAccessor
    {
        public Task<YaleUserCode> GetCodeInformationAsync(int userCodeId);
        public Task<bool> SetUserCode(int userCodeId, string code);
        public Task<bool> SetCodeAsAvailable(int userCode);
    }
}
