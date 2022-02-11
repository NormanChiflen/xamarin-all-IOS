using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreGraphics;
using Foundation;
using Metal;
using ObjCRuntime;

namespace MetalPerformanceShaders {
	public partial class MPSNNGraph {
#if NET
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos10.15")]
		[SupportedOSPlatform ("ios13.0")]
#else
		[TV (13,0)]
		[Mac (10,15)]
		[iOS (13,0)]
#endif
		public unsafe static MPSNNGraph Create (IMTLDevice device, MPSNNImageNode[] resultImages, bool[] resultsAreNeeded)
		{
			fixed (void *resultsAreNeededHandle = resultsAreNeeded)
				return Create (device, resultImages, (IntPtr) resultsAreNeededHandle);
		}
	}
}
