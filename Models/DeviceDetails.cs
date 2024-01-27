using System.Linq;

namespace DispoDataAssistant.Models
{
    public class DeviceDetails
    {
        public string? AssetTag { get; set; }
        public string? SerialNumber { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceManufacturer { get; set; }
        public string? DeviceModel { get; set; }
        public int Length
        {
            get
            {
                return typeof(DeviceDetails).GetProperties().Count();
            }
        }
    }
}
