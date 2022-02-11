//
// MTKTextureLoaderOptions.cs strong dictionary
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.Runtime.Versioning;
using Foundation;
using Metal;
using ObjCRuntime;

namespace MetalKit {
#if !COREBUILD

#if NET
	[SupportedOSPlatform ("ios9.0")]
	[SupportedOSPlatform ("macos10.11")]
#else
	[iOS (9,0)]
	[Mac (10,11)]
#endif
	public partial class MTKTextureLoaderOptions : DictionaryContainer {

		public MTLTextureUsage? TextureUsage {
			get {
				var val = GetNUIntValue (MTKTextureLoaderKeys.TextureUsageKey);
				if (val != null)
					return (MTLTextureUsage)(uint) val;
				return null;
			}
			set {
				if (value.HasValue)
					SetNumberValue (MTKTextureLoaderKeys.TextureUsageKey, (nuint)(uint)value.Value);
				else
					RemoveValue (MTKTextureLoaderKeys.TextureUsageKey);
			}
		}

		public MTLCpuCacheMode? TextureCpuCacheMode {
			get {
				var val = GetNUIntValue (MTKTextureLoaderKeys.TextureCpuCacheModeKey);
				if (val != null)
					return (MTLCpuCacheMode)(uint) val;
				return null;
			}
			set {
				if (value.HasValue)
					SetNumberValue (MTKTextureLoaderKeys.TextureCpuCacheModeKey, (nuint)(uint)value.Value);
				else
					RemoveValue (MTKTextureLoaderKeys.TextureCpuCacheModeKey);
			}
		}

#if NET
		[SupportedOSPlatform ("ios10.0")]
		[SupportedOSPlatform ("macos10.12")]
#else
		[iOS (10,0)]
		[Mac (10,12)]
#endif
		public MTLStorageMode? TextureStorageMode {
			get {
				var val = GetNUIntValue (MTKTextureLoaderKeys.TextureStorageModeKey);
				if (val != null)
					return (MTLStorageMode)(uint) val;
				return null;
			}
			set {
				if (value.HasValue)
					SetNumberValue (MTKTextureLoaderKeys.TextureStorageModeKey, (nuint)(uint)value.Value);
				else
					RemoveValue (MTKTextureLoaderKeys.TextureStorageModeKey);
			}
		}

#if NET
		[SupportedOSPlatform ("ios10.0")]
		[SupportedOSPlatform ("macos10.12")]
#else
		[iOS (10,0)]
		[Mac (10,12)]
#endif
		public MTKTextureLoaderCubeLayout? CubeLayout {
			get {
				var val = GetNSStringValue (MTKTextureLoaderKeys.CubeLayoutKey);
				if (val == null)
					return null;
				return MTKTextureLoaderCubeLayoutExtensions.GetValue (val);
			}
			set {
				if (value.HasValue)
					SetStringValue (MTKTextureLoaderKeys.CubeLayoutKey, value.Value.GetConstant ());
				else
					RemoveValue (MTKTextureLoaderKeys.CubeLayoutKey);
			}
		}

#if NET
		[SupportedOSPlatform ("ios10.0")]
		[SupportedOSPlatform ("macos10.12")]
#else
		[iOS (10,0)]
		[Mac (10,12)]
#endif
		public MTKTextureLoaderOrigin? Origin {
			get {
				var val = GetNSStringValue (MTKTextureLoaderKeys.OriginKey);
				if (val == null)
					return null;
				return MTKTextureLoaderOriginExtensions.GetValue (val);
			}
			set {
				if (value.HasValue)
					SetStringValue (MTKTextureLoaderKeys.OriginKey, value.Value.GetConstant ());
				else
					RemoveValue (MTKTextureLoaderKeys.OriginKey);
			}
		}
	}
#endif
}
