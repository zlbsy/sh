using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using App.View.Character;
using UnityEngine.UI;
using App.Util;
using App.Controller.Common;


namespace App.Controller.Register{
    public class CRegisterConfirmDialog : CDialog {
        [SerializeField]private InputField account;
        [SerializeField]private InputField password;
        [SerializeField]private InputField name;
        public override IEnumerator OnLoad( Request request ) 
        {  
            int characterId = request.Get<int>("characterId");

            yield return this.StartCoroutine(base.OnLoad(request));
        }
	}
}