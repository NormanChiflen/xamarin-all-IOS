// Copyright 2011 - 2014 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
#if !NO_SYSTEM_DRAWING
using System.Drawing;
#endif

#if NET
using System.Runtime.InteropServices.ObjectiveC;
#endif

using ObjCRuntime;
#if !COREBUILD
using Xamarin.Bundler;
#if MONOTOUCH
using UIKit;
#if !WATCH
using CoreAnimation;
#endif
#endif
using CoreGraphics;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Foundation {

#if NET
	public enum NSObjectFlag {
		Empty,
	}
#else
	public class NSObjectFlag {
		public static readonly NSObjectFlag Empty;
		
		NSObjectFlag () {}
	}
#endif

#if NET && !COREBUILD
	[ObjectiveCTrackedType]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public partial class NSObject 
#if !COREBUILD
		: IEquatable<NSObject> 
#endif
	{
#if !COREBUILD
		const string selConformsToProtocol = "conformsToProtocol:";
		const string selEncodeWithCoder = "encodeWithCoder:";

#if MONOMAC
		static IntPtr selConformsToProtocolHandle = Selector.GetHandle (selConformsToProtocol);
		static IntPtr selEncodeWithCoderHandle = Selector.GetHandle (selEncodeWithCoder);
#endif

		// replace older Mono[Touch|Mac]Assembly field (ease code sharing across platforms)
		public static readonly Assembly PlatformAssembly = typeof (NSObject).Assembly;

		NativeHandle handle;
		IntPtr super; /* objc_super* */

#if !NET
		Flags flags;
#else
		// See  "Toggle-ref support for CoreCLR" in coreclr-bridge.m for more information.
		Flags actual_flags;
		internal unsafe Runtime.TrackedObjectInfo* tracked_object_info;
		internal GCHandle? tracked_object_handle;

		unsafe Flags flags {
			get {
				// Get back the InFinalizerQueue flag, it's the only flag we'll set in the tracked object info structure.
				// The InFinalizerQueue will never be cleared once set, so there's no need to unset it here if it's not set in the tracked_object_info structure.
				if (tracked_object_info != null && ((tracked_object_info->Flags) & Flags.InFinalizerQueue) == Flags.InFinalizerQueue)
					actual_flags |= Flags.InFinalizerQueue;

				return actual_flags;
			}
			set {
				actual_flags = value;
				// Update the flags value that we can access them from the toggle ref callback as well.
				if (tracked_object_info != null)
					tracked_object_info->Flags = value;
			}
		}
#endif // NET

		// This enum has a native counterpart in runtime.h
		[Flags]
		internal enum Flags : byte {
			Disposed = 1,
			NativeRef = 2,
			IsDirectBinding = 4,
			RegisteredToggleRef = 8,
			InFinalizerQueue = 16,
			HasManagedRef = 32,
			// 64, // Used by SoM
			IsCustomType = 128,
		}

		// Must be kept in sync with the same enum in trampolines.h
		enum XamarinGCHandleFlags : uint {
			None = 0,
			WeakGCHandle = 1,
			HasManagedRef = 2,
			InitialSet = 4,
		}

		[StructLayout (LayoutKind.Sequential)]
		struct objc_super {
			public IntPtr Handle;
			public IntPtr ClassHandle;
		}

		bool disposed { 
			get { return ((flags & Flags.Disposed) == Flags.Disposed); } 
			set { flags = value ? (flags | Flags.Disposed) : (flags & ~Flags.Disposed);	}
		}

		bool HasManagedRef {
			get { return (flags & Flags.HasManagedRef) == Flags.HasManagedRef; }
			set { flags = value ? (flags | Flags.HasManagedRef) : (flags & ~Flags.HasManagedRef); }
		}

		internal bool IsRegisteredToggleRef { 
			get { return ((flags & Flags.RegisteredToggleRef) == Flags.RegisteredToggleRef); } 
			set { flags = value ? (flags | Flags.RegisteredToggleRef) : (flags & ~Flags.RegisteredToggleRef);	}
		}

		protected internal bool IsDirectBinding {
			get { return ((flags & Flags.IsDirectBinding) == Flags.IsDirectBinding); }
			set { flags = value ? (flags | Flags.IsDirectBinding) : (flags & ~Flags.IsDirectBinding); }
		}

		internal bool InFinalizerQueue {
			get { return ((flags & Flags.InFinalizerQueue) == Flags.InFinalizerQueue); }
		}

		bool IsCustomType {
			get {
				var value = (flags & Flags.IsCustomType) == Flags.IsCustomType;
				if (!value) {
					value = Class.IsCustomType (GetType ());
					if (value)
						flags |= Flags.IsCustomType;
				}
				return value;
			}
		}

		[Export ("init")]
		public NSObject () {
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}
		
		// This is just here as a constructor chain that can will
		// only do Init at the most derived class.
		public NSObject (NSObjectFlag x)
		{
			bool alloced = AllocIfNeeded ();
			InitializeObject (alloced);
		}
		
#if NET
		protected internal NSObject (NativeHandle handle)
#else
		public NSObject (NativeHandle handle)
#endif
			: this (handle, false)
		{
		}
		
#if NET
		protected NSObject (NativeHandle handle, bool alloced)
#else
		public NSObject (NativeHandle handle, bool alloced)
#endif
		{
			this.handle = handle;
			InitializeObject (alloced);
		}
		
		~NSObject () {
			Dispose (false);
		}
		
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		internal static IntPtr CreateNSObject (IntPtr type_gchandle, IntPtr handle, Flags flags)
		{
			// This function is called from native code before any constructors have executed.
			var type = (Type) Runtime.GetGCHandleTarget (type_gchandle);
			try {
				var obj = (NSObject) RuntimeHelpers.GetUninitializedObject (type);
				obj.handle = handle;
				obj.flags = flags;
				return Runtime.AllocGCHandle (obj);
			} catch (Exception e) {
				throw ErrorHelper.CreateError (8041, e, Errors.MX8041 /* Unable to create an instance of the type {0} */, type.FullName);
			}
		}

		IntPtr GetSuper ()
		{
			if (super == IntPtr.Zero) {
				IntPtr ptr;

				unsafe {
					ptr = Marshal.AllocHGlobal (sizeof (objc_super));
					*(objc_super*) ptr = default (objc_super); // zero fill
				}

				var previousValue = Interlocked.CompareExchange (ref super, ptr, IntPtr.Zero);
				if (previousValue != IntPtr.Zero) {
					// somebody beat us to the assignment.
					Marshal.FreeHGlobal (ptr);
					ptr = IntPtr.Zero;
				}
			}

			unsafe {
				objc_super* sup = (objc_super*) super;
				if (sup->ClassHandle == IntPtr.Zero)
					sup->ClassHandle = ClassHandle;
				sup->Handle = handle;
			}

			return super;
		}

		internal static NativeHandle Initialize ()
		{
			return class_ptr;
		}

#if NET
		internal Flags FlagsInternal {
			get { return flags; }
			set { flags = value; }
		}
#endif

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		extern static void RegisterToggleRef (NSObject obj, IntPtr handle, bool isCustomType);

		[DllImport ("__Internal")]
		static extern void xamarin_release_managed_ref (IntPtr handle, [MarshalAs (UnmanagedType.I1)] bool user_type);

#if NET
		static void RegisterToggleRefMonoVM (NSObject obj, IntPtr handle, bool isCustomType)
		{
			// We need this indirection for CoreCLR, otherwise JITting RegisterToggleReference will throw System.Security.SecurityException: ECall methods must be packaged into a system module.
			RegisterToggleRef (obj, handle, isCustomType);
		}
#endif

		static void RegisterToggleReference (NSObject obj, IntPtr handle, bool isCustomType)
		{
#if NET
			if (Runtime.IsCoreCLR) {
				Runtime.RegisterToggleReferenceCoreCLR (obj, handle, isCustomType);
			} else {
				RegisterToggleRefMonoVM (obj, handle, isCustomType);
			}
#else
			RegisterToggleRef (obj, handle, isCustomType);
#endif
		}

#if !XAMCORE_3_0
		public static bool IsNewRefcountEnabled ()
		{
			return true;
		}
#endif

		/*
		Register the current object with the toggleref machinery if the following conditions are met:
		-The new refcounting is enabled; and
		-The class is not a custom type - it must wrap a framework class.
		*/
		protected void MarkDirty () {
			MarkDirty (false);
		}
			
		internal void MarkDirty (bool allowCustomTypes)
		{
			if (IsRegisteredToggleRef)
				return;

			if (!allowCustomTypes && IsCustomType)
				return;
			
			IsRegisteredToggleRef = true;
			RegisterToggleReference (this, Handle, allowCustomTypes);
		}

		private void InitializeObject (bool alloced) {
			if (alloced && handle == IntPtr.Zero && Class.ThrowOnInitFailure) {
				if (ClassHandle == IntPtr.Zero)
					throw new Exception (string.Format ("Could not create an native instance of the type '{0}': the native class hasn't been loaded.\n" +
						"It is possible to ignore this condition by setting ObjCRuntime.Class.ThrowOnInitFailure to false.",
						GetType ().FullName));
				throw new Exception (string.Format ("Failed to create a instance of the native type '{0}'.\n" +
					"It is possible to ignore this condition by setting ObjCRuntime.Class.ThrowOnInitFailure to false.",
					new Class (ClassHandle).Name));
			}

			// The authorative value for the IsDirectBinding value is the register attribute:
			//
			//     [Register ("MyClass", true)] // the second parameter specifies the IsDirectBinding value
			//     class MyClass : NSObject {}
			//
			// Unfortunately looking up this attribute every time a class is instantiated is
			// slow (since fetching attributes is slow), so we guess here: if the actual type
			// of the object is in the platform assembly, then we assume IsDirectBinding=true:
			//
			// IsDirectBinding = (this.GetType ().Assembly == PlatformAssembly);
			//
			// and any subclasses in the platform assembly which is not a direct binding have
			// to set the correct value in their constructors.
			IsDirectBinding = (this.GetType ().Assembly == PlatformAssembly);
			Runtime.RegisterNSObject (this, handle);

			bool native_ref = (flags & Flags.NativeRef) == Flags.NativeRef;
			CreateManagedRef (!alloced || native_ref);
		}

		[DllImport ("__Internal")]
		[return: MarshalAs (UnmanagedType.I1)]
		static extern bool xamarin_set_gchandle_with_flags_safe (IntPtr handle, IntPtr gchandle, XamarinGCHandleFlags flags);

		void CreateManagedRef (bool retain)
		{
			HasManagedRef = true;
			bool isUserType = Runtime.IsUserType (handle);
			if (isUserType) {
				var flags = XamarinGCHandleFlags.HasManagedRef | XamarinGCHandleFlags.InitialSet | XamarinGCHandleFlags.WeakGCHandle;
				var gchandle = GCHandle.Alloc (this, GCHandleType.WeakTrackResurrection);
				var h = GCHandle.ToIntPtr (gchandle);
				if (!xamarin_set_gchandle_with_flags_safe (handle, h, flags)) {
					// A GCHandle already existed: this shouldn't happen, but let's handle it anyway.
					Runtime.NSLog ("Tried to create a managed reference from an object that already has a managed reference (type: {0})", GetType ());
					gchandle.Free ();
				}
			}

			if (retain)
				DangerousRetain ();
		}

		void ReleaseManagedRef ()
		{
			var handle = this.Handle; // Get a copy of the handle, because it will be cleared out when calling Runtime.NativeObjectHasDied, and we still need the handle later.
			var user_type = Runtime.IsUserType (handle);
			HasManagedRef = false;
			if (!user_type) {
				/* If we're a wrapper type, we need to unregister here, since we won't enter the release trampoline */
				Runtime.NativeObjectHasDied (handle, this);
			}
			xamarin_release_managed_ref (handle, user_type);
			FreeData ();
#if NET
			if (tracked_object_handle.HasValue) {
				tracked_object_handle.Value.Free ();
				tracked_object_handle = null;
			}
#endif
		}

		static bool IsProtocol (Type type, IntPtr protocol)
		{
			while (type != typeof (NSObject) && type != null) {
				var attrs = type.GetCustomAttributes (typeof(ProtocolAttribute), false);
				var protocolAttribute = (ProtocolAttribute) (attrs.Length > 0 ? attrs [0] : null);
				if (protocolAttribute != null && !protocolAttribute.IsInformal) {
					string name;

					if (!string.IsNullOrEmpty (protocolAttribute.Name)) {
						name = protocolAttribute.Name;
					} else {
						attrs = type.GetCustomAttributes (typeof(RegisterAttribute), false);
						var registerAttribute = (RegisterAttribute) (attrs.Length > 0 ? attrs [0] : null);
						if (registerAttribute != null && !string.IsNullOrEmpty (registerAttribute.Name)) {
							name = registerAttribute.Name;
						} else {
							name = type.Name;
						}
					}

					var proto = Runtime.GetProtocol (name);
					if (proto != IntPtr.Zero && proto == protocol)
						return true;
				}
				type = type.BaseType;
			}

			return false;
		}

		[Preserve]
		bool InvokeConformsToProtocol (NativeHandle protocol)
		{
			return ConformsToProtocol (protocol);
		}

		[Export ("conformsToProtocol:")]
		[Preserve ()]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public virtual bool ConformsToProtocol (NativeHandle protocol)
		{
			bool does;
			bool is_wrapper = IsDirectBinding;
			bool is_third_party;

			if (is_wrapper) {
				is_third_party = this.GetType ().Assembly != NSObject.PlatformAssembly;
				if (is_third_party) {
					// Third-party bindings might lie about IsDirectBinding (see bug #14772),
					// so don't trust any 'true' values unless we're in monotouch.dll.
					var attribs = this.GetType ().GetCustomAttributes (typeof(RegisterAttribute), false);
					if (attribs != null && attribs.Length == 1)
						is_wrapper = ((RegisterAttribute) attribs [0]).IsWrapper;
				}
			}

#if MONOMAC
			if (is_wrapper) {
				does = Messaging.bool_objc_msgSend_IntPtr (this.Handle, selConformsToProtocolHandle, protocol);
			} else {
				does = Messaging.bool_objc_msgSendSuper_IntPtr (this.SuperHandle, selConformsToProtocolHandle, protocol);
			}
#else
			if (is_wrapper) {
				does = Messaging.bool_objc_msgSend_IntPtr (this.Handle, Selector.GetHandle (selConformsToProtocol), protocol);
			} else {
				does = Messaging.bool_objc_msgSendSuper_IntPtr (this.SuperHandle, Selector.GetHandle (selConformsToProtocol), protocol);
			}
#endif

			if (does)
				return true;
			
			if (!Runtime.DynamicRegistrationSupported)
				return false;

			object [] adoptedProtocols = GetType ().GetCustomAttributes (typeof (AdoptsAttribute), true);
			foreach (AdoptsAttribute adopts in adoptedProtocols){
				if (adopts.ProtocolHandle == protocol)
					return true;
			}

			// Check if this class or any of the interfaces
			// it implements are protocols.

			if (IsProtocol (GetType (), protocol))
				return true;

			var ifaces = GetType ().GetInterfaces ();
			foreach (var iface in ifaces) {
				if (IsProtocol (iface, protocol))
					return true;
			}

			return false;
		}

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public void DangerousRelease ()
		{
			DangerousRelease (handle);
		}

		internal static void DangerousRelease (NativeHandle handle)
		{
			if (handle == IntPtr.Zero)
				return;
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.ReleaseHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Release));
#endif
		}

		internal static void DangerousRetain (NativeHandle handle)
		{
			if (handle == IntPtr.Zero)
				return;
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.RetainHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Retain));
#endif
		}
			
		internal static void DangerousAutorelease (NativeHandle handle)
		{
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.AutoreleaseHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Autorelease));
#endif
		}

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public NSObject DangerousRetain ()
		{
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.RetainHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Retain));
#endif
			return this;
		}

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public NSObject DangerousAutorelease ()
		{
#if MONOMAC
			Messaging.void_objc_msgSend (handle, Selector.AutoreleaseHandle);
#else
			Messaging.void_objc_msgSend (handle, Selector.GetHandle (Selector.Autorelease));
#endif
			return this;
		}

		public IntPtr SuperHandle {
			get {
				if (handle == IntPtr.Zero)
					throw new ObjectDisposedException (GetType ().Name);

				return GetSuper ();
			}
		}
		
		public NativeHandle Handle {
			get { return handle; }
			set {
				if (handle == value)
					return;
				
				if (handle != IntPtr.Zero)
					Runtime.UnregisterNSObject (handle);
				
				handle = value;

#if NET
				unsafe {
					if (tracked_object_info != null)
						tracked_object_info->Handle = value;
				}
#endif

				if (handle != IntPtr.Zero)
					Runtime.RegisterNSObject (this, handle);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected void InitializeHandle (NativeHandle handle)
		{
			InitializeHandle (handle, "init*");
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		protected void InitializeHandle (NativeHandle handle, string initSelector)
		{
			if (this.handle == IntPtr.Zero && Class.ThrowOnInitFailure) {
				if (ClassHandle == IntPtr.Zero)
					throw new Exception (string.Format ("Could not create an native instance of the type '{0}': the native class hasn't been loaded.\n" +
						"It is possible to ignore this condition by setting ObjCRuntime.Class.ThrowOnInitFailure to false.",
						GetType ().FullName));
				throw new Exception (string.Format ("Failed to create a instance of the native type '{0}'.\n" +
					"It is possible to ignore this condition by setting ObjCRuntime.Class.ThrowOnInitFailure to false.",
					new Class (ClassHandle).Name));
			}

			if (handle == IntPtr.Zero && Class.ThrowOnInitFailure) {
				Handle = IntPtr.Zero; // We'll crash if we don't do this.
				throw new Exception (string.Format ("Could not initialize an instance of the type '{0}': the native '{1}' method returned nil.\n" +
				"It is possible to ignore this condition by setting ObjCRuntime.Class.ThrowOnInitFailure to false.",
					GetType ().FullName, initSelector));
			}

			this.Handle = handle;
		}
		
		private bool AllocIfNeeded () {
			if (handle == IntPtr.Zero) {
#if MONOMAC
				handle = Messaging.IntPtr_objc_msgSend (Class.GetHandle (this.GetType ()), Selector.AllocHandle);
#else
				handle = Messaging.IntPtr_objc_msgSend (Class.GetHandle (this.GetType ()), Selector.GetHandle (Selector.Alloc));
#endif
				return true;
			}
			return false;
		}
		
#if !XAMCORE_3_0
		private IntPtr GetObjCIvar (string name) {
			IntPtr native;
			
			object_getInstanceVariable (handle, name, out native);
			
			return native;
		}
		
		[Obsolete ("Do not use; this API does not properly retain/release existing/new values, so leaks and/or crashes may occur.")]
		public NSObject GetNativeField (string name) {
			IntPtr field = GetObjCIvar (name);
			
			if (field == IntPtr.Zero)
				return null;
			return Runtime.GetNSObject (field);
		}
		
		private void SetObjCIvar (string name, IntPtr value) {
			object_setInstanceVariable (handle, name, value);
		}
		
		[Obsolete ("Do not use; this API does not properly retain/release existing/new values, so leaks and/or crashes may occur.")]
		public void SetNativeField (string name, NSObject value) {
			if (value == null)
				SetObjCIvar (name, IntPtr.Zero);
			else
				SetObjCIvar (name, value.Handle);
		}
		
		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_getInstanceVariable (IntPtr obj, string name, out IntPtr val);

		[DllImport ("/usr/lib/libobjc.dylib")]
		extern static void object_setInstanceVariable (IntPtr obj, string name, IntPtr val);
#endif // !XAMCORE_3_0

		private void InvokeOnMainThread (Selector sel, NSObject obj, bool wait)
		{
#if NET
			Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (this.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), sel.Handle, obj.GetHandle (), wait);
#else
#if MONOMAC
			Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (this.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, sel.Handle, obj.GetHandle (), wait);
#else
			Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (this.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), sel.Handle, obj.GetHandle (), wait);
#endif
#endif
		}
		
		public void BeginInvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, false);
		}
		
		public void InvokeOnMainThread (Selector sel, NSObject obj)
		{
			InvokeOnMainThread (sel, obj, true);
		}
		
		public void BeginInvokeOnMainThread (Action action)
		{
			var d = new NSAsyncActionDispatcher (action);
#if NET
			Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
		                                                        NSDispatcher.Selector.Handle, d.Handle, false);
#else
#if MONOMAC
			Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, 
		                                                        NSDispatcher.Selector.Handle, d.Handle, false);
#else
			Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), 
			                                                Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, false);
#endif
#endif
		}

		internal void BeginInvokeOnMainThread (System.Threading.SendOrPostCallback cb, object state)
		{
			var d = new NSAsyncSynchronizationContextDispatcher (cb, state);
#if NET
			Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
			                                                Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, false);
#else
#if MONOMAC
			Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle,
		                                                        NSDispatcher.Selector.Handle, d.Handle, false);
#else
			Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
			                                                Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, false);
#endif
#endif
		}
		
		public void InvokeOnMainThread (Action action)
		{
			using (var d = new NSActionDispatcher (action)) {
#if NET
				Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), 
				                                                Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, true);
#else
#if MONOMAC
				Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, 
		                                                                NSDispatcher.Selector.Handle, d.Handle, true);
#else
				Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), 
				                                                Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, true);
#endif
#endif
			}
		}		

		internal void InvokeOnMainThread (System.Threading.SendOrPostCallback cb, object state)
		{
			using (var d = new NSSynchronizationContextDispatcher (cb, state)) {
#if NET
				Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
				                                                Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, true);
#else
#if MONOMAC
				Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle,
			                                                        NSDispatcher.Selector.Handle, d.Handle, true);
#else
				Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (d.Handle, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone),
				                                                Selector.GetHandle (NSDispatcher.SelectorName), d.Handle, true);
#endif
#endif
			}
		}

		public static NSObject FromObject (object obj)
		{
			if (obj == null)
				return NSNull.Null;
			var t = obj.GetType ();
			if (t == typeof (NSObject) || t.IsSubclassOf (typeof (NSObject)))
				return (NSObject) obj;
			
			switch (Type.GetTypeCode (t)){
			case TypeCode.Boolean:
				return new NSNumber ((bool) obj);
			case TypeCode.Char:
				return new NSNumber ((ushort) (char) obj);
			case TypeCode.SByte:
				return new NSNumber ((sbyte) obj);
			case TypeCode.Byte:
				return new NSNumber ((byte) obj);
			case TypeCode.Int16:
				return new NSNumber ((short) obj);
			case TypeCode.UInt16:
				return new NSNumber ((ushort) obj);
			case TypeCode.Int32:
				return new NSNumber ((int) obj);
			case TypeCode.UInt32:
				return new NSNumber ((uint) obj);
			case TypeCode.Int64:
				return new NSNumber ((long) obj);
			case TypeCode.UInt64:
				return new NSNumber ((ulong) obj);
			case TypeCode.Single:
				return new NSNumber ((float) obj);
			case TypeCode.Double:
				return new NSNumber ((double) obj);
			case TypeCode.String:
				return new NSString ((string) obj);
			default:
#if NET
				if (t == typeof (NativeHandle))
					return NSValue.ValueFromPointer ((NativeHandle) obj);
#else
				if (t == typeof (IntPtr))
					return NSValue.ValueFromPointer ((IntPtr) obj);
#endif
#if !NO_SYSTEM_DRAWING
				if (t == typeof (SizeF))
					return NSValue.FromSizeF ((SizeF) obj);
				else if (t == typeof (RectangleF))
					return NSValue.FromRectangleF ((RectangleF) obj);
				else if (t == typeof (PointF))
					return NSValue.FromPointF ((PointF) obj);
#endif
				if (t == typeof (nint))
					return NSNumber.FromNInt ((nint) obj);
				else if (t == typeof (nuint))
					return NSNumber.FromNUInt ((nuint) obj);
				else if (t == typeof (nfloat))
					return NSNumber.FromNFloat ((nfloat) obj);
				else if (t == typeof (CGSize))
					return NSValue.FromCGSize ((CGSize) obj);
				else if (t == typeof (CGRect))
					return NSValue.FromCGRect ((CGRect) obj);
				else if (t == typeof (CGPoint))
					return NSValue.FromCGPoint ((CGPoint) obj);

#if !MONOMAC
				if (t == typeof (CGAffineTransform))
					return NSValue.FromCGAffineTransform ((CGAffineTransform) obj);
				else if (t == typeof (UIEdgeInsets))
					return NSValue.FromUIEdgeInsets ((UIEdgeInsets) obj);
#if !WATCH
				else if (t == typeof (CATransform3D))
					return NSValue.FromCATransform3D ((CATransform3D) obj);
#endif
#endif
				// last chance for types like CGPath, CGColor... that are not NSObject but are CFObject
				// see https://bugzilla.xamarin.com/show_bug.cgi?id=8458
				INativeObject native = (obj as INativeObject);
				if (native != null)
					return Runtime.GetNSObject (native.Handle);
				return null;
			}
		}

		public void SetValueForKeyPath (IntPtr handle, NSString keyPath)
		{
			if (keyPath == null)
				throw new ArgumentNullException ("keyPath");
#if NET
			if (IsDirectBinding) {
				ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle_NativeHandle (this.Handle, Selector.GetHandle ("setValue:forKeyPath:"), handle, keyPath.Handle);
			} else {
				ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle_NativeHandle (this.SuperHandle, Selector.GetHandle ("setValue:forKeyPath:"), handle, keyPath.Handle);
			}
#else
#if MONOMAC
			if (IsDirectBinding) {
				ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, selSetValue_ForKeyPath_Handle, handle, keyPath.Handle);
			} else {
				ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, selSetValue_ForKeyPath_Handle, handle, keyPath.Handle);
			}
#else
			if (IsDirectBinding) {
				ObjCRuntime.Messaging.void_objc_msgSend_IntPtr_IntPtr (this.Handle, Selector.GetHandle ("setValue:forKeyPath:"), handle, keyPath.Handle);
			} else {
				ObjCRuntime.Messaging.void_objc_msgSendSuper_IntPtr_IntPtr (this.SuperHandle, Selector.GetHandle ("setValue:forKeyPath:"), handle, keyPath.Handle);
			}
#endif
#endif
		}

		// if IsDirectBinding is false then we _likely_ have managed state and it's up to the subclass to provide
		// a correct implementation of GetHashCode / Equals. We default to Object.GetHashCode (like classic)

		public override int GetHashCode ()
		{
			if (!IsDirectBinding)
				return base.GetHashCode ();
			// Hash is nuint so 64 bits, and Int32.GetHashCode == same Int32
			return GetNativeHash ().GetHashCode ();
		}

		public override bool Equals (object obj)
		{
			var o = obj as NSObject;
			if (o == null)
				return false;
				
			bool isDirectBinding = IsDirectBinding;
			// is only one is a direct binding then both cannot be equals
			if (isDirectBinding != o.IsDirectBinding)
				return false;

			// we can only ask `isEqual:` to test equality if both objects are direct bindings
			return isDirectBinding ? IsEqual (o) : ReferenceEquals (this, o);
		}

		// IEquatable<T>
		public bool Equals (NSObject obj) => Equals ((object) obj);

		public override string ToString ()
		{
			if (disposed)
				return base.ToString ();
			return Description ?? base.ToString ();
		}

		public virtual void Invoke (Action action, double delay)
		{
			var d = new NSAsyncActionDispatcher (action);
			d.PerformSelector (NSDispatcher.Selector, null, delay);
		}

		public virtual void Invoke (Action action, TimeSpan delay)
		{
			var d = new NSAsyncActionDispatcher (action);
			d.PerformSelector (NSDispatcher.Selector, null, delay.TotalSeconds);
		}

		internal void ClearHandle ()
		{
			handle = IntPtr.Zero;
		}

		protected virtual void Dispose (bool disposing) {
			if (disposed)
				return;
			disposed = true;
			
			if (handle != IntPtr.Zero) {
				if (disposing) {
					ReleaseManagedRef ();
				} else {
#if NET
					// By adding an external reference to the object from finalizer we will
					// resurrect it. Since Runtime class tracks the NSObject instances with
					// GCHandle(..., WeakTrackResurrection) we need to make sure it's aware
					// that the object was finalized.
					//
					// On CoreCLR the non-tracked objects don't get a callback from the
					// garbage collector when they enter the finalization queue but the
					// information is necessary for Runtime.TryGetNSObject to work correctly. 
					// Since we are on the finalizer thread now we can just set the flag
					// directly here.
					actual_flags |= Flags.InFinalizerQueue;
#endif

					NSObject_Disposer.Add (this);
				}
			} else {
				FreeData ();
			}
		}

		unsafe void FreeData ()
		{
			if (super != IntPtr.Zero) {
				Marshal.FreeHGlobal (super);
				super = IntPtr.Zero;
			}
		}

		[Register ("__NSObject_Disposer")]
		[Preserve (AllMembers=true)]
		internal class NSObject_Disposer : NSObject {
			static readonly List <NSObject> drainList1 = new List<NSObject> ();
			static readonly List <NSObject> drainList2 = new List<NSObject> ();
			static List <NSObject> handles = drainList1;

			static readonly IntPtr class_ptr = Class.GetHandle ("__NSObject_Disposer");
#if MONOMAC
			static readonly IntPtr drainHandle = Selector.GetHandle ("drain:");
#endif
			
			static readonly object lock_obj = new object ();
			
			private NSObject_Disposer ()
			{
				// Disable default ctor, there should be no instances of this class.
			}
			
			static internal void Add (NSObject handle) {
				bool call_drain;
				lock (lock_obj) {
					handles.Add (handle);
					call_drain = handles.Count == 1;
				}
				if (!call_drain)
					return;
#if NET
				Messaging.void_objc_msgSend_NativeHandle_NativeHandle_bool (class_ptr, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), Selector.GetHandle ("drain:"), IntPtr.Zero, false);
#else
#if MONOMAC
				Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (class_ptr, Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDoneHandle, drainHandle, IntPtr.Zero, false);
#else
				Messaging.void_objc_msgSend_IntPtr_IntPtr_bool (class_ptr, Selector.GetHandle (Selector.PerformSelectorOnMainThreadWithObjectWaitUntilDone), Selector.GetHandle ("drain:"), IntPtr.Zero, false);
#endif
#endif
			}
			
			[Export ("drain:")]
			static  void Drain (NSObject ctx) {
				List<NSObject> drainList;
				
				lock (lock_obj) {
					drainList = handles;
					if (handles == drainList1)
						handles = drainList2;
					else
						handles = drainList1;
				}
				
				foreach (NSObject x in drainList)
					x.ReleaseManagedRef ();
				drainList.Clear();
			}
		}
			
		[Register ("__XamarinObjectObserver")]
		class Observer : NSObject {
			WeakReference obj;
			Action<NSObservedChange> cback;
			NSString key;
			
			public Observer (NSObject obj, NSString key, Action<NSObservedChange> observer)
			{
				if (observer == null)
					throw new ArgumentNullException (nameof(observer));

				this.obj = new WeakReference (obj);
				this.key = key;
				this.cback = observer;
				IsDirectBinding = false;
			}

			[Preserve (Conditional = true)]
			public override void ObserveValue (NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
			{
				if (keyPath == key && context == Handle)
					cback (new NSObservedChange (change));
				else
					base.ObserveValue (keyPath, ofObject, change, context);
			}

			protected override void Dispose (bool disposing)
			{
				if (disposing) {
					NSObject target;
					if (obj != null) {
						target = (NSObject) obj.Target;
						if (target != null)
							target.RemoveObserver (this, key, Handle);
					}
					obj = null;
					cback = null;
				} else {
					Runtime.NSLog ("Warning: observer object was not disposed manually with Dispose()");
				}
				base.Dispose (disposing);
			}
		}
		
		public IDisposable AddObserver (string key, NSKeyValueObservingOptions options, Action<NSObservedChange> observer)
		{
			return AddObserver (new NSString (key), options, observer);
		}

		public IDisposable AddObserver (NSString key, NSKeyValueObservingOptions options, Action<NSObservedChange> observer)
		{
			var o = new Observer (this, key, observer);
			AddObserver (o, key, options, o.Handle);
			return o;
		}
#endif // !COREBUILD
	}

#if !COREBUILD
	public class NSObservedChange {
		NSDictionary dict;
		public NSObservedChange (NSDictionary source)
		{
			dict = source;
		}

		public NSKeyValueChange Change {
			get {
				var n = (NSNumber) dict [NSObject.ChangeKindKey];
				return (NSKeyValueChange) n.Int32Value;
			}
		}

		public NSObject NewValue {
			get {
				return dict [NSObject.ChangeNewKey];
			}
		}

		public NSObject OldValue {
			get {
				return dict [NSObject.ChangeOldKey];
			}
		}

		public NSIndexSet Indexes {
			get {
				return (NSIndexSet) dict [NSObject.ChangeIndexesKey];
			}
		}

		public bool IsPrior { 
			get {
				var n = dict [NSObject.ChangeNotificationIsPriorKey] as NSNumber;
				if (n == null)
					return false;
				return n.BoolValue;
			}  
		}
	}
#endif
}
