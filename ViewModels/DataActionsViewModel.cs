﻿using System;
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
        private string _tabFilePath = "output.csv";
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

        public DataActionsViewModel(DataInputViewModel dataInputViewModel)
        {
            _dataInputViewModel = dataInputViewModel;
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
            string tabFilename = $"{PickupDate!.Value:MM dd yyyy} {PickupLocation}";
            string commaFileName = $"{PickupDate!.Value:MM dd yyyy} {PickupLocation}";

            if (!string.IsNullOrWhiteSpace(AssetTag))
            {
                _devices.Add(CollectDeviceDetails());

                _dataInputViewModel.ClearInputControls();
                PickupDate = null;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "TXT files (*.txt)|*.txt",
                FileName = tabFilename,
                DefaultExt = ".txt"
                
            };

            saveFileDialog.OverwritePrompt = false;
            bool? result = saveFileDialog.ShowDialog();

            if (result == false) { return; }
            if (!result.HasValue && !result.Value) { return; }

            _tabFilePath = saveFileDialog.FileName;

            VerifyCsvHeaderAndWriteIfNotPresent(_tabFilePath, "\t");

            string? directory = Path.GetDirectoryName(_tabFilePath);
            string csvFileName = $"{commaFileName}.csv";
            string? csvFilePath = string.Empty;
            if(!string.IsNullOrEmpty(directory))
            {
                csvFilePath = Path.Combine(directory, csvFileName);
                VerifyCsvHeaderAndWriteIfNotPresent(csvFilePath, ",");
            }

            foreach (DeviceDetails device in _devices)
            {
                
                AppendCsvLine(device, _tabFilePath, "\t");
                if(!string.IsNullOrEmpty(csvFilePath))
                {
                    AppendCsvLine(device, csvFilePath, ",");
                }
            }
            MessageBox.Show("Devices saved to file successfully");
            _devices.Clear();
            _dataInputViewModel.AssetTagTextBox.Focus();
        }

        private void VerifyCsvHeaderAndWriteIfNotPresent(string file, string delimiter)
        {
            if (!File.Exists(file))
            {
                WriteCsvHeader(file, delimiter);
            }
            else
            {
                string headerLine;
                using (StreamReader sr = new StreamReader(file))
                {
                    headerLine = sr.ReadLine()!;
                }
                var csvHeader = _csvHandler.BuildCsvHeader(delimiter);
                csvHeader = csvHeader.TrimEnd('\r', '\n');
                if (headerLine != csvHeader)
                {
                    WriteCsvHeader(file, delimiter);
                }
            }
        }

        private void WriteCsvHeader(string file, string delimiter)
        {
            string csvHeader = _csvHandler.BuildCsvHeader(delimiter);

            using (StreamWriter newFile = new StreamWriter(file))
            {
                newFile.Write(csvHeader);
            }
        }

        private void AppendCsvLine(DeviceDetails device, string file, string delimiter)
        {
            string csvLine = _csvHandler.ConvertDeviceDetailsToCsvLine(device, delimiter);
            using (StreamWriter newFile = new StreamWriter(file, append: true))
            {
                newFile.WriteLine(csvLine);
            };
        }
    }
}
