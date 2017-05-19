using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.View.Item;
using App.Controller.Common;


namespace App.Controller{
    public class CItemListDialog : CDialog {
        [SerializeField]private Transform content;
        [SerializeField]private GameObject childItem;
        [SerializeField]private VItemDetail itemDetail; 
        public override IEnumerator OnLoad( Request request ) 
        {  
            yield return StartCoroutine(base.OnLoad(request));
            if (Global.SUser.self.items == null)
            {
                SItem sItem = new SItem();
                yield return StartCoroutine(sItem.RequestList());
                Global.SUser.self.items = sItem.items;
            }
            ScrollViewSets(content, childItem, Global.SUser.self.items);
            yield return 0;
        }
        public void ItemIconClick(int id){Debug.LogError("ItemIconClick");
            App.Model.MItem mItem = System.Array.Find(Global.SUser.self.items, i=>i.Id == id);
            itemDetail.BindingContext = mItem.ViewModel;
            itemDetail.UpdateView();
        }
        public void ClickSaleItem(int id){
        }
        public void ClickUseItem(int id){
        }
        public override void Close(){
            itemDetail.Close();
            base.Close();
        }
	}
}