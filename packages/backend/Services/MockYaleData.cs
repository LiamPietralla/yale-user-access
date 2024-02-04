using YaleAccess.Models;

namespace YaleAccess.Services
{
    public class MockYaleData
    {
        public List<YaleUserCode> UserCodes { get; set; } = new();

        public MockYaleData()
        {
            // Create the home code
            YaleUserCode homeCode = new()
            {
                Id = 1,
                Code = "1234",
                IsHome = true,
                Status = UserCodeStatus.ENABLED
            };

            // Create 5 guest codes
            List<YaleUserCode> guestCodes = new();
            foreach (int code in Enumerable.Range(2, 5))
            {
                guestCodes.Add(new YaleUserCode()
                {
                    Id = code,
                    Code = "1234",
                    IsHome = false,
                    Status = UserCodeStatus.AVAILABLE
                });
            }

            // Add the home code to the list
            guestCodes.Add(homeCode);

            // Set the user codes
            UserCodes = guestCodes;
        }

        public YaleUserCode GetUserCode(int id)
        {
            return UserCodes.First(x => x.Id == id);
        }

        public bool SetUserCode(int id, string code)
        {
            // Get the user code
            YaleUserCode userCode = GetUserCode(id);

            // Set the code
            userCode.Code = code;

            // Update the user code in the list
            UserCodes[UserCodes.IndexOf(userCode)] = userCode;

            // Return true to indicate success
            return true;
        }

        public bool SetUserCodeAsAvailable(int id)
        {
            // Get the user code
            YaleUserCode userCode = GetUserCode(id);

            // Set the code
            userCode.Status = UserCodeStatus.AVAILABLE;

            // Update the user code in the list
            UserCodes[UserCodes.IndexOf(userCode)] = userCode;

            // Return true to indicate success
            return true;
        }
    }
}
