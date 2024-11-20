using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Media.Animation;
using System.Xml;
using System.Windows.Media.Imaging;
using WMPLib;


namespace CurrentPlayer
{
	public partial class MainWindow : Window
	{
		ObservableCollection<FileInfo> playlist = new ObservableCollection<FileInfo>();
		bool IsPlaying = false;
		int ActiveIndex = 0;
		int Repeat = 0;

		bool Shuffle = false;

		int fadeCounter = -1;

		int rewindCounter = -1;
		int fastForwardCounter = -1;

		#region Icons
		BitmapImage FfIcon;
		BitmapImage FullscreenIcon;
		BitmapImage FullscreenExitIcon;
		BitmapImage MuteIcon;
		BitmapImage OpenIcon;
		BitmapImage PauseIcon;
		BitmapImage PlayIcon;
		BitmapImage PlaylistIcon;
		BitmapImage PlaylistClearIcon;
		BitmapImage PlaylistSaveIcon;
		BitmapImage RepeatIcon;
		BitmapImage RepeatOneIcon;
		BitmapImage RewindIcon;
		BitmapImage ShuffleIcon;
		BitmapImage StopIcon;
		BitmapImage VolumeIcon;
		#endregion
		public MainWindow()
		{
			InitializeComponent();

			this.MinHeight = 420;
			this.MinWidth = 600;

			GridMain.AllowDrop = true;
			Player.AllowDrop = true;
			Player.ScrubbingEnabled = true;

			this.WindowState = WindowState.Normal;

			Player.LoadedBehavior = System.Windows.Controls.MediaState.Manual;

			GridPlaylist.Visibility = Visibility.Hidden;
			GridPlaylist.IsEnabled = false;

			// Timer
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(.1);
			timer.Tick += timer_Tick;
			timer.Start();

			#region Icons
			Uri BaseUri = new Uri(@"D:\Code\iTi\MATERIAL\Programming - WPF\Project\Images\Controls\Blue2Original\");

			FfIcon = new BitmapImage(new Uri(BaseUri, @"FastForward.png"));
			FullscreenIcon = new BitmapImage(new Uri(BaseUri, @"Fullscreen.png"));
			FullscreenExitIcon = new BitmapImage(new Uri(BaseUri, @"FullscreenExit.png"));
			MuteIcon = new BitmapImage(new Uri(BaseUri, @"Mute.png"));
			OpenIcon = new BitmapImage(new Uri(BaseUri, @"OpenMedia.png"));
			PauseIcon = new BitmapImage(new Uri(BaseUri, @"Pause.png"));
			PlayIcon = new BitmapImage(new Uri(BaseUri, @"Play.png"));
			PlaylistIcon = new BitmapImage(new Uri(BaseUri, @"Playlist.png"));
			PlaylistClearIcon = new BitmapImage(new Uri(BaseUri, @"PlaylistClear.png"));
			PlaylistSaveIcon = new BitmapImage(new Uri(BaseUri, @"PlaylistSave.png"));
			RepeatIcon = new BitmapImage(new Uri(BaseUri, @"Repeat.png"));
			RepeatOneIcon = new BitmapImage(new Uri(BaseUri, @"RepeatOne.png"));
			RewindIcon = new BitmapImage(new Uri(BaseUri, @"Rewind.png"));
			ShuffleIcon = new BitmapImage(new Uri(BaseUri, @"Shuffle.png"));
			StopIcon = new BitmapImage(new Uri(BaseUri, @"Stop.png"));
			VolumeIcon = new BitmapImage(new Uri(BaseUri, @"Volume.png"));
			#endregion

			#region IconToButtons
			ButtonNext.OpacityMask = new ImageBrush(FfIcon);
			ButtonFullScreen.OpacityMask = new ImageBrush(FullscreenIcon);
			ButtonVolume.OpacityMask = new ImageBrush(VolumeIcon);
			ButtonOpen.OpacityMask = new ImageBrush(OpenIcon);
			ButtonPlayPause.OpacityMask = new ImageBrush(PlayIcon);
			ButtonPlaylist.OpacityMask = new ImageBrush(PlaylistIcon);
			ButtonClearPlaylist.OpacityMask = new ImageBrush(PlaylistClearIcon);
			ButtonSavePlaylist.OpacityMask = new ImageBrush(PlaylistSaveIcon);
			ButtonRepeat.OpacityMask = new ImageBrush(RepeatIcon);
			ButtonPrevious.OpacityMask = new ImageBrush(RewindIcon);
			ButtonShuffle.OpacityMask = new ImageBrush(ShuffleIcon);
			ButtonStop.OpacityMask = new ImageBrush(StopIcon);
			ButtonOpenPlaylist.OpacityMask = new ImageBrush(OpenIcon);
			#endregion
		}
		//
		// Drag and Drop events
		//
		private void GridMediaElement_Drop(object sender, DragEventArgs e)
		{
			string [] sourceString = ((string[])e.Data.GetData(DataFormats.FileDrop));

			playlist.Clear();
			foreach(string s in sourceString)
				playlist.Add(new FileInfo(s));

			AddMediaToListView();

			this.PlayMedia(ActiveIndex);
		}
		private void ListViewPlaylist_Drop(object sender, DragEventArgs e)
		{
			string[] sourceString = ((string[])e.Data.GetData(DataFormats.FileDrop));

			foreach (string s in sourceString)
				playlist.Add(new FileInfo(s));

			AddMediaToListView();
		}
		//
		// Controls
		//
		private void ButtonOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog OpenDialog = new OpenFileDialog();
			OpenDialog.Title = "Open media";

			OpenDialog.Multiselect = true;
			OpenDialog.AddExtension = true;
			OpenDialog.Filter = "Media files (*.mp3, *.wma, *.wav, *.avi, *.wmv, *.mpg, *.mp4)|*.mp3; *.wma; *.wav; *.avi; *.wmv; *.mpg; *.mp4";

			if ((bool)OpenDialog.ShowDialog())
			{
				playlist.Clear();
				ActiveIndex = 0;

				foreach (string fileName in OpenDialog.FileNames)
					playlist.Add(new FileInfo(fileName));

				AddMediaToListView();

				this.PlayMedia(ActiveIndex);
			}
		}
		private void ButtonPlaylist_Click(object sender, RoutedEventArgs e)
		{
			if (GridPlaylist.IsEnabled == false)
			{
				GridPlaylist.Visibility = Visibility.Visible;
				GridPlaylist.IsEnabled = true;
				ButtonPlaylist.Background = Brushes.Teal;

				AddMediaToListView();
			}
			else
			{
				GridPlaylist.Visibility = Visibility.Hidden;
				GridPlaylist.IsEnabled = false;
				ButtonPlaylist.Background = Brushes.SteelBlue;
			}
		}
		private void ListViewPlaylist_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (ListViewPlaylist.Items.Count > 0)
			{
				ActiveIndex = ListViewPlaylist.SelectedIndex;
				this.PlayMedia(ActiveIndex);
			}
		}
		private void ButtonPlayPause_Click(object sender, RoutedEventArgs e)
		{
			if (Player.Source != null)
			{
				if (IsPlaying == false)
				{
					Player.Play();
					ButtonPlayPause.OpacityMask = new ImageBrush(PauseIcon);
					IsPlaying = true;
				}
				else
				{
					Player.Pause();
					ButtonPlayPause.OpacityMask = new ImageBrush(PlayIcon);
					IsPlaying = false;
				}
			}
		}
		private void ButtonStop_Click(object sender, RoutedEventArgs e)
		{
			Player.Stop();
			IsPlaying = false;
			ButtonPlayPause.OpacityMask = new ImageBrush(PlayIcon);
		}
		private void ButtonPrevious_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			rewindCounter = 0; // Set the rewind flag, signifying the previous button is held down.
			ButtonPrevious.Background = Brushes.Teal;
		}
		private void ButtonPrevious_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (rewindCounter < 5) // If the button wasn't held down for 5 ticks, play the previous media
			{
				if (ActiveIndex > 0)
				{
					ActiveIndex--;
					this.PlayMedia(ActiveIndex);
				}
			}

			rewindCounter = -1; // Reset the rewind flag.

			Player.Play();
			ButtonPrevious.Background = Brushes.SteelBlue;
		}
		private void ButtonNext_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			fastForwardCounter = 0; // Set the fast forward flag, signifying the next button is held down.
			ButtonNext.Background = Brushes.Teal;

		}
		private void ButtonNext_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (fastForwardCounter < 5)
			{
				if (ActiveIndex < playlist.Count - 1)
				{
					ActiveIndex++;
					this.PlayMedia(ActiveIndex);
				}
			}

			fastForwardCounter = -1;

			Player.Play();
			ButtonNext.Background = Brushes.SteelBlue;
		}
		private void ButtonFullScreen_Click(object sender, RoutedEventArgs e)
		{
			if (WindowStyle != System.Windows.WindowStyle.None)
			{
				this.WindowState = System.Windows.WindowState.Maximized;
				this.WindowStyle = System.Windows.WindowStyle.None;
				ButtonFullScreen.OpacityMask = new ImageBrush(FullscreenExitIcon);
			}
			else
			{
				this.WindowState = System.Windows.WindowState.Normal;
				this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
				ButtonFullScreen.OpacityMask = new ImageBrush(FullscreenIcon);
			}
		}
		private void ButtonRepeat_Click(object sender, RoutedEventArgs e)
		{
			Repeat++;
			if (Repeat == 1) // // Repeat entire playlist
			{
				ButtonRepeat.OpacityMask = new ImageBrush(RepeatIcon);
				ButtonRepeat.Background = Brushes.Teal;
			}
			else if (Repeat == 2) // Repeat the same song
			{
				ButtonRepeat.OpacityMask = new ImageBrush(RepeatOneIcon);
				ButtonRepeat.Background = Brushes.Teal;
			}
			else
			{
				Repeat = 0;
				ButtonRepeat.OpacityMask = new ImageBrush(RepeatIcon);
				ButtonRepeat.Background = Brushes.SteelBlue;
			}
		}
		private void ButtonShuffle_Click(object sender, RoutedEventArgs e)
		{
			Shuffle = !Shuffle;

			if (Shuffle)
				ButtonShuffle.Background = Brushes.Teal;
			else
				ButtonShuffle.Background = Brushes.SteelBlue;
		}
		private void Player_MediaOpened(object sender, RoutedEventArgs e)
		{
			SliderSeek.Minimum = 0;
			SliderSeek.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
			LabelEndPosition.Content = Player.NaturalDuration.TimeSpan.ToString().Substring(0, 8);

			if (Player.HasVideo)
			{
				this.Width = Player.NaturalVideoWidth;
				this.Height = Player.NaturalVideoHeight;
			}
		}
		private void Player_MediaEnded(object sender, RoutedEventArgs e)
		{
			if (Shuffle)
			{
				Random r = new Random();
				ActiveIndex = r.Next(0, playlist.Count - 1);
				this.PlayMedia(ActiveIndex);
			}

			else if (Repeat == 0) // Play the playlist once
			{
				if (ActiveIndex < playlist.Count - 1)
				{
					ActiveIndex++;
					this.PlayMedia(ActiveIndex);
				}
				else
				{
					Player.Stop();
					ActiveIndex = 0;
					ListViewPlaylist.SelectedIndex = ActiveIndex;
				}
			}

			else if (Repeat == 1) // Repeat entire playlist
			{
				if (ActiveIndex < playlist.Count - 1)
					ActiveIndex++;
				else
					ActiveIndex = 0;

				ListViewPlaylist.SelectedIndex = ActiveIndex;
				this.PlayMedia(ActiveIndex);
			}

			else if (Repeat == 2) // Repeat the same song
			{
				ListViewPlaylist.SelectedIndex = ActiveIndex;
				this.PlayMedia(ActiveIndex);
			}
		}
		private void SliderSeek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Player.Position = TimeSpan.FromSeconds(SliderSeek.Value);
		}
		private void ButtonVolume_Click(object sender, RoutedEventArgs e)
		{
			if (!Player.IsMuted)
			{
				Player.IsMuted = true;
				ButtonVolume.OpacityMask = new ImageBrush(MuteIcon);

			}
			else
			{
				Player.IsMuted = false;
				ButtonVolume.OpacityMask = new ImageBrush(VolumeIcon);
			}
		}
		private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Player.Volume = SliderVolume.Value;
		}
		//
		// Playlist events
		//
		private void ButtonOpenPlaylist_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog OpenPlaylistDialog = new OpenFileDialog();
			OpenPlaylistDialog.Title = "Open Playlist";

			OpenPlaylistDialog.AddExtension = true;
			OpenPlaylistDialog.Filter = "Playlist files (*.wpl)|*.wpl";

			if ((bool)OpenPlaylistDialog.ShowDialog())
			{
				XmlDocument DocPlaylist = new XmlDocument();
				DocPlaylist.Load(OpenPlaylistDialog.FileName);
				XmlNodeList mediaFiles = DocPlaylist.GetElementsByTagName("media");

				playlist.Clear();
				ActiveIndex = 0;

				foreach (XmlNode media in mediaFiles)
					playlist.Add(new FileInfo(media.Attributes["src"].Value));

				if (ListViewPlaylist.IsEnabled == true)
				{
					ListViewPlaylist.Items.Clear();
					foreach (FileInfo file in playlist)
						ListViewPlaylist.Items.Add(file.Name);
				}

				this.PlayMedia(ActiveIndex);
			}
		}
		private void ButtonSavePlaylist_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog SavePlaylistDialog = new SaveFileDialog();
			SavePlaylistDialog.Title = "Save Playlist";

			SavePlaylistDialog.AddExtension = true;
			SavePlaylistDialog.Filter = "Playlist files (*.wpl)|*.wpl";

			if ((bool)SavePlaylistDialog.ShowDialog())
			{
				string playlistTitle = SavePlaylistDialog.SafeFileName;
				string savePath = SavePlaylistDialog.FileName;

				XElement meta1 = new XElement("meta");
				meta1.SetAttributeValue("name", "Generator");
				meta1.SetAttributeValue("content", "Microsoft Windows Media Player -- 12.0.7601.17514");

				XElement meta2 = new XElement("meta");
				meta2.SetAttributeValue("name", "ItemCount");
				meta2.SetAttributeValue("content", "1");

				XElement seq = new XElement("seq");

				foreach (FileInfo mediaFile in playlist)
				{
					XElement mediaElement = new XElement("media", mediaFile.Name);
					mediaElement.SetAttributeValue("src", mediaFile.FullName);
					seq.Add(mediaElement);
				}

				new XDocument(
					new XElement("smil",
						new XElement("head",
							meta1, meta2,
							new XElement("title", playlistTitle)
						),
						new XElement("body", seq)
					)
				).Save(savePath);
			}
		}
		private void ButtonClearPlaylist_Click(object sender, RoutedEventArgs e)
		{
			Player.Stop();
			Player.Source = null;

			playlist.Clear();
			ListViewPlaylist.Items.Clear();
		}
		//
		// Window size
		//
		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			GridMediaElement.Width = this.Width;
			GridMediaElement.Height = this.Height;

			Player.Width = this.Width;
			Player.Height = this.Height;

			GridMediaElement.Margin = new Thickness(0, 0, 0, 0);
			Player.Margin = new Thickness(0, 0, 0, 0);

			GridPlaylist.Margin = new Thickness(this.Width - 215, 35, 0, 0);
			GridPlaylist.Height = this.Height - 200;
			ListViewPlaylist.Height = this.Height - 240;
			GridControls.Margin = new Thickness(10, this.Height - 130, 0, 0);
			GridControls.Width = this.Width - 30;

			ButtonVolume.Margin = new Thickness(this.Width - 180, ButtonVolume.Margin.Top, 0, 0);
			SliderVolume.Margin = new Thickness(this.Width - 140, SliderVolume.Margin.Top, 0, 0);

			SliderSeek.Width = this.Width - 150;
			LabelEndPosition.Margin = new Thickness(this.Width - 90, LabelEndPosition.Margin.Top, 0, 0);
		}
		private void Window_StateChanged(object sender, EventArgs e)
		{
			this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
			this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 20;
		}
		//
		// Controls fade
		//
		private void GridMain_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{

			fadeCounter = -1;
			if (GridControls.Opacity == 0)
			{
				DoubleAnimation ToOpaqueAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.1)));
				GridControls.BeginAnimation(OpacityProperty, ToOpaqueAnimation);
			}

			if (GridPlaylist.Visibility == Visibility.Visible)
			{
				if (GridPlaylist.Opacity == 0)
				{
					DoubleAnimation ToOpaqueAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(.1)));
					GridPlaylist.BeginAnimation(OpacityProperty, ToOpaqueAnimation);
				}
			}
		}
		private void GridMain_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			fadeCounter = 0;
		}
		//
		// Helper methods
		//
		private void timer_Tick(object sender, EventArgs e)
		{
			//Seek position
			if ((Player.Source != null) && (Player.NaturalDuration.HasTimeSpan))
			{
				SliderSeek.Value = Player.Position.TotalSeconds;
				LabelCurrentPosition.Content = Player.Position.ToString().Substring(0, 8);
			}

			//Controls fade away
			if (fadeCounter >= 0) // mouse has left the Main Grid
				fadeCounter++;

			if (fadeCounter >= 50 && GridControls.Opacity == 1 && IsPlaying)
			{
				DoubleAnimation ToTransparentAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(1)));
				GridControls.BeginAnimation(OpacityProperty, ToTransparentAnimation);
				GridPlaylist.BeginAnimation(OpacityProperty, ToTransparentAnimation);
			}

			// Rewind Button
			if (rewindCounter != -1) // increment the counter if the rewind button is held down.
				rewindCounter++;

			if (rewindCounter >= 5 && SliderSeek.Value > 0) // Only rewind if the button is held down for 5 ticks.
			{
				Player.Pause();
				SliderSeek.Value -= 5;
				Player.Position =  TimeSpan.FromSeconds( SliderSeek.Value);
			}

			// Fast forward button
			if (fastForwardCounter != -1) // increment the counter if the fast forward button is held down.
				fastForwardCounter++;

			if (fastForwardCounter >= 5)
			{
				Player.Pause();
				SliderSeek.Value += 5;
				Player.Position = TimeSpan.FromSeconds(SliderSeek.Value);
			}
		}
		private void PlayMedia(int ActiveIndex)
		{
			SliderSeek.Value = 0;
			Player.Source = new System.Uri(playlist[ActiveIndex].FullName);
			Player.Play();

			IsPlaying = true;

			ButtonPlayPause.OpacityMask = new ImageBrush(PauseIcon);
		}
		private void AddMediaToListView()
		{
			if (ListViewPlaylist.IsEnabled == true)
			{
				WindowsMediaPlayer player = new WindowsMediaPlayer();
				TimeSpan MediaDuration = new TimeSpan();

				ListViewPlaylist.Items.Clear();
				foreach (FileInfo file in playlist)
				{
					IWMPMedia MediaFile = player.newMedia(file.FullName);
					MediaDuration = TimeSpan.FromSeconds(MediaFile.duration);

					ListViewPlaylist.Items.Add(new { Symbol = ">", Name = file.Name, Length = MediaDuration.ToString().Split('.')[0] });
				}
			}
		}

	} // MainWindow class
} // namespace
