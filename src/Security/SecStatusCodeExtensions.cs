// 
// SecStatusCodeExtensions.cs
//
// Authors:
//	Alex Soto (alexsoto@microsoft.com)
// 
// Copyright 2018 Xamarin Inc.
//

using System;
using System.Runtime.InteropServices;
using ObjCRuntime;
using Foundation;
using System.Runtime.Versioning;

namespace Security {
	public static class SecStatusCodeExtensions {

#if !NET
		[iOS (11,3), TV (11,3), Watch (4,3)]
#else
		[SupportedOSPlatform ("ios11.3")]
		[SupportedOSPlatform ("tvos11.3")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static /* CFStringRef */ IntPtr SecCopyErrorMessageString (
			/* OSStatus */ SecStatusCode status,
			/* void * */ IntPtr reserved); /* always null */

#if !NET
		[iOS (11,3), TV (11,3), Watch (4,3)] // Since Mac 10,3
#else
		[SupportedOSPlatform ("ios11.3")]
		[SupportedOSPlatform ("tvos11.3")]
#endif
		public static string GetStatusDescription (this SecStatusCode status)
		{
			var ret = SecCopyErrorMessageString (status, IntPtr.Zero);
			return Runtime.GetNSObject<NSString> (ret, owns: true);
		}
	}
}
