﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionEngine {

	public sealed class ParallelAction : ActionBase {

		public enum PlayState {
			INCOMPLETE,
			COMPLETE
		}

		private readonly List<ActionBase> actions_ = new List<ActionBase>();
		private readonly List<PlayState> playStates_ = new List<PlayState>();

		#region Parameters

		public ParallelAction Add (ActionBase action) {
			action.SetOwner(this);
			actions_.Add(action);
			playStates_.Add(PlayState.INCOMPLETE);
			return this;
		}

		#endregion Parameters

		protected override void OnBegin () {
			for (var i = 0; i < actions_.Count; ++i) {
				actions_[i].Begin();
			}
		}

		protected override bool OnUpdate (float deltaTime) {
			var allCompleted = true;

			for (var i = 0; i < actions_.Count; ++i) {
				var playState = playStates_[i];
				if (playState == PlayState.COMPLETE)
					continue;

				var action = actions_[i];
				if (action.Update(deltaTime) == false) {
					allCompleted = false;
					continue;
				}

				action.Complete();
				playStates_[i] = PlayState.COMPLETE;
			}

			return allCompleted;
		}

		protected override void OnComplete () {
			for (var i = 0; i < actions_.Count; ++i) {
				var playState = playStates_[i];
				if (playState == PlayState.COMPLETE)
					continue;

				var action = actions_[i];
				action.Complete();

				playStates_[i] = PlayState.COMPLETE;
			}
		}

		protected override void OnRewind () {
			playStates_.Clear();

			for (var i = 0; i < actions_.Count; ++i) {
				actions_[i].Rewind();
				playStates_.Add(PlayState.INCOMPLETE);
			}
		}

		protected override void OnKill () {
			for (var i = 0; i < actions_.Count; ++i) {
				actions_[i].Kill();
			}
			actions_.Clear();
			playStates_.Clear();
		}
	}
}
