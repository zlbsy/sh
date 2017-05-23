using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;


namespace App.Controller.LoginBonus{
    public class CLoginBonusDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            App.Model.Master.MLoginBonus[] loginBonusesList = App.Util.Cacher.LoginBonusCacher.Instance.GetAll();
            //Debug.LogError("loginBonusesList="+loginBonusesList.Length);
            SLoginBonus sLoginBonus = new SLoginBonus();
            yield return StartCoroutine(sLoginBonus.RequestLogCount());
            List<MLoginBonus> loginBonuses = new List<MLoginBonus>();
            int count = 0;
            int day = 1;
            foreach(App.Model.Master.MLoginBonus model in loginBonusesList){
                MLoginBonus mLoginBonus = new MLoginBonus();
                mLoginBonus.Id = model.id;
                mLoginBonus.Contents = model.contents;
                mLoginBonus.Day = day++;
                if (count++ < Global.SUser.self.Loginbonus_cnt)
                {
                    mLoginBonus.Received = true;
                }
                loginBonuses.Add(mLoginBonus);
            }
            ScrollViewSets(content, childItem, loginBonuses.ToArray());
            yield return 0;
        }
        public void ItemIconClick(int id){
            
        }
	}
}