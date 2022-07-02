﻿using System;
using System.Media;
using System.Windows;

namespace DyviniaUtils.Dialogs {
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionDialog : Window {
        public ExceptionDialog(Exception ex, string title, bool isCrash, string messagePrefix) {
            InitializeComponent();

            Title = title;
            DataContext = this;

            // Show or hide Header & Play according sounds
            int headerHeight = 30;
            if (!isCrash) {
                headerHeight = 5;
                HeaderText.Visibility = Visibility.Collapsed;
                SystemSounds.Exclamation.Play();
            }
            else SystemSounds.Hand.Play();
            Header.Height = new GridLength(headerHeight);

            // Create exception message
            string message = ex.Message;
            if (messagePrefix != null) 
                message = messagePrefix + Environment.NewLine + message;
            if (ex.InnerException != null)
                message += Environment.NewLine + Environment.NewLine + ex.InnerException;
            message += Environment.NewLine + Environment.NewLine + ex.StackTrace;
            ExceptionText.Text = message;

            if (isCrash) CloseButton.Click += (s, e) => Environment.Exit(0);
            else CloseButton.Click += (s, e) => Close();
            CopyButton.Click += (s, e) => Clipboard.SetDataObject(message);
        }

        public static void Show(Exception ex, string title, bool isCrash = false, string messagePrefix = null) {
            Application.Current.Dispatcher.Invoke(() => {
                ExceptionDialog window = new(ex, title, isCrash, messagePrefix);
                window.ShowDialog();
            });
        }
    }
}
