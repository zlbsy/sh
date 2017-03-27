using App.Model;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Character{
    public partial class VCharacter{
        public void OnDamage(App.Model.Battle.MDamageParam arg){
            this.ChangeAction(ActionType.hert);
            this.num.text = arg.value.ToString();
            this.num.transform.localPosition = new Vector3(0,0.2f,0);
            this.num.color = Color.white;
            this.num.gameObject.SetActive(true);
            HOTween.To(this.num.transform, 1f, new TweenParms().Prop("localPosition", new Vector3(0,0.5f,0)));
            HOTween.To(this.num, 1f, new TweenParms().Prop("color", new Color(this.num.color.r, this.num.color.g, this.num.color.b, 0f)));
        }
    }
}