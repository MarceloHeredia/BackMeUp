using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using BackMeUp.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace BackMeUp.UI.Pages
{
    public sealed partial class CreateBackupPage
    {

        private List<string> GameList => ["a", "b"];
        public CreateBackupPage()
        {
            this.InitializeComponent();
        }

        private void CreateBackup_Click(object sender, RoutedEventArgs e)
        {
            var saveBackup = new SaveBackup
            {
                GameName = SelectedGameName.SelectedValue.ToString(),
                SaveName = TextSaveName.Text,
                Creation = DateTime.Now,
                Description = TextDescription.Text
            };
        }
    }
}
