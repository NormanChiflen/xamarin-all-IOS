#nullable enable

using System;

using Microsoft.Build.Framework;

using Xamarin.Localization.MSBuild;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public abstract class ReadAppManifestTaskBase : XamarinTask {
		public ITaskItem? AppManifest { get; set; }

		[Required]
		public string? SdkVersion { get; set; }

		[Output]
		public string? CLKComplicationGroup { get; set; }

		[Output]
		public string? CFBundleDisplayName { get; set; }

		[Output]
		public string? CFBundleVersion { get; set; }

		[Output]
		public string? MinimumOSVersion { get; set; }

		[Output]
		public string? NSExtensionPointIdentifier { get; set; }

		[Output]
		public string? UIDeviceFamily { get; set; }

		[Output]
		public bool WKWatchKitApp { get; set; }

		[Output]
		public string? XSAppIconAssets { get; set; }

		[Output]
		public string? XSLaunchImageAssets { get; set; }

		public override bool Execute ()
		{
			PDictionary? plist = null;

			if (!string.IsNullOrEmpty (AppManifest?.ItemSpec)) {
				try {
					plist = PDictionary.FromFile (AppManifest!.ItemSpec);
				} catch (Exception ex) {
					Log.LogError (null, null, null, AppManifest!.ItemSpec, 0, 0, 0, 0, MSBStrings.E0010, AppManifest.ItemSpec, ex.Message);
					return false;
				}
			}

			var minimumOSVersionInManifest = plist?.Get<PString> (PlatformFrameworkHelper.GetMinimumOSVersionKey (Platform))?.Value;
			if (string.IsNullOrEmpty (minimumOSVersionInManifest)) {
				MinimumOSVersion = SdkVersion;
			} else if (!IAppleSdkVersion_Extensions.TryParse (minimumOSVersionInManifest, out var _)) {
				Log.LogError (null, null, null, AppManifest?.ItemSpec, 0, 0, 0, 0, MSBStrings.E0011, minimumOSVersionInManifest);
				return false;
			} else {
				MinimumOSVersion = minimumOSVersionInManifest;
			}

			if (Platform == ApplePlatform.MacCatalyst) {
				// Convert the min macOS version to the min iOS version, which the rest of our tooling expects.
				if (!MacCatalystSupport.TryGetiOSVersion (Sdks.GetAppleSdk (Platform).GetSdkPath (SdkVersion, false), MinimumOSVersion, out var convertedVersion))
					Log.LogError (MSBStrings.E0187, MinimumOSVersion);
				MinimumOSVersion = convertedVersion;
			}

			CFBundleDisplayName = plist?.GetCFBundleDisplayName ();
			CFBundleVersion = plist?.GetCFBundleVersion ();
			CLKComplicationGroup = plist?.Get<PString> (ManifestKeys.CLKComplicationGroup)?.Value;
			NSExtensionPointIdentifier = plist?.GetNSExtensionPointIdentifier ();
			UIDeviceFamily = plist?.GetUIDeviceFamily ().ToString ();
			WKWatchKitApp = plist?.GetWKWatchKitApp () == true;
			XSAppIconAssets = plist?.Get<PString> (ManifestKeys.XSAppIconAssets)?.Value;
			XSLaunchImageAssets = plist?.Get<PString> (ManifestKeys.XSLaunchImageAssets)?.Value;

			return true;
		}
	}
}
