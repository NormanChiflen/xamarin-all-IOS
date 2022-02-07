// 
// VTSession.cs: Property setting/reading
//
// Authors: Miguel de Icaza (miguel@xamarin.com)
//     
// Copyright 2014-2015 Xamarin Inc.
//

#nullable enable

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;
using CoreVideo;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace VideoToolbox {		

#if NET
	[SupportedOSPlatform ("ios8.0")]
	[SupportedOSPlatform ("tvos10.2")]
#else
	[iOS (8,0)]
	[TV (10,2)]
#endif
	public class VTSession : NativeObject {
#if !NET
		protected internal VTSession (NativeHandle handle)
			: base (handle, false)
		{
		}
#endif

		[Preserve (Conditional=true)]
		internal VTSession (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		// All of them returns OSStatus mapped to VTStatus enum

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTSessionSetProperty (IntPtr handle, IntPtr propertyKey, IntPtr value);

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTSessionCopyProperty (IntPtr handle, IntPtr propertyKey, /* CFAllocator */ IntPtr allocator, out IntPtr propertyValueOut);

		[DllImport (Constants.VideoToolboxLibrary)]
		internal extern static VTStatus VTSessionSetProperties (IntPtr handle, IntPtr propertyDictionary);

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTSessionCopySerializableProperties (IntPtr handle, /* CFAllocator */ IntPtr allocator, out IntPtr dictionaryOut);

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static VTStatus VTSessionCopySupportedPropertyDictionary (/* VTSessionRef */ IntPtr session, /* CFDictionaryRef* */ out IntPtr supportedPropertyDictionaryOut);

		public VTStatus SetProperties (VTPropertyOptions options)
		{
			if (options is null)
				throw new ArgumentNullException (nameof (options));

			return VTSessionSetProperties (Handle, options.Dictionary.Handle);
		}

		public VTStatus SetProperty (NSString propertyKey, NSObject? value)
		{
			if (propertyKey is null)
				throw new ArgumentNullException (nameof (propertyKey));

			return VTSessionSetProperty (Handle, propertyKey.Handle, value.GetHandle ());
		}

		public VTPropertyOptions? GetProperties ()
		{
			var result = VTSessionCopySerializableProperties (Handle, IntPtr.Zero, out var ret);
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;

			var dict = Runtime.GetNSObject<NSDictionary> (ret, true);
			if (dict is null)
				return null;
			return new VTPropertyOptions (dict);
		}

		public NSObject? GetProperty (NSString propertyKey)
		{
			if (propertyKey is null)
				throw new ArgumentNullException (nameof (propertyKey));

			var result = VTSessionCopyProperty (Handle, propertyKey.Handle, IntPtr.Zero, out var ret);
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject<NSObject> (ret, true);
		}

		public NSDictionary? GetSerializableProperties ()
		{
			var result = VTSessionCopySerializableProperties (Handle, IntPtr.Zero, out var ret);
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;

			return Runtime.GetNSObject<NSDictionary> (ret, true);
		}

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public NSDictionary? GetSupportedProperties ()
		{
			var result = VTSessionCopySupportedPropertyDictionary (Handle, out var ret);
			if (result != VTStatus.Ok || ret == IntPtr.Zero)
				return null;
			
			return Runtime.GetNSObject<NSDictionary> (ret, true);
		}
	}
}
