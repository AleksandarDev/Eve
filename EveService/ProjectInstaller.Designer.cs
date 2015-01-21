namespace EveServices
{
	partial class ProjectInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.serviceProcessInstallerGlobal = new System.ServiceProcess.ServiceProcessInstaller();
			this.serviceInstallerForEveAPIService = new System.ServiceProcess.ServiceInstaller();
			// 
			// serviceProcessInstallerGlobal
			// 
			this.serviceProcessInstallerGlobal.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.serviceProcessInstallerGlobal.Password = null;
			this.serviceProcessInstallerGlobal.Username = null;
			// 
			// serviceInstallerForEveAPIService
			// 
			this.serviceInstallerForEveAPIService.ServiceName = "EveAPIService";
			this.serviceInstallerForEveAPIService.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerGlobal,
            this.serviceInstallerForEveAPIService});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerGlobal;
		private System.ServiceProcess.ServiceInstaller serviceInstallerForEveAPIService;
	}
}