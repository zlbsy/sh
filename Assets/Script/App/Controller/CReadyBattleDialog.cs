using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;


namespace App.Controller{
    public class CReadyBattleDialog : CDialog {
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return this.StartCoroutine(base.OnLoad(request));
        }
	}
}