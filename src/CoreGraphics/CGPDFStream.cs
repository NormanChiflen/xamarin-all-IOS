// 
// CGPDFStream.cs: Implements the managed CGPDFStream binding
//
// Authors: Miguel de Icaza
//     
// Copyright 2010 Novell, Inc
// Copyright 2011-2014 Xamarin Inc
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

#nullable enable

using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace CoreGraphics {

	// untyped enum -> CGPDFStream.h
	public enum CGPDFDataFormat {
		Raw,
		JPEGEncoded,
		JPEG2000
	};

	// CGPDFStream.h
	public class CGPDFStream : NonRefcountedNativeObject {
		internal CGPDFStream (IntPtr handle)
			: base (handle, false)
		{
		}

		protected override void Free ()
		{
			// Nothing to do here
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPDFDictionaryRef */ IntPtr CGPDFStreamGetDictionary (/* CGPDFStreamRef */ IntPtr stream);
		
		public CGPDFDictionary Dictionary {
			get {
				return new CGPDFDictionary (CGPDFStreamGetDictionary (Handle));
			}
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CFDataRef */ IntPtr CGPDFStreamCopyData (/* CGPDFStreamRef */ IntPtr stream, /* CGPDFDataFormat* */ out CGPDFDataFormat format);

		public NSData GetData (out CGPDFDataFormat format)
		{
			IntPtr obj = CGPDFStreamCopyData (Handle, out format);
			return Runtime.GetNSObject<NSData> (obj, true);
		}
	}
}
