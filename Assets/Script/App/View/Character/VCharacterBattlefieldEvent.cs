using App.Model;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Character{
    public partial class VCharacter{
        public void OnDamage(App.Model.Battle.MDamageParam arg){
            this.ChangeAction(ActionType.hert);
            this.num.text = arg.value.ToString();
            this.num.gameObject.SetActive(true);
            this.num.transform.localPosition = new Vector3(0,0.2f,0);
            this.num.color = Color.white;
            Vector3 saveScale = this.num.transform.localScale;
            this.num.transform.localScale = Vector3.zero;
            var seqWaveReward = new Sequence ();
            seqWaveReward.Insert (0f, HOTween.To (this.num.transform, 0.2f, new TweenParms().Prop("localScale", saveScale * 2f, false).Ease(EaseType.EaseInQuart)));
            seqWaveReward.Insert (0.2f, HOTween.To (this.num.transform, 0.3f, new TweenParms().Prop("localScale", saveScale, false).Ease(EaseType.EaseOutBounce)));
            seqWaveReward.Insert (0.5f, HOTween.To (this.num, 0.2f, new TweenParms().Prop("color", new Color(this.num.color.r, this.num.color.g, this.num.color.b, 0f), false).OnComplete(()=>{
                this.num.gameObject.SetActive(false);
            })));
            seqWaveReward.Insert (0f, HOTween.To (this.ViewModel.Hp, 0.2f, new TweenParms().Prop("Value", this.ViewModel.Hp.Value + arg.value, false).Ease(EaseType.EaseInQuart)));
            seqWaveReward.Play ();
        }
    }
}