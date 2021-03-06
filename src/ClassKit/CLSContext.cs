//
// CLSContext.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2018 Xamarin Inc. All rights reserved.
//

using System;
using Foundation;
using ObjCRuntime;

namespace ClassKit {
	public partial class CLSContext {

		public CLSContextTopic Topic {
			get => CLSContextTopicExtensions.GetValue (WeakTopic);
			set => WeakTopic = value.GetConstant ();
		}
	}
}
