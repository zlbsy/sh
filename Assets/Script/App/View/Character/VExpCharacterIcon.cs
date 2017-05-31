﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;
using App.Model.Battle;
using Holoville.HOTween;

namespace App.View.Character{
    public class VExpCharacterIcon : VBase {
        [SerializeField]private VCharacterIcon characterIcon;
        [SerializeField]private Text txtExp;
        [SerializeField]private Image imgExp;
        [SerializeField]private Text levelUp;
        public override void UpdateView(App.Model.MBase model)
        {
            levelUp.gameObject.SetActive(false);
            MExpCharacter mExpCharacter = model as MExpCharacter;
            characterIcon.BindingContext = System.Array.Find(Global.SUser.self.characters, c=>c.CharacterId == mExpCharacter.id).ViewModel;
            characterIcon.UpdateView();
            txtExp.text = mExpCharacter.toExp.ToString();
            if (mExpCharacter.toLevel > mExpCharacter.fromLevel)
            {
                ShowLevelUp();
            }
            //float width = 120f * mExpCharacter.toExp
            imgExp.rectTransform.sizeDelta = new Vector2(-120f*0.7f, imgExp.rectTransform.sizeDelta.y);
            imgExp.rectTransform.anchoredPosition = new Vector2(imgExp.rectTransform.sizeDelta.x * 0.5f, imgExp.rectTransform.anchoredPosition.y);
        }
        private void ShowLevelUp(){
            levelUp.gameObject.SetActive(true);
            levelUp.transform.localScale = Vector3.zero;
            Sequence seqHp = new Sequence ();
            seqHp.Insert (0f, HOTween.To (levelUp.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one * 2f, false).Ease(EaseType.EaseInQuart)));
            seqHp.Insert (0.2f, HOTween.To (levelUp.transform, 0.3f, new TweenParms().Prop("localScale", Vector3.one, false).OnComplete(()=>{
                levelUp.gameObject.SetActive(false);
            })));
            seqHp.Play ();
        }
    }
}