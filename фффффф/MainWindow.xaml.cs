using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AudioPlayer
{
    public partial class MainWindow : Window
    {
        private List<string> playlist = new List<string>();
        private int currentTrackIndex = -1;
        private Random random = new Random();
        private DispatcherTimer positionTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            positionTimer.Interval = TimeSpan.FromSeconds(1);
            positionTimer.Tick += PositionTimer_Tick;
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string folderPath = openFileDialog.FileName;
                string[] audioFiles = Directory.GetFiles(folderPath, "*.mp3");
                playlist.AddRange(audioFiles);
                PlayNextTrack();
            }
        }

        private void PlayNextTrack()
        {
            if (playlist.Any())
            {
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
                PlayAudio(playlist[currentTrackIndex]);
            }
        }

        private void PlayAudio(string filePath)
        {
            mediaElement.Source = new Uri(filePath);
            mediaElement.Play();
            positionTimer.Start();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PlayNextTrack();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Any())
            {
                currentTrackIndex = (currentTrackIndex - 1 + playlist.Count) % playlist.Count;
                PlayAudio(playlist[currentTrackIndex]);
            }
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement repeat functionality
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement shuffle functionality
        }

        private void PositionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                mediaElement.Position = TimeSpan.FromSeconds(positionSlider.Value);
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Volume = volumeSlider.Value / 100;
        }

        private void PositionTimer_Tick(object sender, EventArgs e)
        {
            positionSlider.Value = mediaElement.Position.TotalSeconds;
            // Update other UI elements showing position information
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Show history window
        }
    }
}
