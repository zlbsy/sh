using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using App.Model.Scriptable;


namespace App.Controller{
    public class CGachaResultDialog : CDialog {
        [SerializeField]private Transform[] positions;
        [SerializeField]private GameObject childItem;
        [SerializeField]private GameObject childEquipment;
        public override IEnumerator OnLoad( Request request ) 
		{  
            yield return StartCoroutine(base.OnLoad(request));

		}
	}
}