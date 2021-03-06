﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AmbientSounds.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UploadPage : Page
    {
        public UploadPage()
        {
            this.InitializeComponent();
        }

        private void GoBack()
        {
            if (App.AppFrame.CanGoBack)
            {
                App.AppFrame.GoBack();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = true;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = false;
        }
    }
}