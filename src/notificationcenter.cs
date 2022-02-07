using System;
using System.ComponentModel;
using CoreGraphics;
using ObjCRuntime;
using Foundation;

#if !MONOMAC
using UIKit;
#else
using AppKit;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace NotificationCenter {
	[iOS (8,0)][Mac (10,10)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // not meant to be user created
	[Deprecated (PlatformName.iOS, 14,0)]
	[Deprecated (PlatformName.MacOSX, 11,0)]
	interface NCWidgetController {

		[Static]
		[Export ("widgetController")]
		NCWidgetController GetWidgetController();

		[Export ("setHasContent:forWidgetWithBundleIdentifier:")]
		void SetHasContent (bool flag, string bundleID);
	}

	[iOS (8,0)][Mac (10,10)]
	[Deprecated (PlatformName.iOS, 14,0)]
	[Deprecated (PlatformName.MacOSX, 11,0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface NCWidgetProviding {

		[Export ("widgetPerformUpdateWithCompletionHandler:")]
		void WidgetPerformUpdate(Action<NCUpdateResult> completionHandler);

		[Export ("widgetMarginInsetsForProposedMarginInsets:"), DelegateName ("NCWidgetProvidingMarginInsets"), DefaultValueFromArgument ("defaultMarginInsets")]
#if !MONOMAC
		[Deprecated (PlatformName.iOS, 10,0)]
		UIEdgeInsets GetWidgetMarginInsets (UIEdgeInsets defaultMarginInsets);
#else
		NSEdgeInsets GetWidgetMarginInsets (NSEdgeInsets defaultMarginInsets);
#endif

#if MONOMAC
		[Export ("widgetAllowsEditing")]
		bool WidgetAllowsEditing {
			get;
#if !NET
			[NotImplemented]
			set;
#endif
		}

		[Export ("widgetDidBeginEditing")]
		void WidgetDidBeginEditing ();

		[Export ("widgetDidEndEditing")]
		void WidgetDidEndEditing ();
#else
		[iOS (10,0)]
		[Export ("widgetActiveDisplayModeDidChange:withMaximumSize:")]
		void WidgetActiveDisplayModeDidChange (NCWidgetDisplayMode activeDisplayMode, CGSize maxSize);
#endif
	}

#if !MONOMAC
	[iOS (8,0)]
	[BaseType (typeof (UIVibrancyEffect))]
#if NET
	[Internal]
	[Category]
#else
	[Category (allowStaticMembers: true)] // Classic isn't internal so we need this
#endif
	interface UIVibrancyEffect_NotificationCenter {
		[Internal]
		[Deprecated (PlatformName.iOS, 10,0, message: "Use 'UIVibrancyEffect.GetWidgetEffect' instead.")]
		[Static, Export ("notificationCenterVibrancyEffect")]
		UIVibrancyEffect NotificationCenterVibrancyEffect ();
	}

	[Deprecated (PlatformName.iOS, 14,0)]
	[Category]
	[BaseType (typeof (NSExtensionContext))]
	interface NSExtensionContext_NCWidgetAdditions {
		[iOS (10,0)]
		[Export ("widgetLargestAvailableDisplayMode")]
		NCWidgetDisplayMode GetWidgetLargestAvailableDisplayMode ();

		[iOS (10,0)]
		[Export ("setWidgetLargestAvailableDisplayMode:")]
		void SetWidgetLargestAvailableDisplayMode (NCWidgetDisplayMode mode);

		[iOS (10,0)]
		[Export ("widgetActiveDisplayMode")]
		NCWidgetDisplayMode GetWidgetActiveDisplayMode ();

		[iOS (10,0)]
		[Export ("widgetMaximumSizeForDisplayMode:")]
		CGSize GetWidgetMaximumSize (NCWidgetDisplayMode displayMode);
	}

	[Category]
	[Internal] // only static methods, which are not _nice_ to use as extension methods
	[Deprecated (PlatformName.iOS, 14,0)]
	[BaseType (typeof (UIVibrancyEffect))]
	interface UIVibrancyEffect_NCWidgetAdditions {
		[iOS (10,0)]
		[Deprecated (PlatformName.iOS, 13,0, message: "Use 'UIVibrancyEffect.GetWidgetEffect' instead.")]
		[Static]
		[Export ("widgetPrimaryVibrancyEffect")]
		UIVibrancyEffect GetWidgetPrimaryVibrancyEffect ();

		[iOS (10,0)]
		[Deprecated (PlatformName.iOS, 13,0, message: "Use 'UIVibrancyEffect.GetWidgetEffect' instead.")]
		[Static]
		[Export ("widgetSecondaryVibrancyEffect")]
		UIVibrancyEffect GetWidgetSecondaryVibrancyEffect ();

		[iOS (13,0)]
		[Static]
		[Export ("widgetEffectForVibrancyStyle:")]
		UIVibrancyEffect GetWidgetEffect (UIVibrancyEffectStyle vibrancyStyle);
	}
#endif

#if MONOMAC
	[Mac (10,10)]
	[Deprecated (PlatformName.MacOSX, 11,0)]
	[BaseType (typeof(NSViewController), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NCWidgetListViewDelegate)})]
	interface NCWidgetListViewController
	{
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);
		
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		INCWidgetListViewDelegate Delegate { get; set; }

		[Export ("contents", ArgumentSemantic.Copy)]
		NSViewController[] Contents { get; set; }

		[Export ("minimumVisibleRowCount", ArgumentSemantic.Assign)]
		nuint MinimumVisibleRowCount { get; set; }

		[Export ("hasDividerLines")]
		bool HasDividerLines { get; set; }

		[Export ("editing")]
		bool Editing { get; set; }

		[Export ("showsAddButtonWhenEditing")]
		bool ShowsAddButtonWhenEditing { get; set; }

		[Export ("viewControllerAtRow:makeIfNecessary:")]
		NSViewController GetViewController (nuint row, bool makeIfNecesary);

		[Export ("rowForViewController:")]
		nuint GetRow (NSViewController viewController);
	}

	interface INCWidgetListViewDelegate {}

	[Mac (10, 10)]
	[Deprecated (PlatformName.MacOSX, 11,0)]
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface NCWidgetListViewDelegate
	{
		[Abstract]
		[Export ("widgetList:viewControllerForRow:"), DelegateName ("NCWidgetListViewGetController"), DefaultValue (null)]
		NSViewController GetViewControllerForRow (NCWidgetListViewController list, nuint row);

		[Export ("widgetListPerformAddAction:"), DelegateName ("NCWidgetListViewController")]
		void PerformAddAction (NCWidgetListViewController list);

		[Export ("widgetList:shouldReorderRow:"), DelegateName ("NCWidgetListViewControllerShouldReorderRow"), DefaultValue (false)]
		bool ShouldReorderRow (NCWidgetListViewController list, nuint row);

		[Export ("widgetList:didReorderRow:toRow:"), EventArgs ("NCWidgetListViewControllerDidReorder"), DefaultValue (false)]
		void DidReorderRow (NCWidgetListViewController list, nuint row, nuint newIndex);

		[Export ("widgetList:shouldRemoveRow:"), DelegateName ("NCWidgetListViewControllerShouldRemoveRow"), DefaultValue (false)]
		bool ShouldRemoveRow (NCWidgetListViewController list, nuint row);

		[Export ("widgetList:didRemoveRow:"), EventArgs ("NCWidgetListViewControllerDidRemoveRow"), DefaultValue (false)]
		void DidRemoveRow (NCWidgetListViewController list, nuint row);
	}

	[Mac (10,10)]
	[Deprecated (PlatformName.MacOSX, 11,0)]
	[BaseType (typeof(NSViewController), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NCWidgetSearchViewDelegate)})]
	interface NCWidgetSearchViewController
	{
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		INCWidgetSearchViewDelegate Delegate { get; set; }

		[NullAllowed]
		[Export ("searchResults", ArgumentSemantic.Copy)]
		NSObject[] SearchResults { get; set; }
	
		[NullAllowed]
		[Export ("searchDescription")]
		string SearchDescription { get; set; }

		[NullAllowed]
		[Export ("searchResultsPlaceholderString")]
		string SearchResultsPlaceholderString { get; set; }

		[NullAllowed]
		[Export ("searchResultKeyPath")]
		string SearchResultKeyPath { get; set; }
	}

	interface INCWidgetSearchViewDelegate {}

	[Mac (10,10)]
	[Deprecated (PlatformName.MacOSX, 11,0)]
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface NCWidgetSearchViewDelegate
	{
#if !NET
		[Abstract]
		[Export ("widgetSearch:searchForTerm:maxResults:"), EventArgs ("NSWidgetSearchForTerm"), DefaultValue (false)]
		void SearchForTearm (NCWidgetSearchViewController controller, string searchTerm, nuint max);
#else
		[Abstract]
		[Export ("widgetSearch:searchForTerm:maxResults:"), EventArgs ("NSWidgetSearchForTerm"), DefaultValue (false)]
		void SearchForTerm (NCWidgetSearchViewController controller, string searchTerm, nuint max);
#endif

		[Abstract]
		[Export ("widgetSearchTermCleared:"), EventArgs ("NSWidgetSearchViewController"), DefaultValue (false)]
		void TermCleared (NCWidgetSearchViewController controller);

		[Abstract]
		[Export ("widgetSearch:resultSelected:"), EventArgs ("NSWidgetSearchResultSelected"), DefaultValue (false)]
		void ResultSelected (NCWidgetSearchViewController controller, NSObject obj);
	}
#endif
}
