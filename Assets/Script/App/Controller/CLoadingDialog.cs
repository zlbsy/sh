using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using UnityEngine.UI;


namespace App.Controller{
    public class CLoadingDialog : CDialog {
        [SerializeField]private Image barBackground;
        [SerializeField]private Image barPrevious;
        [SerializeField]private Text progress;
        [SerializeField]private Text message;
        [SerializeField]private Image review;
		public override IEnumerator OnLoad( ) 
		{  
            yield return StartCoroutine(base.OnLoad());
			yield return 0;
		}
	}
}