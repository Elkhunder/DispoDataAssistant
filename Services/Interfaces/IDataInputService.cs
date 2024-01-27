namespace DispoDataAssistant.Services.Interfaces
{
    internal interface IDataInputService
    {
        public void SetDeviceType(string deviceType);
        public string GetDeviceType();
        public string SetDeviceModel(string deviceModel);
        public string GetDeviceModel();
        public string SetDeviceManufacturer(string deviceManufacturer);
        public string GetDeviceManufacturer();
        public string SetPickupLocation(string pickupLocation);
        public string GetPickupLocation();
    }
}
