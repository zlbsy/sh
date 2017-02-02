using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;


namespace App.Controller{
    public class CBuildingDialog : CDialog {
		public override IEnumerator OnLoad( ) 
		{  
            yield return StartCoroutine(base.OnLoad());
			yield return 0;
		}
	}
}