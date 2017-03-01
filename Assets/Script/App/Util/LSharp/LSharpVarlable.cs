using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util.LSharp{
    public class LSharpVarlable : LSharpBase<LSharpVarlable> {
		public override void analysis(){
			LSharpScript.Instance.analysis();
        }
        public static string GetVarlable(string str){
            string result = str;
            return result;
        }
	}
}