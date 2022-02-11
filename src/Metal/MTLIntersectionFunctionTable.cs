#if !TVOS
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace Metal {

	// add some extension methods to make the API of the protocol nicer
	public static class MTLIntersectionFunctionTableExtensions {

#if NET
		[SupportedOSPlatform ("macos11.0")]
		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[Mac (11,0)]
		[iOS (14,0)]
		[NoTV]
#endif
		public static void SetBuffers (this IMTLIntersectionFunctionTable table, IMTLBuffer[] buffers, nuint[] offsets, NSRange range)
		{
			if (buffers == null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffers));
			if (offsets == null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (offsets));

			var bufferPtrArray = buffers.Length <= 1024 ? stackalloc IntPtr[buffers.Length] : new IntPtr [buffers.Length];
			// get all intptr from the array to pass to the lower level call
			for (var i = 0; i < buffers.Length; i++) {
				bufferPtrArray [i] = buffers [i].Handle;
			}

			unsafe {
				fixed (void* buffersPtr = bufferPtrArray)
				fixed (void* offsetsPtr = offsets) { // can use fixed
					table.SetBuffers ((IntPtr) buffersPtr, (IntPtr) offsetsPtr, range);
				}
			}
			GC.KeepAlive (buffers);
		}
	}
}
#endif
