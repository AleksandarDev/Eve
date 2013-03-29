using System.Runtime.Serialization;

namespace Eve.API.Services.Contracts.Services {
	[DataContract]
	public class ServiceUser {
		public const string InvalidToken = "[InvalidToken]";


		public ServiceUser(string userName, string passwordHash) {
			this.UserName = userName;
			this.PasswordHash = passwordHash;

			this.Token = ServiceUser.InvalidToken;
		}


		public void Validate() {
			ServiceUser.ValidateUser(this);
		}

		public static void ValidateUser(ServiceUser user) {
			// TODO Check for password hash and username match
			user.IsValid = true;
			user.Token = "TestToken";
		}


		#region Properties

		[DataMember]
		public string UserName { get; private set; }

		[DataMember]
		public string PasswordHash { get; private set; }

		[DataMember]
		public bool IsValid { get; private set; }

		[DataMember]
		public string Token { get; private set; }

		public bool IsSignedIn {
			get { return this.Token != ServiceUser.InvalidToken; }
		}

		#endregion
	}
}
