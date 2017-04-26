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
            subClasses.Add("Call", LSharpFunction.Instance);
            subClasses.Add("Load", LSharpLoad.Instance);
            subClasses.Add("Character", LSharpCharacter.Instance);
            subClasses.Add("Talk", LSharpTalk.Instance);
            subClasses.Add("Battle", LSharpBattle.Instance);
            subClasses.Add("Var", LSharpVarlable.Instance);
            subClasses.Add("Tutorial", LSharpTutorial.Instance);
            subClasses.Add("Wait", LSharpWait.Instance);
            if (LSharpVarlable.Instance.VarList.Count == 0 && Global.SUser.self.Progress != null)
            {
                Dictionary<string, int> progress = Global.SUser.self.Progress;
                foreach(string k in progress.Keys){
                    LSharpVarlable.SetVarlable(k, progress[k].ToString());
                }
            }
        }
        public void ToList(List<string> datas){
            lineList = new List<string>(datas);
            copyList = new List<string>(datas);
            Analysis();
        }
        public void SaveList(){
            List<string>[] arr = dataList[0];
            if (arr != null)
            {
                arr[1]=lineList;
                arr[2]=copyList;
            }
        }
        public int LineListCount{
            get{
                return lineList.Count;
            }
        }
        public void Analysis(List<string> datas){
            if (dataList.Count > 0)
            {
                SaveList();
            }
            List<string>[] arr = new List<string>[]{datas, null, null};
            dataList.Insert(0, arr);
            ToList(datas);
        }
        public string ShiftLine(){
            if (lineList.Count == 0)
            {
                return string.Empty;
            }
            string lineValue = lineList[0].Trim(); 
            lineList.RemoveAt(0);
            return lineValue;
        }
        public void UnshiftLine(string lineValue){
            lineList.Insert(0, lineValue);
        }
        public override void Analysis(){
            string lineValue = "";
            if (lineList.Count == 0)
            {
                dataList.RemoveAt(0);
                if (dataList.Count > 0)
                {
                    List<string>[] arr = dataList[0];
                    lineList = arr[1];
                    copyList = arr[2];
                    Analysis();
                }
                return;
            }
            while (lineList.Count > 0 && lineValue.Length == 0)
            {
                lineValue = lineList[0].Trim(); 
                lineList.RemoveAt(0);
            }
            if (string.IsNullOrEmpty(lineValue.Trim()))
            {
                Analysis();
                return;
            }
            lineValue = LSharpVarlable.GetVarlable(lineValue);
            Debug.Log("lineValue = " + lineValue);
            if(lineValue.IndexOf("if") >= 0){
                LSharpIf.GetIf(lineValue);
                return;
            }else if(lineValue.IndexOf("function") >= 0){
                LSharpFunction.AddFunction(lineValue);
                return;
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
                Analysis();
            }
        }
        public void CallAnalysis(object o, string lineValue){
            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("Analysis",new Type[]{typeof(String)});
            if (mi != null)
            {
                mi.Invoke(o, new string[]{lineValue});
            }
        }
	}
}