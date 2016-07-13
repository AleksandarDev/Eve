using System;
using System.Drawing;

namespace Eve.API.Vision {
	[ProviderDescription("Captcha Decoder Provider")]
	public class CaptchaDecoderProvider : ProviderBase<CaptchaDecoderProvider> {
		protected override void Initialize() { }
		protected override void Uninitialize() { }
		
		public string Decode(Bitmap image) {
			throw new NotImplementedException();
		}

		#region Properties

		#endregion
	}
}
