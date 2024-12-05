#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023 Analog Feelings and contributors.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using Bluechirp.Library.Models.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Bluechirp.Views;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel => (LoginViewModel)this.DataContext;

    public LoginPage()
    {
        this.InitializeComponent();
        this.DataContext = App.ServiceProvider.GetRequiredService<LoginViewModel>();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel.Dispose();
    }
}