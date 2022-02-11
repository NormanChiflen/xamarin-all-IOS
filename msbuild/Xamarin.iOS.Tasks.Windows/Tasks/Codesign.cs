using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xamarin.iOS.Tasks.Windows.Properties;
using Xamarin.iOS.Windows;

namespace Xamarin.iOS.HotRestart.Tasks {
	public class Codesign : Task, ICancelableTask {
		CancellationTokenSource cancellationSource;

		#region Inputs

		[Required]
		public string AppBundlePath { get; set; }

		[Required]
		public string BundleIdentifier { get; set; }

		[Required]
		public string CodeSigningPath { get; set; }

		[Required]
		public string ProvisioningProfilePath { get; set; }

		#endregion

		public override bool Execute ()
		{
			try {
				var hotRestartClient = new HotRestartClient ();
				var plistArgs = new Dictionary<string, string>
				{
					{ "CFBundleIdentifier", BundleIdentifier }
				};
				var password = hotRestartClient.CertificatesManager.GetCertificatePassword (certificatePath: CodeSigningPath);

				if (password == null) {
					throw new Exception (Resources.Codesign_MissingPasswordFile);
				}

				hotRestartClient.Sign (AppBundlePath, ProvisioningProfilePath, CodeSigningPath, password, plistArgs);
			} catch (WindowsiOSException ex) {
				var message = GetFullExceptionMesage (ex);

				Log.LogError (null, ex.ErrorCode, null, null, 0, 0, 0, 0, message);
			} catch (Exception ex) {
				Log.LogErrorFromException (ex);
			}

			return !Log.HasLoggedErrors;
		}

		public void Cancel () => cancellationSource?.Cancel ();

		string GetFullExceptionMesage (Exception ex)
		{
			var messageBuilder = new StringBuilder ();

			return GetFullExceptionMesage (ex, messageBuilder);
		}

		string GetFullExceptionMesage (Exception ex, StringBuilder messageBuilder)
		{
			messageBuilder.AppendLine (ex.Message);

			if (ex.InnerException != null) {
				return GetFullExceptionMesage (ex.InnerException, messageBuilder);
			} else {
				return messageBuilder.ToString ();
			}
		}
	}
}
