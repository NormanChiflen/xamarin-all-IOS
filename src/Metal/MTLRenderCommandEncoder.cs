using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace Metal {
	public static class IMTLRenderCommandEncoder_Extensions {
#if MONOMAC
#if NET
		[SupportedOSPlatform ("macos10.13")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[Mac (10,13)]
		[NoiOS]
		[NoTV]
		[NoWatch]
#endif
		public unsafe static void SetViewports (this IMTLRenderCommandEncoder This, MTLViewport [] viewports)
		{
			fixed (void* handle = viewports)
				This.SetViewports ((IntPtr)handle, (nuint)(viewports?.Length ?? 0));
		}

#if NET
		[SupportedOSPlatform ("macos10.13")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
#else
		[Mac (10,13)]
		[NoiOS]
		[NoTV]
		[NoWatch]
#endif
		public unsafe static void SetScissorRects (this IMTLRenderCommandEncoder This, MTLScissorRect [] scissorRects)
		{
			fixed (void* handle = scissorRects)
				This.SetScissorRects ((IntPtr)handle, (nuint)(scissorRects?.Length ?? 0));
		}
#endif

#if IOS
#if NET
		[SupportedOSPlatform ("ios11.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
#else
		[iOS (11,0)]
		[NoTV]
		[NoMac]
		[NoWatch]
#endif
		public unsafe static void SetTileBuffers (this IMTLRenderCommandEncoder This, IMTLBuffer[] buffers, nuint[] offsets, NSRange range)
		{
			fixed (void* handle = offsets)
				This.SetTileBuffers (buffers, (IntPtr)handle, range);
		}

#if NET
		[SupportedOSPlatform ("ios11.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
#else
		[iOS (11,0)]
		[NoTV]
		[NoMac]
		[NoWatch]
#endif
		public unsafe static void SetTileSamplerStates (this IMTLRenderCommandEncoder This, IMTLSamplerState[] samplers, float[] lodMinClamps, float[] lodMaxClamps, NSRange range)
		{
			fixed (void* minHandle = lodMinClamps) {
				fixed (void* maxHandle = lodMaxClamps) {
					This.SetTileSamplerStates (samplers, (IntPtr)minHandle, (IntPtr)maxHandle, range);
				}
			}
		}
#endif
	}
}
