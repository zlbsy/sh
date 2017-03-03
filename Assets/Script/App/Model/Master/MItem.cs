using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
    public class MItem : MBase {
        public enum ItemType
        {
            ap
        }
        public MItem(){
        }
        public string name;//
        /*public ItemType type;*/
        //public int price;
	}
}