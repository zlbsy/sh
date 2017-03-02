using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util.LSharp{
    public class LSharpVarlable : LSharpBase<LSharpVarlable> {
        private Dictionary<string, string> varList = new Dictionary<string, string>();
        public Dictionary<string, string> VarList
        {
            get{ 
                return varList;
            }
        }

        public void Set(string[] arguments){
            LSharpVarlable.SetVarlable(arguments[0], arguments[1]);
            LSharpScript.Instance.Analysis();
        }
        public static void SetVarlable(string key, string value){
            LSharpVarlable.Instance.VarList.Add(key, value);
        }
        public static string GetVarlable(string str){
            string result = str;
            return result;
        }
	}
}