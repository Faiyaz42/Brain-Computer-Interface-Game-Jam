﻿using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;

namespace Notion.Unity
{
    public class Notion
    {
        public bool IsLoggedIn { get; private set; }
        public DeviceStatus Status { get; private set; }

        private readonly FirebaseController _firebase;
        private SubscriptionManager _subscriptionManager;
        private NeurosityUser _user;

        public Notion(FirebaseController firebaseController)
        {
            _firebase = firebaseController;
        }

        public async Task Login(Device credentials)
        {
            
            var u = await _firebase.Login(credentials);
            _user = new NeurosityUser(u, _firebase);

            if(credentials.DeviceId == "")
            {
                IEnumerable<string> credentialsKeys = await GetAccountDeviceIds(credentials);
                credentials.DeviceId = credentialsKeys.FirstOrDefault();
            }

            _subscriptionManager = new SubscriptionManager(_firebase, credentials, _user);

            Status = await _user.GetSelectedDeviceStatus();

            IsLoggedIn = true;
        }

        public async Task Logout()
        {
            await _subscriptionManager.Dispose();
            _firebase.Logout();
            IsLoggedIn = false;
        }

   
        public async Task<IEnumerable<string>> GetAccountDeviceIds(Device credentials)
        {
            return await _user.GetAccountDeviceIds();
        }

        public async Task<IEnumerable<DeviceInfo>> GetDevices()
        {
            return await _user.GetDevicesSlow(); // Modified from Ryan Turney's SDK. See sumamry in NeurosityUser.cs
        }

        public async Task<DeviceInfo> GetSelectedDevice()
        {
            return await _user.GetSelectedDevice();
        }

        public async Task<DeviceStatus> GetSelectedDeviceStatus()
        {
            return await _user.GetSelectedDeviceStatus();
        }

        public async Task<DeviceStatus> GetDeviceStatus(string DeviceId)
        {
            return await _user.GetDeviceStatus(DeviceId);
        }

        public void Subscribe(IMetricHandler handler)
        {
            _subscriptionManager.Subscribe(handler);
        }

        public void Subscribe(ISettingsHandler handler)
        {
            _subscriptionManager.Subscribe(handler);
        }

        public void Unsubscribe(IMetricHandler handler)
        {
            _subscriptionManager.Unsubscribe(handler);
        }

        public void Unsubscribe(ISettingsHandler handler)
        {
            _subscriptionManager.Unsubscribe(handler);
        }

        public async void ChangeSettings(Settings settings)
        {
            await _user.UpdateSettings(settings);
        }

        public async Task RemoveDevice(string deviceId)
        {
            await _user.RemoveDevice(deviceId);
        }

        public async Task RemoveDevice(Device device)
        {
            await RemoveDevice(device.DeviceId);
        }
    }
}