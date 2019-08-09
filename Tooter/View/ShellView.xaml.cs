﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tooter.Helpers;
using Tooter.Services;
using Tooter.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tooter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellView : Page
    {
        public ShellView()
        {
            this.InitializeComponent();
            NavService.CreateInstance(ContentFrame);
            
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ContentFrame.Navigate(typeof(TimelineView), typeof(HomeViewModel));
            await ViewModel.DoAsyncPrepartions();
        }

        private void MenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FontIcon menuListItem)
            {
                if (menuListItem == HomeButtonIcon && !HomeButton.IsSelected)
                {
                    ContentFrame.Navigate(typeof(TimelineView), typeof(HomeViewModel));
                }
                else if(menuListItem == LocalButtonIcon && !LocalButton.IsSelected)
                {
                    ContentFrame.Navigate(typeof(TimelineView), typeof(LocalViewModel));
                }
                else if(menuListItem == FederatedButtonIcon && !FederatedButton.IsSelected)
                {
                    ContentFrame.Navigate(typeof(TimelineView), typeof(FederatedViewModel));
                }
            }
           
        }

        
    }
}
