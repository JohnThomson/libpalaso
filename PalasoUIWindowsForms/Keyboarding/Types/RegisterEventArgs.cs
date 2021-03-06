// Copyright (c) 2013, SIL International.
// Distributable under the terms of the MIT license (http://opensource.org/licenses/MIT).
using System;
using System.Windows.Forms;

namespace Palaso.UI.WindowsForms.Keyboarding.Types
{
	public class RegisterEventArgs: EventArgs
	{
		public RegisterEventArgs(Control control, object eventHandler)
		{
			Control = control;
			EventHandler = eventHandler;
		}

		public Control Control { get; private set; }
		public object EventHandler { get; private set; }
	}
}
