using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util.LSharp{
    public class LSharpScript : LSharpBase {
        private Dictionary<string, LSharpBase> subClasses;
        private List<List<string>[]> dataList = new List<List<string>[]>();
        private List<string> lineList;
        private List<string> copyList;
        public LSharpScript(){
            subClasses = new Dictionary<string, LSharpBase>();
            subClasses.Add("if", LSharpIf.Instance);
            subClasses.Add("function", LSharpFunction.Instance);
            subClasses.Add("Character", LSharpCharacter.Instance);
            subClasses.Add("Talk", LSharpCharacter.Instance);
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
            List<string>[] arr = new List<string>[]{datas};
            dataList.Insert(0, arr);
        }
        public override void analysis(){
            string lineValue = "";
            if (lineList.Count == 0)
            {
                dataList.RemoveAt(0);
                if (dataList.Count > 0)
                {
                    List<string>[] arr = dataList[0];
                    lineList = arr[0];
                    copyList = arr[1];
                    analysis();
                }
                return;
            }
            while (lineList.Count > 0)
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
            if(lineValue.IndexOf("if") >= 0){
                subClasses["if"].analysis();
            }else if(lineValue.IndexOf("function") >= 0){
                subClasses["function"].analysis();
            }
            string[] sarr = lineValue.Split('.');
            if (subClasses.ContainsKey(sarr[0]))
            {
                LSharpBase subClass = subClasses[sarr[0]];
                subClass.analysis(lineValue);
            }
            else
            {
                analysis();
            }
        }
	}
}