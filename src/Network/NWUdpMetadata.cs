//
// NWUdpMetadata.cs: Bindings the Netowrk nw_protocol_metadata_t API that is an Udp.
//
// Authors:
//   Manuel de la Pena <mandel@microsoft.com>
//
// Copyrigh 2019 Microsoft
//

#nullable enable

using System;
using ObjCRuntime;
using Foundation;
using Security;
using CoreFoundation;
using System.Runtime.Versioning;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Network {

#if NET
	[SupportedOSPlatform ("tvos12.0")]
	[SupportedOSPlatform ("macos10.14")]
	[SupportedOSPlatform ("ios12.0")]
#else
	[TV (12,0)]
	[Mac (10,14)]
	[iOS (12,0)]
	[Watch (6,0)]
#endif
	public class NWUdpMetadata : NWProtocolMetadata {

		[Preserve (Conditional = true)]
		internal NWUdpMetadata (NativeHandle handle, bool owns) : base (handle, owns) {}

		public NWUdpMetadata () : this (nw_udp_create_metadata (), owns: true) {}
	}
}
