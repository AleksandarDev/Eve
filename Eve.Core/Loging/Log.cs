using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eve.Core.Loging {
	public static class Log {
		private static LinkedList<LogMessage> messages = new LinkedList<LogMessage>();


		private static void AddMessage(LogMessage message) {
			Log.messages.AddLast(message);

			if (Log.WriteToDebug && (Log.WriteToDebugLevel & message.Level) == message.Level)
				System.Diagnostics.Debug.WriteLine(message.ToString());
		}

		public async static Task SaveToFile(string path) {
			var writer = new System.IO.StreamWriter(path);
			for (int index = 0; index < Log.messages.Count; index++) {
				await writer.WriteLineAsync(Log.messages.ElementAt(index).ToString());
			}
			await writer.FlushAsync();
			writer.Close();
			writer.Dispose();
		}

		#region Properties

		public static bool Enabled { get; set; }
		public static bool WriteToDebug { get; set; }
		public static LogLevels WriteToDebugLevel { get; set; }

		#endregion

		#region Object Extensions

		public static ILogInstance Write(this object @this, Type thisType,
										 string format, params object[] args) {
			return (new LogInstance(thisType)).WriteLine(format, args);
		}

		public static ILogInstance Info(this object @this, Type thisType,
								 string format, params object[] args) {
			return (new LogInstance(thisType)).Info(format, args);
		}

		public static ILogInstance Warn(this object @this, Type thisType,
								 string format, params object[] args) {
			return (new LogInstance(thisType)).Warn(format, args);
		}

		public static ILogInstance Debug(this object @this, Type thisType,
								 string format, params object[] args) {
			return (new LogInstance(thisType)).Debug(format, args);
		}

		public static ILogInstance Error<TException>(this object @this, Type thisType,
													 TException exception, string format, params object[] args) {
			return (new LogInstance(thisType)).Error(exception, format, args);
		}

		#endregion

		[Flags]
		public enum LogLevels {
			None	= 0x0,
			Error	= 0x1,
			Debug	= 0x2,
			Warninig	= 0x4,
			Information	= 0x8,
			Write	= 0x10,
			All		= Error | Debug | Warninig | Information | Write
		}

		private class LogMessage {
			public Type SenderType { get; set; }
			public DateTime Time { get; set; }
			public LogLevels Level { get; set; }
			public string Message { get; set; }
			public Type ExceptionType { get; set; }
			public object Exception { get; set; }


			public LogMessage() {
				this.Time = DateTime.UtcNow;
				this.Level = LogLevels.Write;
				this.Message = String.Empty;
			}


			public override string ToString() {
				if (Level != LogLevels.Error)
					return String.Format("{0} {1}: {2}",
										 this.Time.ToLongTimeString(), this.SenderType.Name, this.Message);
				else
					return String.Format("{0} {1}: {2}\n\t{3}",
										 this.Time.ToLongTimeString(), this.SenderType.Name, this.Message,
										 this.Exception.ToString());
			}
		}

		public interface ILogInstance {
			ILogInstance WriteLine(string format, params object[] args);
			ILogInstance Info(string format, params object[] args);
			ILogInstance Warn(string format, params object[] args);
			ILogInstance Error<TException>(TException exception, string format, params object[] args);
			ILogInstance Debug(string format, params object[] args);
		}

		public class LogInstance : ILogInstance {
			private readonly Type targetType;


			public LogInstance(Type targetType) {
				this.targetType = targetType;
			}


			public ILogInstance WriteLine(string format, params object[] args) {
				Log.AddMessage(new LogMessage() {
					SenderType = this.targetType,
					Level = LogLevels.Write,
					Message = String.Format(format, args)
				});

				return this;
			}

			public ILogInstance Info(string format, params object[] args) {
				Log.AddMessage(new LogMessage() {
					SenderType = this.targetType,
					Level = LogLevels.Information,
					Message = String.Format(format, args)
				});

				return this;
			}

			public ILogInstance Warn(string format, params object[] args) {
				Log.AddMessage(new LogMessage() {
					SenderType = this.targetType,
					Level = LogLevels.Warninig,
					Message = String.Format(format, args)
				});

				return this;
			}

			public ILogInstance Error<TException>(TException exception, string format, params object[] args) {
				Log.AddMessage(new LogMessage() {
					SenderType = this.targetType,
					Level = LogLevels.Error,
					Message = String.Format(format, args),
					Exception = exception,
					ExceptionType = typeof(TException)
				});

				return this;
			}

			public ILogInstance Debug(string format, params object[] args) {
				Log.AddMessage(new LogMessage() {
					SenderType = this.targetType,
					Level = LogLevels.Debug,
					Message = String.Format(format, args)
				});

				return this;
			}
		}
	}
}
