﻿using System.ServiceModel;

namespace Eve.API.Services.Contracts.Services.Interfaces {
	public interface IRelayCallbackContract {
		[OperationContract(IsOneWay = true)]
		void PingRequest(string message);
	}
}