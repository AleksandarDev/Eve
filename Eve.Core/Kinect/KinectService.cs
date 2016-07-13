using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Eve.Core.Kinect {
	public static class KinectService {
		private static List<KinectSensor> sensors;


		public static async Task Start() {
			// Find all available sensors
			KinectService.sensors = new List<KinectSensor>();
			foreach (var potentialSensor in KinectSensor.KinectSensors) {
				if (potentialSensor.Status == KinectStatus.Connected) {
					KinectService.sensors.Add(potentialSensor);
				}
			}

			await KinectService.StartSensors();

			KinectService.IsRunning = true;
		}

		private static async Task StartSensors() {
			await Task.Run(() => {
				foreach (var kinectSensor in KinectService.sensors) {
					kinectSensor.Start();
				}
			});
		}

		public static KinectAudioSource GetAudioSource(int sensorIndex) {
			// Check if requested sensor index is in range
			if (sensorIndex < 0 || sensorIndex >= KinectService.sensors.Count)
				throw new IndexOutOfRangeException("Sensor index is out of range");

			// Check if there are devices available
			if (!KinectService.HasSensorAvailable) {
				System.Diagnostics.Debug.WriteLine("No sensors available", typeof(KinectService).Name);
				// TODO Throw exception
				return null;
			}

			var sensor = KinectService.sensors.First();
			if (sensor == null || !sensor.IsRunning) {
				System.Diagnostics.Debug.WriteLine("No valid sensors found on list!", typeof(KinectService).Name);
				// TODO Throw exception
				return null;
			}

			return sensor.AudioSource;
		}

		public static async Task Stop() {
			await Task.Run(() => {
				if (KinectService.sensors != null) {
					foreach (var kinectSensor in KinectService.sensors) {
						if (kinectSensor.IsRunning) {
							kinectSensor.AudioSource.Stop();
							kinectSensor.Stop();
						}
					}
					KinectService.sensors.Clear();
				}
			});
		}


		#region Peoperties

		public static bool IsRunning { get; private set; }
		public static bool HasSensorAvailable { get { return KinectService.IsRunning && KinectService.sensors.Any(); } }

		#endregion
	}
}
