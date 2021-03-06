﻿using System;
using System.Collections;
using UnityEngine;

namespace ActionEngine {

	public sealed class ScriptAction : ActionBase<ScriptAction> {
		private Action script_ = null;

		#region Parameters

		public ScriptAction SetScript (Action script) {
			script_ = script;
			return this;
		}

		#endregion Parameters

		protected override void OnBegin () {
			if (script_ != null)
				script_();
		}

		protected override void OnKill () {
			script_ = null;
		}
	}
}
