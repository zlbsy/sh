using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Util.Cacher;
using App.Controller;

namespace App.View.Effect{
    public class VEffectAnimation : VBase {
        [SerializeField]private SpriteRenderer image;
        [SerializeField]public Animator animator;
        public void AnimationEnd(){
            GameObject.Destroy(this.gameObject);
            System.GC.Collect();
        }
    }
}