using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Eve.Models;

namespace Eve {
	public static class AuthConfig {
		public static void RegisterAuth() {
			// To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
			// you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

			OAuthWebSecurity.RegisterMicrosoftClient(
				clientId: "741a3816-2c51-4b82-8e72-7b7e25240e21",
				clientSecret: "mGCRwf0s/AZbTYtiU21CokJ3glI1R8oqRwrx+nX+vVI=");

			OAuthWebSecurity.RegisterGoogleClient();

			//OAuthWebSecurity.RegisterTwitterClient(
			//    consumerKey: "",
			//    consumerSecret: "");

			//OAuthWebSecurity.RegisterFacebookClient(
			//    appId: "",
			//    appSecret: "");
		}
	}
}
