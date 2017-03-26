using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;

namespace App.View.Common{
    public class VTweenAlpha : VBase {
        [SerializeField]private float from;
        [SerializeField]private float to;
        [SerializeField]private float duration = 0.5f;
        [SerializeField]private float delay = 0f;
        [SerializeField]private bool isLoop;
        [SerializeField]private LoopType loopType;
        private Sequence sequence;
        public void Start()
        {
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            sr.color = new Color(1f, 1f, 1f, from);
            sequence = new Sequence ();
            sequence.Insert (delay, HOTween.To (sr, duration, new TweenParms().Prop("color", new Color(1f, 1f, 1f, to), false).Ease(EaseType.EaseInQuart)));
            if (isLoop)
            {
                sequence.loopType = loopType;
                sequence.loops = int.MaxValue;
            }
            sequence.Play ();
        }
        public override void OnDestroy(){
            sequence.Kill();
        }
	}
}