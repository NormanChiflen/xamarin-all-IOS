//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2015 Xamarin, Inc.
//

#if !TVOS && !MONOMAC && !WATCH

using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using System.Runtime.Versioning;

#nullable enable

namespace MediaPlayer {

	// MPVolumeSettings.h
	public static class MPVolumeSettings {
#if NET
		[UnsupportedOSPlatform ("ios11.3")]
#if IOS
		[Obsolete ("Starting with ios11.3 use 'MPVolumeView' to present volume controls.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#endif
#else
		[Deprecated (PlatformName.iOS, 11,3, message: "Use 'MPVolumeView' to present volume controls.")]
#endif
		[DllImport (Constants.MediaPlayerLibrary, EntryPoint="MPVolumeSettingsAlertShow")]
		public extern static void AlertShow ();

#if NET
		[UnsupportedOSPlatform ("ios11.3")]
#if IOS
		[Obsolete ("Starting with ios11.3 use 'MPVolumeView' to present volume controls.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#endif
#else
		[Deprecated (PlatformName.iOS, 11,3, message: "Use 'MPVolumeView' to present volume controls.")]
#endif
		[DllImport (Constants.MediaPlayerLibrary, EntryPoint="MPVolumeSettingsAlertHide")]
		public extern static void AlertHide ();

		// note: sizeof (BOOL) is 1 like C, i.e. it's not a Win32 BOOL (4 bytes)
#if NET
		[UnsupportedOSPlatform ("ios11.3")]
#if IOS
		[Obsolete ("Starting with ios11.3 use 'MPVolumeView' to present volume controls.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#endif
#else
		[Deprecated (PlatformName.iOS, 11,3, message: "Use 'MPVolumeView' to present volume controls.")]
#endif
		[DllImport (Constants.MediaPlayerLibrary, EntryPoint="MPVolumeSettingsAlertIsVisible")]
		[return: MarshalAs (UnmanagedType.I1)]
		public extern static /* BOOL */ bool AlertIsVisible ();
	}
}

#endif
