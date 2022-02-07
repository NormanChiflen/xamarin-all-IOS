using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using ObjCRuntime;
using Foundation;
using CoreFoundation;
using OS_nw_group_descriptor=System.IntPtr;
using OS_nw_endpoint=System.IntPtr;

#if !NET
using NativeHandle = System.IntPtr;
#endif

#nullable enable

namespace Network {

#if NET
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos12.0")]
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst15.0")]
#else
	[Watch (8,0)]
	[TV (15,0)]
	[Mac (12,0)]
	[iOS (15,0)]
	[MacCatalyst (15,0)]
#endif
	public class NWMultiplexGroup : NWMulticastGroup {

		[DllImport (Constants.NetworkLibrary)]
		static extern OS_nw_group_descriptor nw_group_descriptor_create_multiplex (OS_nw_endpoint remoteEndpoint);
		
		[Preserve (Conditional = true)]
		internal NWMultiplexGroup (NativeHandle handle, bool owns) : base (handle, owns) {}

		public NWMultiplexGroup (NWEndpoint endpoint) 
			: base (nw_group_descriptor_create_multiplex (endpoint.GetCheckedHandle ()), true)  { }
	}
}
