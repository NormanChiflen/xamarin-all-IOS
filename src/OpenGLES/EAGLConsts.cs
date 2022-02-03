//
// Author:
//   Jonathan Pryor:
//
// (C) 2009 Novell, Inc.
//
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Foundation;
using ObjCRuntime;

namespace OpenGLES {

#if NET
	[UnsupportedOSPlatform ("tvos12.0")]
	[UnsupportedOSPlatform ("ios12.0")]
#if TVOS
	[Obsolete ("Starting with tvos12.0 use 'Metal' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#elif IOS
	[Obsolete ("Starting with ios12.0 use 'Metal' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#endif
#else
	[Deprecated (PlatformName.iOS, 12,0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12,0, message: "Use 'Metal' instead.")]
#endif
	public static class EAGLDrawableProperty {
		public static readonly NSString ColorFormat;
		public static readonly NSString RetainedBacking;

		static EAGLDrawableProperty ()
		{
			var handle = Libraries.OpenGLES.Handle;
			ColorFormat     = Dlfcn.GetStringConstant (handle, 
					"kEAGLDrawablePropertyColorFormat");
			RetainedBacking = Dlfcn.GetStringConstant (handle, 
					"kEAGLDrawablePropertyRetainedBacking");
		}
	}

#if NET
	[UnsupportedOSPlatform ("tvos12.0")]
	[UnsupportedOSPlatform ("ios12.0")]
#if TVOS
	[Obsolete ("Starting with tvos12.0 use 'Metal' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#elif IOS
	[Obsolete ("Starting with ios12.0 use 'Metal' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#endif
#else
	[Deprecated (PlatformName.iOS, 12,0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12,0, message: "Use 'Metal' instead.")]
#endif
	public static class EAGLColorFormat {
		public static readonly NSString RGB565;
		public static readonly NSString RGBA8;

		static EAGLColorFormat ()
		{
			var handle = Libraries.OpenGLES.Handle;
			RGB565  = Dlfcn.GetStringConstant (handle, "kEAGLColorFormatRGB565");
			RGBA8   = Dlfcn.GetStringConstant (handle, "kEAGLColorFormatRGBA8");
		}
	}
}
