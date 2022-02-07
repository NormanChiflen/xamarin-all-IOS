// 
// UIFont.cs: Implements the managed UIFont
//
// Authors:
//   Geoff Norton.
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2009 Novell, Inc
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

using System;
using ObjCRuntime;
using Foundation;
using System.Runtime.Versioning;

namespace UIKit {
	public partial class UIFont {
		public override string ToString ()
		{
			return String.Format ("{0} {1}", Name, PointSize);
		}

#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7,0)]
#endif
		public static UIFont PreferredHeadline {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Headline);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7,0)]
#endif
		public static UIFont PreferredBody {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Body);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7,0)]
#endif
		public static UIFont PreferredSubheadline {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Subheadline);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7,0)]
#endif
		public static UIFont PreferredFootnote {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Footnote);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7,0)]
#endif
		public static UIFont PreferredCaption1 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Caption1);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7,0)]
#endif
		public static UIFont PreferredCaption2 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Caption2);
			}
		}

#if NET
		[SupportedOSPlatform ("ios9.0")]
#else
		[iOS (9,0)]
#endif
		public static UIFont PreferredTitle1 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title1);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios9.0")]
#else
		[iOS (9,0)]
#endif
		public static UIFont PreferredTitle2 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title2);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios9.0")]
#else
		[iOS (9,0)]
#endif
		public static UIFont PreferredTitle3 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title3);
			}
		}
		
#if NET
		[SupportedOSPlatform ("ios9.0")]
#else
		[iOS (9,0)]
#endif
		public static UIFont PreferredCallout {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Callout);
			}
		}

		static nfloat GetFontWeight (UIFontWeight weight)
		{
			switch (weight) {
			case UIFontWeight.UltraLight:
				return UIFontWeightConstants.UltraLight;
			case UIFontWeight.Thin:
				return UIFontWeightConstants.Thin;
			case UIFontWeight.Light:
				return UIFontWeightConstants.Light;
			case UIFontWeight.Regular:
				return UIFontWeightConstants.Regular;
			case UIFontWeight.Medium:
				return UIFontWeightConstants.Medium;
			case UIFontWeight.Semibold:
				return UIFontWeightConstants.Semibold;
			case UIFontWeight.Bold:
				return UIFontWeightConstants.Bold;
			case UIFontWeight.Heavy:
				return UIFontWeightConstants.Heavy;
			case UIFontWeight.Black:
				return UIFontWeightConstants.Black;
			default:
				throw new ArgumentException (weight.ToString ());
			}
		}

#if NET
		[SupportedOSPlatform ("ios8.2")]
#else
		[iOS (8,2)]
#endif
		public static UIFont SystemFontOfSize (nfloat size, UIFontWeight weight)
		{
			return SystemFontOfSize (size, GetFontWeight (weight));
		}

#if NET
		[SupportedOSPlatform ("ios9.0")]
#else
		[iOS (9,0)]
#endif
		public static UIFont MonospacedDigitSystemFontOfSize (nfloat size, nfloat weight)
		{
			var ptr = _MonospacedDigitSystemFontOfSize (size, weight);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

#if NET
		[SupportedOSPlatform ("ios9.0")]
#else
		[iOS (9,0)]
#endif
		public static UIFont MonospacedDigitSystemFontOfSize (nfloat fontSize, UIFontWeight weight)
		{
			return MonospacedDigitSystemFontOfSize (fontSize, GetFontWeight (weight));
		}

#if NET
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
#else
		[iOS (13,0)]
		[TV (13,0)]
#endif
		public static UIFont GetMonospacedSystemFont (nfloat size, nfloat weight)
		{
			var ptr = _MonospacedSystemFontOfSize (size, weight);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

#if NET
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
#else
		[iOS (13,0)]
		[TV (13,0)]
#endif
		public static UIFont GetMonospacedSystemFont (nfloat size, UIFontWeight weight) => GetMonospacedSystemFont (size, GetFontWeight (weight));

		// In this case we want to _always_ return a different managed instance
		// so one can be disposed without affecting others
		// ref: https://bugzilla.xamarin.com/show_bug.cgi?id=25511

#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7, 0)]
#endif
		public static UIFont GetPreferredFontForTextStyle (NSString uiFontTextStyle)
		{
			var ptr = _GetPreferredFontForTextStyle (uiFontTextStyle);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7, 0)]
#endif
		public static UIFont GetPreferredFontForTextStyle (UIFontTextStyle uiFontTextStyle)
		{
			return GetPreferredFontForTextStyle (uiFontTextStyle.GetConstant ());
		}

#if !WATCH
#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7, 0)]
#endif
		public static UIFont GetPreferredFontForTextStyle (NSString uiFontTextStyle, UITraitCollection traitCollection)
		{
			var ptr = _GetPreferredFontForTextStyle (uiFontTextStyle, traitCollection);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7, 0)]
#endif
		public static UIFont GetPreferredFontForTextStyle (UIFontTextStyle uiFontTextStyle, UITraitCollection traitCollection)
		{
			return GetPreferredFontForTextStyle (uiFontTextStyle.GetConstant (), traitCollection);
		}
#endif

#if NET
		[SupportedOSPlatform ("ios7.0")]
#else
		[iOS (7, 0)]
#endif
		public static UIFont FromDescriptor (UIFontDescriptor descriptor, nfloat pointSize)
		{
			var ptr = _FromDescriptor (descriptor, pointSize);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		public static UIFont FromName (string name, nfloat size)
		{
			var ptr = _FromName (name, size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		public static UIFont SystemFontOfSize (nfloat size)
		{
			var ptr = _SystemFontOfSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

#if NET
		[SupportedOSPlatform ("ios8.2")]
#else
		[iOS (8,2)]
#endif
		public static UIFont SystemFontOfSize (nfloat size, nfloat weight)
		{
			var ptr = _SystemFontOfSize (size, weight);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		public static UIFont BoldSystemFontOfSize (nfloat size)
		{
			var ptr = _BoldSystemFontOfSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		public static UIFont ItalicSystemFontOfSize (nfloat size)
		{
			var ptr = _ItalicSystemFontOfSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		public virtual UIFont WithSize (nfloat size)
		{
			var ptr = _WithSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		public static bool operator == (UIFont f1, UIFont f2)
		{
			if (((object) f1) == null)
				return ((object) f2) == null;
			else if ((object) f2 == null)
				return false;
			return f1.Handle == f2.Handle;
		}

		public static bool operator != (UIFont f1, UIFont f2)
		{
			return !(f1 == f2);
		}

		public override bool Equals (object obj)
		{
			UIFont font = (obj as UIFont);
			return this == font;
		}

		public override int GetHashCode ()
		{
			return GetNativeHash ().GetHashCode ();
		}
	}
}
