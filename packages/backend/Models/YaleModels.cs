namespace YaleAccess.Models
{
    public class YaleUserCode
    { 
        /// <summary>
        /// The ID of the user code. When making requets to ZWave.JS API, this is the value to use.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The code / pin number for the user code. This may be null if the user code is not set or is not in use.
        /// Even though its a string, it is a number and appropriate validation is required.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// The current status of the user code.
        /// </summary>
        public UserCodeStatus Status { get; set; }

        /// <summary>
        /// Is true is this is the 'home' or normal user code for daily use.
        /// </summary>
        public bool IsHome { get; set; }
    }

    public enum UserCodeStatus
    {
        /// <summary>
        /// This means the code is not being used, and can be assigned
        /// </summary>
        AVAILABLE = 0,

        /// <summary>
        /// This means that the code is in used, and cannot be assigned
        /// </summary>
        ENABLED = 1,

        /// <summary>
        /// Not sure what this means yet, perhaps it is related to schedules?
        /// </summary>
        DISABLED = 2
    }

}
