using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Eve.Diagnostics.Logging;
using EveWindowsPhone.Adapters;
using EveWindowsPhone.Modules;
using EveWindowsPhone.Pages.AdvancedSettings;
using EveWindowsPhone.ViewModels;

namespace EveWindowsPhone.Pages.Modules.Play {
	[Module("MPlay", "Play",
		"/Resources/Images/IPod.png",
		"/Pages/Modules/Play/PlayView.xaml")]
	public class PlayViewModel : NotificationObject {
		private readonly Log.LogInstance log =
			new Log.LogInstance(typeof(PlayViewModel));

		private readonly INavigationServiceFacade navigationServiceFacade;
		private readonly IIsolatedStorageServiceFacade isolatedStorageServiceFacade;
		private readonly IRelayServiceFacade relayServiceFacade;

		private bool isPlaying;
		private bool isShuffleEnabled;
		private bool isRepeatEnabled;
		private string songName;
		private string artistName;
		private int songLength;
		private int songPosition;
		private int untilSongEnd;
		private double songPositionValue;


		public PlayViewModel(INavigationServiceFacade navigationServiceFacade,
							 IIsolatedStorageServiceFacade isolatedStorageServiceFacade,
							 IRelayServiceFacade relayServiceFacade) {
			if (navigationServiceFacade == null)
				throw new ArgumentNullException("navigationServiceFacade");
			if (isolatedStorageServiceFacade == null)
				throw new ArgumentNullException("isolatedStorageServiceFacade");
			if (relayServiceFacade == null)
				throw new ArgumentNullException("relayServiceFacade");

			this.navigationServiceFacade = navigationServiceFacade;
			this.isolatedStorageServiceFacade = isolatedStorageServiceFacade;
			this.relayServiceFacade = relayServiceFacade;

			this.IsPlaying = false;
			this.IsShuffleEnabled = false;
			this.IsRepeatEnabled = false;
			this.SongName = "Unknown";
			this.ArtistName = "Unknown";
			this.SongLength = 0;
			this.SongPosition = 0;

			// TODO remove, only sample
			this.IsPlaying = false;
			this.IsShuffleEnabled = false;
			this.IsRepeatEnabled = false;
			this.SongName = "I Need A Dollar";
			this.ArtistName = "Aloe Blacc";
			this.SongLength = 258;
			this.SongPosition = 47;

			var timer = new DispatcherTimer() {
				Interval = TimeSpan.FromMilliseconds(1000)
			};
			timer.Tick += (sender, args) => 
				this.SongPosition++;
			timer.Start();
		}

		public void AdvancedSettings() {
			AdvancedSettingsView.NavigateWithIndex(this.navigationServiceFacade, 6);
		}

		#region Properties

		public bool IsPlaying {
			get { return this.isPlaying; }
			set {
				this.isPlaying = value;
				this.RaisePropertyChanged("IsPlaying");
			}
		}

		public bool IsShuffleEnabled {
			get { return this.isShuffleEnabled; }
			set {
				this.isShuffleEnabled = value;
				this.RaisePropertyChanged("IsShuffleEnabled");
			}
		}

		public bool IsRepeatEnabled {
			get { return this.isRepeatEnabled; }
			set {
				this.isRepeatEnabled = value;
				this.RaisePropertyChanged("IsRepeatEnabled");
			}
		}

		public string SongName {
			get { return this.songName; }
			set {
				this.songName = value;
				this.RaisePropertyChanged("SongName");
			}
		}

		public string ArtistName {
			get { return this.artistName; }
			set {
				this.artistName = value;
				this.RaisePropertyChanged("ArtistName");
			}
		}

		public int SongLength {
			get { return this.songLength; }
			set {
				this.songLength = value;
				this.RaisePropertyChanged("SongLength");
			}
		}

		public int SongPosition {
			get { return this.songPosition; }
			set {
				this.songPosition = value;
				this.RaisePropertyChanged("SongPosition");

				this.UntilSongEnd = this.SongLength - this.SongPosition;
				this.SongPositionValue = this.SongPosition / (double)this.SongLength;
			}
		}

		public int UntilSongEnd {
			get { return this.untilSongEnd; }
			set {
				this.untilSongEnd = value;
				this.RaisePropertyChanged("UntilSongEnd");
			}
		}

		public double SongPositionValue {
			get { return this.songPositionValue; }
			set {
				this.songPositionValue = value;
				this.RaisePropertyChanged("SongPositionValue");
			}
		}

		#endregion
	}
}
