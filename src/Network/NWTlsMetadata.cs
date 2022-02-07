//
// NWTlsMetadata.cs: Bindings the Netowrk nw_protocol_metadata_t API that is an Tls.
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
	public class NWTlsMetadata : NWProtocolMetadata {

		[Preserve (Conditional = true)]
		internal NWTlsMetadata (NativeHandle handle, bool owns) : base (handle, owns) {}

		public SecProtocolMetadata SecProtocolMetadata
			=> new SecProtocolMetadata (nw_tls_copy_sec_protocol_metadata (GetCheckedHandle ()), owns: true);

	}
}
