﻿using Mastonet.Entities;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Core;
using Windows.Storage;

namespace Tooter.Helpers
{
    public static class ClientDataHelper
    {
        static LocalObjectStorageHelper _localStorageHelper = new LocalObjectStorageHelper();
        internal static HashSet<string> ClientProfileList { get; } = new HashSet<string>();
        internal static string LastUsedProfile { get; private set; } = null;

        const string SavedClientProfilesFileName = "savedClientProfiles.txt";

        // Token settings
        const string AccessTokenString = "accessToken";
        const string CreatedAtString = "createdAt";
        const string ScopeString = "scope";
        const string TokenTypeString = "tokenType";

        // App Registration settings
        const string InstanceString = "instance";
        const string AppScopeString = "appScope";
        const string AppIDString = "appID";

        // Profile swapping settings
        const string LastUsedProfileString = "lastUsedProfile";

        internal async static Task StartUpAsync()
        {
            await LoadClientProfiles();
        }

        internal static void SetLastUsedProfile(string clientProfileID)
        {
            _localStorageHelper.Save(LastUsedProfileString, clientProfileID);
        }

        internal static string GetLastUsedProfile()
        {
            LastUsedProfile = _localStorageHelper.Read<string>(LastUsedProfileString);
            return LastUsedProfile;
        }

        internal static async Task StoreClientData(string clientProfileID, Auth token, AppRegistration appRegistration)
        {
            if (!ClientProfileList.Contains(clientProfileID))
            {
                Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
                dataDictionary[AccessTokenString] = token.AccessToken;
                dataDictionary[CreatedAtString] = token.CreatedAt;
                dataDictionary[ScopeString] = token.Scope;
                dataDictionary[TokenTypeString] = token.TokenType;
                dataDictionary[InstanceString] = appRegistration.Instance;
                dataDictionary[AppScopeString] = appRegistration.Scope;
                dataDictionary[AppIDString] = appRegistration.Id;

                _localStorageHelper.Save<object>(clientProfileID, dataDictionary);


                await AddClientProfileAsync(clientProfileID);
            }

        }

        internal static async Task AddClientProfileAsync(string clientProfileID)
        {
            ClientProfileList.Add(clientProfileID);
            StorageFile fileToSave = await GetSavedClientsFileAsync();
            await FileIO.AppendTextAsync(fileToSave, $"{clientProfileID},");
        }

        internal static async Task RemoveClientProfileAsync(string clientProfileID)
        {
            ClientProfileList.Remove(clientProfileID);

            StringBuilder contentBuilder = new StringBuilder();
            for (int i = 0; i < ClientProfileList.Count; i++)
            {
                contentBuilder.Append($"{ClientProfileList.ElementAt(i)},");
            }

            StorageFile profilesFile = await GetSavedClientsFileAsync();

            await FileIO.WriteTextAsync(profilesFile, contentBuilder.ToString());
        }

        private static async Task<StorageFile> GetSavedClientsFileAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var savedClientsFile = await localFolder.CreateFileAsync(SavedClientProfilesFileName, CreationCollisionOption.OpenIfExists);
            return savedClientsFile;
        }

        internal static (Auth token, AppRegistration appRegistration) LoadClientProfile(string clientProfileID)
        {
            Auth token = new Auth();
            AppRegistration appRegistration = new AppRegistration();

            // values to load from local storage
            token.AccessToken = _localStorageHelper.Read<string>(clientProfileID, AccessTokenString);
            token.CreatedAt = _localStorageHelper.Read<string>(clientProfileID, CreatedAtString);
            token.Scope = _localStorageHelper.Read<string>(clientProfileID, ScopeString);
            token.TokenType = _localStorageHelper.Read<string>(clientProfileID, TokenTypeString);

            appRegistration.Id = _localStorageHelper.Read<long>(clientProfileID, AppIDString);
            appRegistration.Instance = _localStorageHelper.Read(clientProfileID, InstanceString);
            appRegistration.Scope = _localStorageHelper.Read<Mastonet.Scope>(clientProfileID, ScopeString);


            // Values to load from constants.
            appRegistration.ClientId = APIKeys.ClientID;
            appRegistration.ClientSecret = APIKeys.ClientSecret;
            appRegistration.RedirectUri = APIKeys.RedirectUri;

            return (token, appRegistration);
        }

        private async static Task LoadClientProfiles()
        {
            StorageFile clientProfilesFile = await GetSavedClientsFileAsync();
            string fileContents = await FileIO.ReadTextAsync(clientProfilesFile);
            string[] loadedProfiles = fileContents.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var profile in loadedProfiles)
            {
                ClientProfileList.Add(profile);
            }

        }


    }
}
