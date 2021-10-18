// Copyright 2013 Xamarin Inc. All rights reserved

#nullable enable

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;

namespace Foundation {

	// Helper to (mostly) support NS[Mutable]Copying protocols
	public class NSZone : NonRefcountedNativeObject {
		[DllImport (Constants.FoundationLibrary)]
		static extern /* NSZone* */ IntPtr NSDefaultMallocZone ();

		[DllImport (Constants.FoundationLibrary)]
		static extern IntPtr /* NSString* */ NSZoneName (/* NSZone* */ IntPtr zone);

		[DllImport (Constants.FoundationLibrary)]
		static extern void NSSetZoneName (/* NSZone* */ IntPtr zone, /* NSString* */ IntPtr name);

		public NSZone (IntPtr handle)
			: base (handle, false)
		{
		}

		[Preserve (Conditional = true)]
		public NSZone (IntPtr handle, bool owns)
			: this (handle)
		{
			// NSZone is just an opaque pointer without reference counting, so we ignore the 'owns' parameter.
		}

		protected override void Free ()
		{
			// NSZone is just an opaque pointer without reference counting, so there's nothing to free
		}

#if !COREBUILD
		public string? Name {
			get {
				return CFString.FromHandle (NSZoneName (Handle));
			}
			set {
				var nsHandle = CFString.CreateNative (value);
				try {
					NSSetZoneName (Handle, nsHandle);
				} finally {
					CFString.ReleaseNative (nsHandle);
				}
			}
		}

		// note: Copy(NSZone) and MutableCopy(NSZone) with a nil pointer == default
		public static readonly NSZone Default = new NSZone (NSDefaultMallocZone ());
#endif
	}
}
