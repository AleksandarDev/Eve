using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Controls;
using AForge.Video;

namespace Eve.UI.Controls.VisionPlayer {
	public partial class VisionPlayerView : UserControl {
		private VideoSourcePlayer sourcePlayer;


		public VisionPlayerView() {
			InitializeComponent();

			this.PlayerViewModel = this.Resources["PlayerViewModel"] as VisionPlayerViewModel;
			if (this.PlayerViewModel == null) throw new NullReferenceException("Can't find PlayerViewModel!");

			this.sourcePlayer = new VideoSourcePlayer();
			this.WindowsFormsHost.Child = this.sourcePlayer;
		}


		public IVideoSource VideoSource {
			get { return this.sourcePlayer.VideoSource; }
			set { this.sourcePlayer.VideoSource = value; }
		}


		#region Properties

		public VisionPlayerViewModel PlayerViewModel { get; private set; }

		#endregion
	}
}
