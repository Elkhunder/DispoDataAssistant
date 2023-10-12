﻿using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DispoDataAssistant.Messages;
using DispoDataAssistant.Models;
using DispoDataAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DispoDataAssistant.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly DeviceInformation _deviceInformation;
        private readonly Themes _themes;

        private readonly ISettingsService? _settingsManager;
        private readonly IUserSettingsSerivce? _userSettingsManager;

        private string? _selectedTheme;
        private string? _selectedDeviceType;
        private string? _selectedDeviceModel;
        private string? _selectedDeviceManufacturer;
        private string? _selectedPickupLocation;

        public event EventHandler? ToggleSettingsMenuRequested;
        public RelayCommand<string> OpenSettingsMenuCommand { get; private set; }
        public RelayCommand<string> CloseSettingsMenuCommand { get; private set; }

        public RelayCommand<SelectionChangedEventArgs> ThemeChangedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> DeviceTypeChangedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> DeviceModelChangedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> DeviceManufacturerChangedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PickupLocationChangedCommand { get; private set; }

    public SettingsViewModel()
        {
            Console.WriteLine("SettingsViewModel: Instance Created");

            _userSettingsManager = Ioc.Default.GetService<IUserSettingsSerivce>() ?? throw new NullReferenceException();
            _settingsManager = Ioc.Default.GetService<ISettingsService>() ?? throw new NullReferenceException();
            _deviceInformation = Ioc.Default.GetService<DeviceInformation>() ?? throw new NullReferenceException();
            _themes = Ioc.Default.GetService<Themes>() ?? throw new NullReferenceException();

            WeakReferenceMessenger.Default.Register<ToggleSettingsMenuMessage>(this, OnToggleSettingsMenuMessageReceived);
            OpenSettingsMenuCommand = new RelayCommand<string>(OpenSettingsMenu);
            CloseSettingsMenuCommand = new RelayCommand<string>(CloseSettingsMenu);

            ThemeChangedCommand = new RelayCommand<SelectionChangedEventArgs>(ThemeChanged);
            DeviceTypeChangedCommand = new RelayCommand<SelectionChangedEventArgs>(DeviceTypeChanged);
            DeviceModelChangedCommand = new RelayCommand<SelectionChangedEventArgs>(DeviceModelChanged);
            DeviceManufacturerChangedCommand = new RelayCommand<SelectionChangedEventArgs>(DeviceManufacturerChanged);
            PickupLocationChangedCommand = new RelayCommand<SelectionChangedEventArgs>(PickupLocationChanged);

            if (_settingsManager != null)
            {
                _selectedTheme = _settingsManager.GetTheme();
                _selectedDeviceType = _settingsManager.GetDeviceType();
                _selectedDeviceModel = _settingsManager.GetDeviceModel();
                _selectedDeviceManufacturer = _settingsManager.GetDeviceManufacturer();
                _selectedPickupLocation = _settingsManager.GetPickupLocation();
            }
            

        }

        public string SelectedTheme
        {
            get { return _selectedTheme ?? throw new NullReferenceException(); }
            set { _selectedTheme = value; }
        }

        public string SelectedDeviceType
        {
            get { return _selectedDeviceType ?? throw new NullReferenceException(); }
            set { _selectedDeviceType = value; }
        }

        public string SelectedDeviceModel
        {
            get { return _selectedDeviceModel ?? throw new NullReferenceException(); }
            set { _selectedDeviceModel = value; }
        }

        public string SelectedDeviceManufacturer
        {
            get { return _selectedDeviceManufacturer ?? throw new NullReferenceException(); }
            set { _selectedDeviceManufacturer = value; }
        }

        public string SelectedPickupLocation
        {
            get { return _selectedPickupLocation ?? throw new NullReferenceException(); }
            set { _selectedPickupLocation = value; }
        }

        public string[] Themes
        {
            get { return _themes.Names; }
        }

        public List<string> DeviceTypes
        {
            get 
            { 
                return _deviceInformation.DeviceTypes ?? throw new NullReferenceException(); 
            }
        }
        
        public List<string> DeviceModels
        {
            get 
            { 
                return _deviceInformation.DeviceModels ?? throw new NullReferenceException(); 
            }
        }

        public List<string> DeviceManufacturers
        {
            get 
            { 
                return _deviceInformation.DeviceManufacturers ?? throw new NullReferenceException(); 
            }
        }

        public List<string> PickupLocations
        {
            get
            {
                return _deviceInformation.PickupLocations ?? throw new NullReferenceException();
            }
        }

        private void OnToggleSettingsMenuMessageReceived(object recipient, ToggleSettingsMenuMessage message)
        {
            Console.WriteLine("SettingsViewModel: Toggle Settings Menu Message Received");
            ToggleSettingsMenuRequested?.Invoke(this, EventArgs.Empty);
        }

        private void CloseSettingsMenu(string? obj)
        {
            Console.WriteLine("Settings Menu Closed");

            ToggleSettingsMenuRequested?.Invoke(this, EventArgs.Empty);
        }

        public void ThemeChanged(SelectionChangedEventArgs? e)
        {
            Console.WriteLine($"Theme Changed: {_selectedTheme}");

            if(_settingsManager != null && _selectedTheme != null)
            {
                _settingsManager.SetTheme(_selectedTheme);
            }
            if(_userSettingsManager != null && _selectedTheme != null)
            {
                _userSettingsManager.ApplyTheme(_selectedTheme);
            }
        }

        public void DeviceTypeChanged(SelectionChangedEventArgs? e)
        {
            Console.WriteLine($"Device Type Changed: {_selectedDeviceType}");

            if (_settingsManager != null && _selectedDeviceType != null)
            {
                _settingsManager.SetDeviceType(_selectedDeviceType);
            }

            if (_userSettingsManager != null && _selectedDeviceType != null)
            {
                _userSettingsManager.ApplyDeviceType(_selectedDeviceType);
            }
        }
        public void DeviceModelChanged(SelectionChangedEventArgs? e)
        {
            Console.WriteLine($"Device Model Changed: {_selectedDeviceModel}");

            if (_settingsManager != null && _selectedDeviceModel != null)
            {
                _settingsManager.SetDeviceModel(_selectedDeviceModel);
            }

            if (_userSettingsManager != null && _selectedDeviceModel != null)
            {
                _userSettingsManager.ApplyDeviceModel(_selectedDeviceModel);
            }
        }

        public void DeviceManufacturerChanged(SelectionChangedEventArgs? e)
        {
            Console.WriteLine($"Device Manufacturer Changed: {_selectedDeviceManufacturer}");

            if (_settingsManager != null && _selectedDeviceManufacturer != null)
            {
                _settingsManager.SetDeviceManufacturer(_selectedDeviceManufacturer);
            }

            if (_userSettingsManager != null && _selectedDeviceManufacturer != null)
            {
                _userSettingsManager.ApplyDeviceManufacturer(_selectedDeviceManufacturer);
            }
        }

        public void PickupLocationChanged(SelectionChangedEventArgs? e)
        {
            Console.WriteLine($"Pickup Location Changed: {_selectedPickupLocation}");

            if( _settingsManager != null && _selectedPickupLocation != null )
            {
                _settingsManager.SetPickupLocation(_selectedPickupLocation);
            }

            if(_userSettingsManager != null && _selectedPickupLocation != null)
            {
                _userSettingsManager.ApplyPickupLocation(_selectedPickupLocation);
            }
        }

        private void OpenSettingsMenu(string? obj)
        {
            Console.WriteLine("Settings Menu Opened");
        }
    }
}
