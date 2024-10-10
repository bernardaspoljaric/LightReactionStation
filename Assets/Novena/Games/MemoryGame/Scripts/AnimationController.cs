using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Novena.Games.MemoryGame {
	public class AnimationController {

		private float _turnAnimationSpeed;

		public AnimationController(float turnAnimationSpeed)
		{
			_turnAnimationSpeed = turnAnimationSpeed;
		}


		public void TurnCardHalfwayX(RectTransform rt, bool direction, TweenCallback onComplete = null)
		{
			rt.DORotate(new Vector3(0, direction ? 0 : 90f, 0), _turnAnimationSpeed).OnComplete(onComplete);
		}

		public void TurnCardHalfwayY(RectTransform rt, bool direction, TweenCallback onComplete = null)
		{
			rt.DORotate(new Vector3(direction ? 0 : 90f, 0, 0), _turnAnimationSpeed).OnComplete(onComplete);
		}

		public void ShowText(TMP_Text text, bool show)
		{
			text.DOFade(show ? 1 : 0, 0);
		}
	}
}
