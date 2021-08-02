﻿using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;
using Gapotchenko.FX.Diagnostics;
using Microsoft.Toolkit.Uwp.Notifications;

namespace FrostyFix2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        string bf2015;
        string bf2017;
        string mea;
        string bf1;
        string nfs;
        string nfspayback;
        string gw2;
        string dai;
        string datadir;
        string origindir;
        string eaddir;
        string epicdir;
        string steamdir;
        bool customChoose = false;
        string isenabled;
        private DispatcherTimer dispatcherTimer;

        public void checkStatus() {
            isenabled = null;
            lbl_platform.Foreground = Brushes.LightGreen;
            Process[] origin = Process.GetProcessesByName("Origin");
            Process[] eadesktop = Process.GetProcessesByName("EADesktop");
            Process[] epicgames = Process.GetProcessesByName("EpicGamesLauncher");
            Process[] steam = Process.GetProcessesByName("steam");

            if (eadesktop.Length != 0) {
                foreach (var process in eadesktop) {
                    var env = process.ReadEnvironmentVariables();
                    isenabled = env["GAME_DATA_DIR"];
                    lbl_platform.Text = "Platform: EA Desktop";
                }
            }
            if (epicgames.Length != 0 && isenabled == null) {
                foreach (var process in epicgames) {
                    var env = process.ReadEnvironmentVariables();
                    isenabled = env["GAME_DATA_DIR"];
                    lbl_platform.Text = "Platform: Epic Games Launcher";
                }
            }
            if (steam.Length != 0 && isenabled == null) {
                foreach (var process in steam) {
                    var env = process.ReadEnvironmentVariables();
                    isenabled = env["GAME_DATA_DIR"];
                    lbl_platform.Text = "Platform: Steam";
                }
            }
            if (origin.Length != 0 && isenabled == null) {
                foreach (var process in origin) {
                    var env = process.ReadEnvironmentVariables();
                    isenabled = env["GAME_DATA_DIR"];
                    lbl_platform.Text = "Platform: Origin";
                }
            }
            if (isenabled == null) {
                isenabled = null;
                lbl_platform.Text = "";
            }

            if (isenabled != null) {
                string frostyprofile = new DirectoryInfo(isenabled).Name;
                lbl_enabled.Foreground = Brushes.LightGreen;
                lbl_profile.Foreground = Brushes.LightGreen;
                lbl_enabled_tooltip.Visibility = Visibility.Visible;
                lbl_enabled_tooltip.Content = isenabled;
                lbl_profile.Text = "Frosty Profile: " + frostyprofile;
                if (isenabled == "\\ModData" || !isenabled.Contains("ModData")) {
                    lbl_enabled.Text = "User Error when selecting path. Please click Disable Mods and try again";
                    lbl_enabled.Foreground = Brushes.Orange;
                }
                else if (bf2015 != null && isenabled.Contains(bf2015)) {
                    lbl_enabled.Text = "Game: Star Wars: Battlefront (2015)";
                }
                else if (bf2017 != null && isenabled.Contains(bf2017)) {
                    lbl_enabled.Text = "Game: Star Wars: Battlefront II (2017)";
                }
                else if (mea != null && isenabled.Contains(mea)) {
                    lbl_enabled.Text = "Game: Mass Effect: Andromeda";
                }
                else if (bf1 != null && isenabled.Contains(bf1)) {
                    lbl_enabled.Text = "Game: Battlefield One";
                }
                else if (nfs != null && isenabled.Contains(nfs)) {
                    lbl_enabled.Text = "Game: Need for Speed";
                }
                else if (nfspayback != null && isenabled.Contains(nfspayback)) {
                    lbl_enabled.Text = "Game: Need for Speed: Payback";
                }
                else if (gw2 != null && isenabled.Contains(gw2)) {
                    lbl_enabled.Text = "Game: PvZ: Garden Warfare 2";
                }
                else if (dai != null && isenabled.Contains(dai)) {
                    lbl_enabled.Text = "Game: Dragon Age: Inquisition";
                }
                else {
                    lbl_enabled.Text = "Game: Custom";
                }
                tooltipTray.ToolTipText = lbl_platform.Text + Environment.NewLine + lbl_profile.Text + Environment.NewLine + lbl_enabled.Text;
                tooltipTray.Icon = Properties.Resources.FrostyFixGreen;
            }
            else {
                lbl_enabled.Text = "Mods are Currently NOT Enabled";
                lbl_enabled.Foreground = Brushes.LightSalmon;
                lbl_enabled_tooltip.Content = "";
                lbl_enabled_tooltip.Visibility = Visibility.Hidden;
                lbl_profile.Text = "";
                tooltipTray.ToolTipText = lbl_enabled.Text;
                tooltipTray.Icon = Properties.Resources.FrostyFix;
            }

        }

        public void checkEnabled() {
            bool success =
                (rbtn_bf2015.IsChecked == true || rbtn_bf2017.IsChecked == true || rbtn_bf1.IsChecked == true || rbtn_mea.IsChecked == true || rbtn_nfs.IsChecked == true || rbtn_nfspayback.IsChecked == true || rbtn_gw2.IsChecked == true || rbtn_dai.IsChecked == true || customChoose == true) &&
                (rbtn_origin.IsChecked == true || rbtn_eadesk.IsChecked == true || rbtn_epicgames.IsChecked == true || rbtn_steam.IsChecked == true);
            if (success == true) {
                btn_enable.IsEnabled = true;
            }
            else {
                btn_enable.IsEnabled = false;
            }
        }

        public void enableButtonText() {
            string platform = null;
            if (rbtn_origin.IsChecked == true) platform = "Origin ";
            if (rbtn_eadesk.IsChecked == true) platform = "EA Desktop ";
            if (rbtn_epicgames.IsChecked == true) platform = "Epic Games Store ";
            if (rbtn_steam.IsChecked == true) platform = "Steam ";

            string game = null;
            if (rbtn_bf2015.IsChecked == true) game = "Star Wars Battlefront (2015)";
            if (rbtn_bf2017.IsChecked == true) game = "Star Wars Battlefront II (2017)";
            if (rbtn_bf1.IsChecked == true) game = "Battlefield One";
            if (rbtn_mea.IsChecked == true) game = "Mass Effect: Andromeda";
            if (rbtn_nfs.IsChecked == true) game = "Need for Speed";
            if (rbtn_nfspayback.IsChecked == true) game = "Need for Speed: Payback";
            if (rbtn_gw2.IsChecked == true) game = "PvZ: Garden Warfare 2";
            if (rbtn_dai.IsChecked == true) game = "Dragon Age: Inquisition";
            if (rbtn_custom.IsChecked == true) game = "Custom Game";

            btn_enable_text.Text = "Launch " + platform + "with Mods enabled";
            if (game != null) btn_enable_text.Text = btn_enable_text.Text + " for " + game;
        }

        public void openGameDir() {
            if (datadir != null) {
                Process.Start("explorer.exe", datadir);
            }
            else {
                string message = "Select a game in the main window";
                string title = "Select a game";
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(message, title, buttons, icon);
            }
        }

        public void delModData() {
            if (datadir != null) {
                if (Directory.Exists(datadir + "\\ModData")) {
                    string message = "Are you sure you want to delete ModData for the selected game? You will have to regenerate ModData again through Frosty.";
                    string title = "Delete ModData";
                    MessageBoxButton buttons = MessageBoxButton.YesNo;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(message, title, buttons, icon);
                    switch (result) {
                        case MessageBoxResult.Yes:
                            Directory.Delete(datadir + "\\ModData");
                            break;
                    }
                }
                else {
                    string message = "ModData does not exist for selected game.";
                    string title = "ModData does not exist";
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(message, title, buttons, icon);
                }
            }
            else {
                string message = "Select a game in the main window";
                string title = "Select a game";
                MessageBoxButton buttons = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(message, title, buttons, icon);
            }
        }

        private async void btn_enable_Click(object sender, RoutedEventArgs e) {
            Directory.CreateDirectory(datadir + "\\ModData");
            if (Directory.GetDirectories(datadir + "\\ModData").Length == 0 || Directory.Exists(datadir + "\\ModData\\Data")) {
                if (Directory.GetDirectories(datadir + "\\ModData").Length == 0) {
                    string message = "ModData is Empty. After mods are enabled, launch the game from Frosty to generate ModData";
                    string title = "Empty ModData";
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(message, title, buttons, icon);
                }

                if (Directory.Exists(datadir + "\\ModData\\Data") && (Directory.Exists(datadir + "\\ModData\\Default") || Directory.Exists(datadir + "\\ModData\\Editor"))) {
                    string message = "You are using both the Frosty Alpha and Frosty Public version. FrostyFix will not use the Alpha profile system unless you DELETE ModData (option in the About menu)";
                    string title = "Frosty Alpha";
                    MessageBoxButton buttons = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = MessageBox.Show(message, title, buttons, icon);
                }

                Mouse.OverrideCursor = Cursors.Wait;

                //Kill all Launcher processes
                foreach (var process in Process.GetProcessesByName("EADesktop")) {
                    process.Kill();
                }
                foreach (var process in Process.GetProcessesByName("Origin")) {
                    process.Kill();
                }
                if (rbtn_epicgames.IsChecked == true) {
                    foreach (var process in Process.GetProcessesByName("EpicGamesLauncher")) {
                        process.Kill();
                    }
                }
                if (rbtn_steam.IsChecked == true) {
                    foreach (var process in Process.GetProcessesByName("steam")) {
                        process.Kill();
                    }
                }
                await Task.Delay(1000);

                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = "cmd.exe";
                if (rbtn_origin.IsChecked == true) {
                    p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + datadir + "\\ModData\" && start \"\" \"" + origindir + "\\Origin.exe\"";
                    p.StartInfo.WorkingDirectory = origindir;
                }
                else if (rbtn_eadesk.IsChecked == true) {
                    p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + datadir + "\\ModData\" && start \"\" \"" + Path.GetDirectoryName(eaddir) + "\\EADesktop.exe\"";
                    p.StartInfo.WorkingDirectory = Path.GetDirectoryName(eaddir);
                }
                else if (rbtn_epicgames.IsChecked == true) {
                    p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + datadir + "\\ModData\" && start \"\" \"" + epicdir + "Launcher\\Portal\\Binaries\\Win32\\EpicGamesLauncher.exe\"";
                    p.StartInfo.WorkingDirectory = epicdir + "Launcher\\Portal\\Binaries\\Win32\\";
                }
                else if (rbtn_steam.IsChecked == true) {
                    p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + datadir + "\\ModData\" && start \"\" \"" + steamdir + "\\steam.exe\"";
                    p.StartInfo.WorkingDirectory = steamdir;
                }
                p.Start();

                Mouse.OverrideCursor = null;
                await Task.Delay(5000);
                checkStatus();
            }
            else {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.Title = "Select Profile";
                dialog.InitialDirectory = datadir + "ModData\\";
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                    Mouse.OverrideCursor = Cursors.Wait;

                    //Kill all Launcher processes
                    foreach (var process in Process.GetProcessesByName("EADesktop")) {
                        process.Kill();
                    }
                    foreach (var process in Process.GetProcessesByName("Origin")) {
                        process.Kill();
                    }
                    if (rbtn_epicgames.IsChecked == true) {
                        foreach (var process in Process.GetProcessesByName("EpicGamesLauncher")) {
                            process.Kill();
                        }
                    }
                    if (rbtn_steam.IsChecked == true) {
                        foreach (var process in Process.GetProcessesByName("steam")) {
                            process.Kill();
                        }
                    }
                    await Task.Delay(1000);

                    Process p = new Process();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.FileName = "cmd.exe";
                    if (rbtn_origin.IsChecked == true) {
                        p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + dialog.FileName + "\" && start \"\" \"" + origindir + "\\Origin.exe\"";
                        p.StartInfo.WorkingDirectory = origindir;
                    }
                    else if (rbtn_eadesk.IsChecked == true) {
                        p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + dialog.FileName + "\" && start \"\" \"" + Path.GetDirectoryName(eaddir) + "\\EADesktop.exe\"";
                        p.StartInfo.WorkingDirectory = Path.GetDirectoryName(eaddir);
                    }
                    else if (rbtn_epicgames.IsChecked == true) {
                        p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + dialog.FileName + "\" && start \"\" \"" + epicdir + "Launcher\\Portal\\Binaries\\Win32\\EpicGamesLauncher.exe\"";
                        p.StartInfo.WorkingDirectory = epicdir + "Launcher\\Portal\\Binaries\\Win32\\";
                    }
                    else if (rbtn_steam.IsChecked == true) {
                        p.StartInfo.Arguments = "/C set \"GAME_DATA_DIR=" + dialog.FileName + "\" && start \"\" \"" + steamdir + "\\steam.exe\"";
                        p.StartInfo.WorkingDirectory = steamdir;
                    }
                    p.Start();

                    Mouse.OverrideCursor = null;
                    await Task.Delay(4000);
                    checkStatus();
                }
            }
        }

        private async void btn_disable_Click(object sender, RoutedEventArgs e) {
            Mouse.OverrideCursor = Cursors.Wait;
            foreach (var process in Process.GetProcessesByName("EADesktop")) {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("Origin")) {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("EpicGamesLauncher")) {
                    process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("steam")) {
                process.Kill();
            }
            await Task.Delay(2000);
            Mouse.OverrideCursor = null;
            checkStatus();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            checkStatus();
        }

        public MainWindow() {
            InitializeComponent();
            MinimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
            CloseButton.Click += (s, e) => hideFF();

            tooltipTray.Icon = Properties.Resources.FrostyFix;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();


            //Get Paths using Registry
            using (RegistryKey bf2015key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\EA Games\STAR WARS Battlefront"))
                if (bf2015key != null) {
                    bf2015 = (string)bf2015key.GetValue("Install Dir");
                    rbtn_bf2015.IsEnabled = true;
                }
                else {
                    rbtn_bf2015.IsEnabled = false;
                    rbtn_bf2015.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            using (RegistryKey bf2017key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EA Games\STAR WARS Battlefront II"))
                if (bf2017key != null) {
                    bf2017 = (string)bf2017key.GetValue("Install Dir");
                    rbtn_bf2017.IsEnabled = true;
                }
                else {
                    rbtn_bf2017.IsEnabled = false;
                    rbtn_bf2017.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            using (RegistryKey bf1key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\EA Games\Battlefield 1"))
                if (bf1key != null) {
                    bf1 = (string)bf1key.GetValue("Install Dir");
                    rbtn_bf1.IsEnabled = true;
                }
                else {
                    rbtn_bf1.IsEnabled = false;
                    rbtn_bf1.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            using (RegistryKey meakey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\BioWare\Mass Effect Andromeda"))
                if (meakey != null) {
                    mea = (string)meakey.GetValue("Install Dir");
                    rbtn_mea.IsEnabled = true;
                }
                else {
                    rbtn_mea.IsEnabled = false;
                    rbtn_mea.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            using (RegistryKey nfskey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EA Games\Need for Speed"))
                if (nfskey != null) {
                    nfs = (string)nfskey.GetValue("Install Dir");
                    rbtn_nfs.IsEnabled = true;
                }
                else {
                    rbtn_nfs.IsEnabled = false;
                    rbtn_nfs.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            using (RegistryKey nfspaybackkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\EA Games\Need for Speed Payback"))
                if (nfspaybackkey != null) {
                    nfspayback = (string)nfspaybackkey.GetValue("Install Dir");
                    rbtn_nfspayback.IsEnabled = true;
                }
                else {
                    rbtn_nfspayback.IsEnabled = false;
                    rbtn_nfspayback.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            using (RegistryKey gw2key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\PopCap\Plants vs Zombies GW2"))
                if (gw2key != null) {
                    gw2 = (string)gw2key.GetValue("Install Dir");
                    rbtn_gw2.IsEnabled = true;
                }
                else {
                    rbtn_gw2.IsEnabled = false;
                    rbtn_gw2.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            using (RegistryKey daikey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Bioware\Dragon Age Inquisition"))
                if (daikey != null) {
                    dai = (string)daikey.GetValue("Install Dir");
                    rbtn_dai.IsEnabled = true;
                }
                else {
                    rbtn_dai.IsEnabled = false;
                    rbtn_dai.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            //Get Launcher paths
            using (RegistryKey origindirkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Electronic Arts\EA Core"))
                if (origindirkey != null) {
                    origindir = (string)origindirkey.GetValue("EADM6InstallDir");
                }
                else {
                    rbtn_origin.IsEnabled = false;
                    rbtn_origin.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }
            using (RegistryKey eaddirkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Electronic Arts\EA Desktop"))
                if (eaddirkey != null) {
                    eaddir = (string)eaddirkey.GetValue("DesktopAppPath");
                }
                else {
                    rbtn_eadesk.IsEnabled = false;
                    rbtn_eadesk.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }
            using (RegistryKey epicdirkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\EpicGames\Unreal Engine"))
                if (epicdirkey != null) {
                    epicdir = (string)epicdirkey.GetValue("INSTALLDIR");
                }
                else {
                    rbtn_epicgames.IsEnabled = false;
                    rbtn_epicgames.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }
            using (RegistryKey steamdirkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam"))
                if (steamdirkey != null) {
                    steamdir = (string)steamdirkey.GetValue("InstallPath");
                }
                else {
                    rbtn_steam.IsEnabled = false;
                    rbtn_steam.Foreground = new SolidColorBrush(Color.FromArgb(255, 140, 140, 140));
                }

            checkStatus();
            checkEnabled();
        }

        public void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (this.WindowState == WindowState.Maximized) {
                this.BorderThickness = new System.Windows.Thickness(8);
            }
            else {
                this.BorderThickness = new System.Windows.Thickness(0);
            }
        }

        private void ButtonGithub(object sender, RoutedEventArgs e) {
            AboutWindow about = new AboutWindow();
            about.Show();
        }

        private void rbtn_custom_Checked(object sender, RoutedEventArgs e) {
            btn_customchoose.IsEnabled = true;
            enableButtonText();
        }

        private void rbtn_custom_Unchecked(object sender, RoutedEventArgs e) {
            btn_customchoose.IsEnabled = false;
        }

        private void btn_customchoose_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            {
                openFileDlg.Filter = "Game executable (*.exe)|*.exe";
                openFileDlg.FilterIndex = 2;
                openFileDlg.RestoreDirectory = true;

                Nullable<bool> result = openFileDlg.ShowDialog();

                if (result == true) {
                    datadir = System.IO.Path.GetDirectoryName(openFileDlg.FileName);
                    txtb_custompath.Text = datadir;
                    customChoose = true;
                }
            }
        }

        private void rbtn_bf2015_Checked(object sender, RoutedEventArgs e) {
            datadir = bf2015;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_bf2017_Checked(object sender, RoutedEventArgs e) {
            datadir = bf2017;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_bf1_Checked(object sender, RoutedEventArgs e) {
            datadir = bf1;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_mea_Checked(object sender, RoutedEventArgs e) {
            datadir = mea;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_nfs_Checked(object sender, RoutedEventArgs e) {
            datadir = nfs;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_nfspayback_Checked(object sender, RoutedEventArgs e) {
            datadir = nfspayback;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_gw2_Checked(object sender, RoutedEventArgs e) {
            datadir = gw2;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_dai_Checked(object sender, RoutedEventArgs e) {
            datadir = dai;
            checkEnabled();
            enableButtonText();
        }

        private void rbtn_platform_Checked(object sender, RoutedEventArgs e) {
            checkEnabled();
            enableButtonText();
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e) {
            checkStatus();
        }

        private void quitFF (object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void showhideFF(object sender, RoutedEventArgs e) {
            if (this.Visibility == Visibility.Visible) {
                hideFF();
            }
            else if (this.Visibility == Visibility.Hidden) {
                showFF(sender, e);
            }
        }

        private void showFF(object sender, RoutedEventArgs e) {
            ShowHideFF.Header = "Hide FrostyFix";
            Show();
        }

        private void hideFF() {
            ShowHideFF.Header = "Show FrostyFix";
            Hide();
            new ToastContentBuilder()
            .AddText("FrostyFix has been minimized to the taskbar")
            .Show();
        }
    }
}