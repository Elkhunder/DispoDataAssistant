using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DispoDataAssistant.Handlers;
using DispoDataAssistant.Models;
using Microsoft.Win32;

namespace DispoDataAssistant.ViewModels
{
    //TODO: binding next device command to view
    //      binding remove device command to view
    //      binding save file command to view
    
    public class DataActionsViewModel : BaseViewModel
    {
        private string _csvFilePath = "output.csv";
        private readonly List<DeviceDetails> _devices;
        private DataInputViewModel _dataInputViewModel;
        private readonly CsvHandler _csvHandler;

        public string AssetTag
        {
            get => _dataInputViewModel.AssetTag;
            set => _dataInputViewModel.AssetTag = value;
        }
        public string SerialNumber
        {
            get => _dataInputViewModel.SerialNumber;
            set => _dataInputViewModel.SerialNumber = value;
        }
        public string DeviceType
        {
            get => _dataInputViewModel.DeviceType;
            set => _dataInputViewModel.DeviceType = value;
        }
        public string DeviceModel
        {
            get => _dataInputViewModel.DeviceModel;
            set => _dataInputViewModel.DeviceModel = value;
        }
        public string DeviceManufacturer
        {
            get => _dataInputViewModel.DeviceManufacturer;
            set => _dataInputViewModel.DeviceManufacturer = value;
        }
        public string PickupLocation
        {
            get => _dataInputViewModel.PickupLocation ?? "{PickupLocation}";
            set => _dataInputViewModel.PickupLocation = value;
        }
        public DateTime? PickupDate
        {
            get => _dataInputViewModel.PickupDate ?? DateTime.Now;
            set => _dataInputViewModel.PickupDate = value;
        }

        public RelayCommand NextDeviceCommand { get; private set; }
        public RelayCommand RemoveDeviceCommand { get; private set; }
        public RelayCommand SaveFileCommand { get; private set; }

        public DataActionsViewModel()
        {
            _dataInputViewModel = Ioc.Default.GetService<DataInputViewModel>()!;
            _devices = new List<DeviceDetails>();
            _csvHandler = new CsvHandler();

            NextDeviceCommand = new RelayCommand(NextDevice);
            RemoveDeviceCommand = new RelayCommand(RemoveDevice);
            SaveFileCommand = new RelayCommand(SaveFile);
        }

        private DeviceDetails CollectDeviceDetails()
        {
            DeviceDetails details = new DeviceDetails
            {
                AssetTag = AssetTag,
                SerialNumber = SerialNumber,
                DeviceType = DeviceType,
                DeviceManufacturer = DeviceManufacturer,
                DeviceModel = DeviceModel,
            };
            return details;
        }

        private void NextDevice()
        {
            _devices.Add(CollectDeviceDetails());

            _dataInputViewModel.ClearInputControls();

            //Apply focus to asset tag textbox
            _dataInputViewModel.AssetTagTextBox.Focus();

        }

        private void RemoveDevice()
        {
            if (_devices.Count == 0)
            {
                _dataInputViewModel.AssetTagTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(AssetTag) && string.IsNullOrWhiteSpace(SerialNumber))
            {
                _devices.RemoveAt(_devices.Count - 1);
                _dataInputViewModel.AssetTagTextBox.Focus();
                return;
            }
            else if (!string.IsNullOrWhiteSpace(AssetTag))
            {
                _devices.RemoveAll(device => device.AssetTag == AssetTag);
                _dataInputViewModel.AssetTagTextBox.Focus();
            }
            else if (!string.IsNullOrWhiteSpace(SerialNumber))
            {
                _devices.RemoveAll(device => device.SerialNumber == SerialNumber);
                _dataInputViewModel.AssetTagTextBox.Focus();
            }
            else
            {
                _dataInputViewModel.AssetTagTextBox.Focus();
                //Handle device not found
                return;

            }
        }

        private void SaveFile()
        {
            string filename = $"{PickupLocation}-Dispo-{PickupDate!.Value:MMdyyyy}";

            if(!string.IsNullOrWhiteSpace(AssetTag))
            {
                _devices.Add(CollectDeviceDetails());

                _dataInputViewModel.ClearInputControls();
                PickupDate = null;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "TXT files (*.txt)|*.txt",
                FileName = filename,
                DefaultExt = ".txt"
            };
            
            saveFileDialog.OverwritePrompt = false;
            bool? result = saveFileDialog.ShowDialog();

            if (result == false) { return; }
            if(!result.HasValue && !result.Value) { return; }

            _csvFilePath = saveFileDialog.FileName;

            if (!File.Exists(saveFileDialog.FileName))
            {
                WriteCsvHeader();
            }
            else
            {
                string headerLine;
                using (StreamReader sr = new StreamReader(saveFileDialog.FileName))
                {
                    headerLine = sr.ReadLine()!;
                }
                var csvHeader = _csvHandler.BuildCsvHeader();
                csvHeader = csvHeader.TrimEnd('\r', '\n');
                if (headerLine != csvHeader)
                {
                    WriteCsvHeader();
                }
            }
            foreach (DeviceDetails device in _devices)
            {
                string csvLine = _csvHandler.ConvertDeviceDetailsToCsvLine(device);
                AppendCsvLine(csvLine);
            }
            MessageBox.Show("Devices saved to file successfully");
            _devices.Clear();
            _dataInputViewModel.AssetTagTextBox.Focus();
        }

        private void WriteCsvHeader()
        {
            string csvHeader = _csvHandler.BuildCsvHeader();

            using (StreamWriter file = new StreamWriter(_csvFilePath))
            {
                file.Write(csvHeader);
            }
        }

        private void AppendCsvLine(string csvLine)
        {
            using (StreamWriter file = new StreamWriter(_csvFilePath, append: true))
            {
                file.WriteLine(csvLine);
            };
        }
    }
}
