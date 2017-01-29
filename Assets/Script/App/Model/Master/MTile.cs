﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
    [System.Serializable]
	public class MTile : MBase {
        public MTile(){
        }
        public int id;//
        public string name;//
        public float heal;//回复能力
        //可使用法术
        public int[] _strategys;
        public int[] strategys{
            get{ 
                if (_strategys == null)
                {
                    _strategys = (int[])JsonFx.JsonReader.Deserialize(strategy, typeof(int[]));
                }
                return _strategys;
            }
        }
        public string strategy;//可使用法术
	}
}