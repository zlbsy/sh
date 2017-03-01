using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using System;
using System.Reflection;

namespace App.Util.LSharp{
    public class LSharpScript : LSharpBase<LSharpScript> {
        private Dictionary<string, object> subClasses;
        private List<List<string>[]> dataList = new List<List<string>[]>();
        private List<string> lineList;
        private List<string> copyList;
        public LSharpScript(){
            subClasses = new Dictionary<string, object>();
            subClasses.Add("if", LSharpIf.Instance);
            subClasses.Add("function", LSharpFunction.Instance);
            subClasses.Add("Load", LSharpLoad.Instance);
            subClasses.Add("Character", LSharpCharacter.Instance);
            subClasses.Add("Talk", LSharpTalk.Instance);
        }
        public void ToList(List<string> datas){
            lineList = new List<string>(datas);
            copyList = new List<string>(datas);
            analysis();
        }
        public void SaveList(){
            List<string>[] arr = dataList[0];
            if (arr != null)
            {
                arr[1]=lineList;
                arr[2]=copyList;
            }
        }
        public void analysis(List<string> datas){
            List<string>[] arr = new List<string>[]{datas, null, null};
            dataList.Insert(0, arr);
            ToList(datas);
        }
        public override void analysis(){
            string lineValue = "";
            if (lineList.Count == 0)
            {
                dataList.RemoveAt(0);
                if (dataList.Count > 0)
                {
                    List<string>[] arr = dataList[0];
                    lineList = arr[1];
                    copyList = arr[2];
                    analysis();
                }
                return;
            }
            while (lineList.Count > 0 && lineValue.Length == 0)
            {
                lineValue = lineList[0].Trim(); 
                lineList.RemoveAt(0);
            }
            if (lineValue.Length == 0)
            {
                analysis();
                return;
            }
            lineValue = LSharpVarlable.GetVarlable(lineValue);
            Debug.Log("lineValue = " + lineValue);
            if(lineValue.IndexOf("if") >= 0){
                CallAnalysis(subClasses["if"], lineValue);
            }else if(lineValue.IndexOf("function") >= 0){
                CallAnalysis(subClasses["function"], lineValue);
            }
            string[] sarr = lineValue.Split('.');
            string key = sarr[0];
            if (subClasses.ContainsKey(key))
            {
                object subClass = subClasses[key];
                CallAnalysis(subClass, lineValue);
            }
            else
            {
                analysis();
            }
        }
        public void CallAnalysis(object o, string lineValue){
            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("analysis",new Type[]{typeof(String)});
            if (mi != null)
            {
                mi.Invoke(o, new string[]{lineValue});
            }
        }
	}
}