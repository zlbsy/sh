using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
	public class MTile : MBase {
        public MTile(){
		}
        public string name;//
        public float heal;//回复能力
        public int[] strategy_ids;//可使用法术
	}
}