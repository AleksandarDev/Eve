using System;
using System.Runtime.Serialization;

namespace Eve.Core {
	public class EveException : Exception {
		public EveException() : base("An exception occured!") { }
		public EveException(string message) : base(message) { }
		public EveException(string message, Exception innerException) : base(message, innerException) {}
		public EveException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}