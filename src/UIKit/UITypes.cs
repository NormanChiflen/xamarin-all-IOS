//
// UITypes.cs: Various types defined in UIKit
//
// Authors:
//   Miguel de Icaza
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2016 Xamarin Inc.
//
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;
using CoreGraphics;

namespace UIKit {

	[StructLayout (LayoutKind.Sequential)]
	public struct UIEdgeInsets {

		// API match for UIEdgeInsetsZero field/constant
		[Field ("UIEdgeInsetsZero")] // fake (but helps testing and could also help documentation)
		public static readonly UIEdgeInsets Zero;

		public nfloat Top, Left, Bottom, Right;

#if !COREBUILD
		public UIEdgeInsets (nfloat top, nfloat left, nfloat bottom, nfloat right)
		{
			Top = top;
			Left = left;
			Bottom = bottom;
			Right = right;
		}

		// note: UIEdgeInsetsInsetRect (UIGeometry.h) is a macro
		public CGRect InsetRect (CGRect rect)
		{
			return new CGRect (rect.X + Left,
			                       rect.Y + Top,
			                       rect.Width - Left - Right,
			                       rect.Height - Top - Bottom);
		}

		// note: UIEdgeInsetsEqualToEdgeInsets (UIGeometry.h) is a macro
		public bool Equals (UIEdgeInsets other)
		{
			if (Left != other.Left)
				return false;
			if (Right != other.Right)
				return false;
			if (Top != other.Top)
				return false;
			return (Bottom == other.Bottom);
		}

		public override bool Equals (object obj)
		{
			if (obj is UIEdgeInsets)
				return Equals ((UIEdgeInsets) obj);
			return false;
		}

		public static bool operator == (UIEdgeInsets insets1, UIEdgeInsets insets2)
		{
			return insets1.Equals (insets2);
		}

		public static bool operator != (UIEdgeInsets insets1, UIEdgeInsets insets2)
		{
			return !insets1.Equals (insets2);
		}

		public override int GetHashCode ()
		{
			return Top.GetHashCode () ^ Left.GetHashCode () ^ Right.GetHashCode () ^ Bottom.GetHashCode ();
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static UIEdgeInsets UIEdgeInsetsFromString (IntPtr /* NSString */ s);

		static public UIEdgeInsets FromString (string s)
		{
			// note: null is allowed
			var ptr = NSString.CreateNative (s);
			var value = UIEdgeInsetsFromString (ptr);
			NSString.ReleaseNative (ptr);
			return value;
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static IntPtr /* NSString */ NSStringFromUIEdgeInsets (UIEdgeInsets insets);

		// note: ensure we can roundtrip ToString into FromString
		public override string ToString ()
		{
			using (var ns = new NSString (NSStringFromUIEdgeInsets (this)))
				return ns.ToString ();
		}
#endif
	}

#if !WATCH
#if NET
	[SupportedOSPlatform ("ios9.0")]
#else
	[iOS (9,0)]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct UIFloatRange : IEquatable<UIFloatRange> {

		public nfloat Minimum, Maximum;
		
		public UIFloatRange (nfloat minimum, nfloat maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		[DllImport (Constants.UIKitLibrary)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool UIFloatRangeIsInfinite (UIFloatRange range);

		public bool IsInfinite {
			get {
				return UIFloatRangeIsInfinite (this);
			}
		}

		// Got replaced by a macro in iOS 13 (Xcode 11)...
		// [DllImport (Constants.UIKitLibrary)]
		// static extern bool UIFloatRangeIsEqualToRange (UIFloatRange range, UIFloatRange otherRange);

		public bool Equals (UIFloatRange other) => this.Minimum == other.Minimum && this.Maximum == other.Maximum;

		public override bool Equals (object other)
		{
			if (other is UIFloatRange)
				return Equals ((UIFloatRange) other);
			return false;
		}

		public override int GetHashCode ()
		{
			return Minimum.GetHashCode () ^ Maximum.GetHashCode ();
		}

		[Field ("UIFloatRangeZero")] // fake (but helps testing and could also help documentation)
		public static UIFloatRange Zero;

		[Field ("UIFloatRangeInfinite")] // fake (but helps testing and could also help documentation)
		public static UIFloatRange Infinite = new UIFloatRange (nfloat.NegativeInfinity, nfloat.PositiveInfinity);
	}
#endif

#if IOS || __MACCATALYST__
#if NET
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst15.0")]
#else
	[Introduced (PlatformName.iOS, 15,0)]
	[Introduced (PlatformName.MacCatalyst, 15,0)]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct UIPointerAccessoryPosition {
		public nfloat Offset, Angle;

		public UIPointerAccessoryPosition (nfloat offset, nfloat angle)
		{
			Offset = offset;
			Angle = angle;
		}

#if !COREBUILD
		[Field ("UIPointerAccessoryPositionTop", "UIKit")]
		public static UIPointerAccessoryPosition Top => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTop"), typeof (UIPointerAccessoryPosition))!;

		[Field ("UIPointerAccessoryPositionTopRight", "UIKit")]
		public static UIPointerAccessoryPosition TopRight => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTopRight"), typeof (UIPointerAccessoryPosition))!;

		[Field ("UIPointerAccessoryPositionRight", "UIKit")]
		public static UIPointerAccessoryPosition Right => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionRight"), typeof (UIPointerAccessoryPosition))!;

		[Field ("UIPointerAccessoryPositionBottomRight", "UIKit")]
		public static UIPointerAccessoryPosition BottomRight => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottomRight"), typeof (UIPointerAccessoryPosition))!;

		[Field ("UIPointerAccessoryPositionBottom", "UIKit")]
		public static UIPointerAccessoryPosition Bottom => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottom"), typeof (UIPointerAccessoryPosition))!;

		[Field ("UIPointerAccessoryPositionBottomLeft", "UIKit")]
		public static UIPointerAccessoryPosition BottomLeft => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottomLeft"), typeof (UIPointerAccessoryPosition))!;

		[Field ("UIPointerAccessoryPositionLeft", "UIKit")]
		public static UIPointerAccessoryPosition Left => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionLeft"), typeof (UIPointerAccessoryPosition))!;

		[Field ("UIPointerAccessoryPositionTopLeft", "UIKit")]
		public static UIPointerAccessoryPosition TopLeft => (UIPointerAccessoryPosition) Marshal.PtrToStructure (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTopLeft"), typeof (UIPointerAccessoryPosition))!;
#endif
	}
#endif

#if false
	[Protocol]
	public interface IUITextInputTraits {
		[Export ("autocapitalizationType")]
		UITextAutocapitalizationType AutocapitalizationType { get; set; }

		[Export ("autocorrectionType")]
		UITextAutocorrectionType AutocorrectionType { get; set; }

		[Export ("keyboardType")]
		UIKeyboardType KeyboardType { get; set; }

		[Export ("keyboardAppearance")]
		UIKeyboardAppearance KeyboardAppearance { get; set; }

		[Export ("returnKeyType")]
		UIReturnKeyType ReturnKeyType { get; set; }

		[Export ("enablesReturnKeyAutomatically")]
		bool EnablesReturnKeyAutomatically { get; set; } 

		[Export ("secureTextEntry")]
		bool SecureTextEntry { get; set; } 
	}
#endif
}
