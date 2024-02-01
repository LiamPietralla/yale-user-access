using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Serilog;
using YaleAccess.Models;
using YaleAccess.Models.Options;
using YaleAccess.Services.Interfaces;
using ZWaveJS.NET;
using ZWaveOptions = YaleAccess.Models.Options.ZWaveOptions;

namespace YaleAccess.Services
{
    public class YaleAccessor : IYaleAccessor, IDisposable
    {
        #region Dispose Logic

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    driver?.Destroy();
                }

                disposedValue = true;
                driver = null;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose Logic

        private Driver? driver = null;
        private readonly ZWaveNode lockNode = null!;

        public YaleAccessor(IOptions<ZWaveOptions> zwave, IOptions<DevicesOptions> device)
        {
            // Retrive options from configuration
            ZWaveOptions zwaveOptions = zwave.Value;
            DevicesOptions devicesOptions = device.Value;

            // Create a new driver instance
            driver = new Driver(new Uri(zwaveOptions.Url), zwaveOptions.SchemaVersion);

            // Start the driver
            driver.Start();

            // Flag to indicate if the driver is ready to use
            bool isReady = false;

            // Subscribe to the driver ready event
            driver.DriverReady += () =>
            {
                isReady = true;
            };

            driver.StartUpError += (message) =>
            {
                throw new Exception(message);
            };

            // Wait for the driver to be ready
            while (!isReady)
            {
                Thread.Sleep(100);
            }

            // Get the lock node from the driver
            lockNode = driver.Controller.Nodes.Get(devicesOptions.YaleLockNodeId);
        }

        public async Task<YaleUserCode> GetCodeInformationAsync(int userCodeId)
        {
            // Setup the two tasks to get the values we need
            CMDResult status = await lockNode.GetValue(GetUserStatusValue(userCodeId));
            CMDResult code = await lockNode.GetValue(GetUserCodeValue(userCodeId));

            // Covert the result to a YaleUserCode object
            return new YaleUserCode()
            {
                Id = userCodeId,
                Code = GetUserCodeValue(code),
                Status = GetUserStatusValue(status),
                IsHome = false
            };
        }

        public async Task<bool> SetUserCode(int userCodeId, string code)
        {
            // Setup the set value task
            CMDResult result = await lockNode.SetValue(GetUserCodeValue(userCodeId), code);

            // If the result is not successful log the message
            if (!result.Success)
            {
                Log.Logger.Error("Failed to set user code {@userCodeId} to {@code}. Error message: {message}", userCodeId, code, result.Message);
            }

            // Return the result
            return result.Success;
        }

        public async Task<bool> SetCodeAsAvailable(int userCode)
        {
            // Setup the set value task
            CMDResult result = await lockNode.SetValue(GetUserStatusValue(userCode), (int)UserCodeStatus.AVAILABLE);

            // If the result is not successful log the message
            if (!result.Success)
            {
                Log.Logger.Error("Failed to set user code {@userCode} to available status. Error message: {message}", userCode, result.Message);
            }

            // Return the result
            return result.Success;
        }

        private static UserCodeStatus GetUserStatusValue(CMDResult result)
        {
            // Parse the payload as a JSON object
            JObject payloadObject = (JObject)result.ResultPayload;

            // Return the value property
            return (UserCodeStatus)payloadObject.GetValue("value")!.ToObject<int>();
        }

        private static string GetUserCodeValue(CMDResult result)
        {
            // Parse the payload as a JSON object
            JObject payloadObject = (JObject)result.ResultPayload;

            // Return the value property
            return payloadObject.GetValue("value")!.ToString();
        }

        private static ValueID GetUserStatusValue(int userCodeId)
        {
            return new ValueID()
            {
                commandClass = 99,
                endpoint = 0,
                property = "userIdStatus",
                propertyKey = userCodeId
            };
        }

        private static ValueID GetUserCodeValue(int userCodeId)
        {
            return new ValueID()
            {
                commandClass = 99,
                endpoint = 0,
                property = "userCode",
                propertyKey = userCodeId
            };
        }

        public static string ValidateCode(string newCode)
        {
            // The code must be between 4 and 6 digits
            if (newCode.Length < 4 || newCode.Length > 6)
            {
                return "The code must be between 4 and 6 digits.";
            }

            // The code must be numeric
            if (!int.TryParse(newCode, out int _))
            {
                return "The code must be numeric.";
            }

            // Otherwise, the code is valid, return an empty string
            return string.Empty;
        }
    }
}
