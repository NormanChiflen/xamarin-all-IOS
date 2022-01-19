//
// CIVector.cs: Extra methods for CIVector
//
// Copyright 2010, Novell, Inc.
// Copyright 2011, 2012 Xamarin Inc
//
// Author:
//   Miguel de Icaza
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
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreImage {
	public partial class CIVector {
		nfloat this [nint index] {
			get {
				return ValueAtIndex (index);
			}
		}

		public CIVector (nfloat [] values) :
			this (values, values == null ? 0 : values.Length)
		{
		}

		public CIVector (float x)
#if NO_NFLOAT_OPERATORS
			: this (new NFloat (x))
#else
			: this ((nfloat) x)
#endif
		{
		}

		public CIVector (float x, float y)
#if NO_NFLOAT_OPERATORS
			: this (new NFloat (x), new NFloat (y))
#else
			: this ((nfloat) x, (nfloat) y)
#endif
		{
		}

		public CIVector (float x, float y, float z)
#if NO_NFLOAT_OPERATORS
			: this (new NFloat (x), new NFloat (y), new NFloat (z))
#else
			: this ((nfloat) x, (nfloat) y, (nfloat) z)
#endif
		{
		}

		public CIVector (float x, float y, float z, float w)
#if NO_NFLOAT_OPERATORS
			: this (new NFloat (x), new NFloat (y), new NFloat (z), new NFloat (w))
#else
			: this ((nfloat) x, (nfloat) y, (nfloat) z, (nfloat) w)
#endif
		{
		}



		[DesignatedInitializer]
		[Export ("initWithValues:count:")]
		public unsafe CIVector (nfloat [] values, nint count) : base (NSObjectFlag.Empty)
		{
			if (values == null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (values));
			if (count > values.Length)
				throw new ArgumentOutOfRangeException (nameof (count));

			fixed (nfloat *ptr = values) {
				var handle = IntPtr.Zero;
				if (IsDirectBinding) {
					handle = Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr (Handle, Selector.GetHandle ("initWithValues:count:"), (IntPtr) ptr, (IntPtr) count);
				} else {
					handle = Messaging.IntPtr_objc_msgSendSuper_IntPtr_IntPtr (SuperHandle, Selector.GetHandle ("initWithValues:count:"), (IntPtr) ptr, (IntPtr) count);
				}
				InitializeHandle (handle, "initWithValues:count:");
			}
		}

		public unsafe static CIVector FromValues (nfloat [] values)
		{
			if (values == null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (values));
			fixed (nfloat *ptr = values)
				return _FromValues ((IntPtr) ptr, values.Length);
		}
		
		public override string ToString ()
		{
			return StringRepresentation ();
		}
	}
}
