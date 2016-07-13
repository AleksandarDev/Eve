using System.ServiceProcess;

namespace EveServices {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main() {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] {
                new EveAPIService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
