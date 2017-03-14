using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;


namespace App.Controller{
    public class CItemListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.self.items == null)
            {
                SItem sItem = new SItem();
                yield return StartCoroutine(sItem.RequestList());
                Global.SUser.self.items = sItem.items;
            }
            foreach(App.Model.MItem item in Global.SUser.self.items){
                GameObject obj = Instantiate(childItem);
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                VItemIcon vItemIcon = obj.GetComponent<VItemIcon>();
                vItemIcon.BindingContext = item.ViewModel;
                vItemIcon.UpdateView();
            }
            yield return 0;
        }
        public void ItemIconClick(int itemId){

        }
	}
}