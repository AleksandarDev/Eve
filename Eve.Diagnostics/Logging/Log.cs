﻿using System;
using System.Collections.Generic;

namespace Eve.Diagnostics.Logging {
	public delegate void LogEventHandler(
		Log.LogInstance instance, Log.LogMessage message);

	public static class Log {
		// TODO InvokeWatchedMethod to measure time

		private static LinkedList<LogMessage> messages = new LinkedList<LogMessage>();

		public static event LogEventHandler OnMessage;

		private static void AddMessage(Log.LogInstance instance, LogMessage message) {
			Log.messages.AddLast(message);

			if (Log.WriteToDebug && (Log.WriteToDebugLevel & message.Level) == message.Level)
				System.Diagnostics.Debug.WriteLine(message.ToString());

			if (Log.OnMessage != null)
				Log.OnMessage(instance, message);
		}

		public static void SaveToFile(string path) {
			throw new NotImplementedException();
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

		public class LogMessage {
			public Type SenderType { get; set; }
			public DateTime Time { get; set; }
			public LogLevels Level { get; set; }
			public string Message { get; set; }
			public Type ExceptionType { get; set; }
			public object Exception { get; set; }
			public int InstanceID { get; set; }


			public LogMessage(int instance) {
				this.InstanceID = instance;
				this.Time = DateTime.UtcNow;
				this.Level = LogLevels.Write;
				this.Message = String.Empty;
			}


			public override string ToString() {
				if (Level != LogLevels.Error)
					return String.Format("{0:00}:{1:00}:{2:00}.{3:000} {4}: {5}",
						this.Time.Hour, this.Time.Minute, this.Time.Second, this.Time.Millisecond,
						this.SenderType.Name, this.Message);
				else
					return String.Format("{0:00}:{1:00}:{2:00}.{3:000} {4}: {5}\n\t{6}",
						this.Time.Hour, this.Time.Minute, this.Time.Second, this.Time.Millisecond,
						this.SenderType.Name, this.Message,
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
				Log.AddMessage(this, new LogMessage(this.GetHashCode()) {
					SenderType = this.targetType,
					Level = LogLevels.Write,
					Message = String.Format(format, args)
				});

				return this;
			}

			public ILogInstance Info(string format, params object[] args) {
				Log.AddMessage(this, new LogMessage(this.GetHashCode()) {
					SenderType = this.targetType,
					Level = LogLevels.Information,
					Message = String.Format(format, args)
				});

				return this;
			}

			public ILogInstance Warn(string format, params object[] args) {
				Log.AddMessage(this, new LogMessage(this.GetHashCode()) {
					SenderType = this.targetType,
					Level = LogLevels.Warninig,
					Message = String.Format(format, args)
				});

				return this;
			}

			public ILogInstance Error<TException>(TException exception, string format, params object[] args) {
				Log.AddMessage(this, new LogMessage(this.GetHashCode()) {
					SenderType = this.targetType,
					Level = LogLevels.Error,
					Message = String.Format(format, args),
					Exception = exception,
					ExceptionType = typeof(TException)
				});

				return this;
			}

			public ILogInstance Debug(string format, params object[] args) {
				Log.AddMessage(this, new LogMessage(this.GetHashCode()) {
					SenderType = this.targetType,
					Level = LogLevels.Debug,
					Message = String.Format(format, args)
				});

				return this;
			}
		}
	}
}
