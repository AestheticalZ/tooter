﻿using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Commands;
using Tooter.Dialogs;
using TooterLib.Helpers;
using TooterLib.Model;
using TooterLib.Services;
using Tooter.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Tooter.Services;

namespace Tooter.ViewModel
{
    class ShellViewModel : Notifier
    {

        private Account _currentUser;

        public Account CurrentUser
        {
            get { return _currentUser; }
            set {
                _currentUser = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand NewTootCommand;

        public ShellViewModel()
        {
            NewTootCommand = new RelayCommand(async () => await NavigateToTootView());
            App.Current.EnteredBackground += Current_EnteredBackground;
        }

        private async void Current_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await CacheService.CacheCurrentTimeline();
            deferral.Complete();
        }

        private async Task NavigateToTootView()
        {
            await new NewTootDialog(CurrentUser.StaticAvatarUrl).ShowAsync();
        }


        internal async Task DoAsyncPrepartions()
        {
            try
            {
                CurrentUser = await ClientHelper.Client.GetCurrentUser();
            }
            catch (Exception)
            {

                await ErrorService.ShowConnectionError();
            }
        }

        internal async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await ClientHelper.MakeLogoutPreprationsAsync();
            NavService.CreateInstance((Frame)Window.Current.Content);
            NavService.Instance.Navigate(typeof(LoginView));
        }

    }
}
