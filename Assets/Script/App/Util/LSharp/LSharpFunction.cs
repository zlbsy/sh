﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util.LSharp{
    public class LSharpFunction : LSharpBase<LSharpFunction> {
        private Dictionary<string, Dictionary<string, List<string>>> funList = new Dictionary<string, Dictionary<string, List<string>>>();
        public Dictionary<string, Dictionary<string, List<string>>> FunList{
            get{ 
                return funList;
            }
        }
        public static void AddFunction(string lineValue){
            lineValue = lineValue.Trim();
            Dictionary<string, List<string>> funArr = new Dictionary<string, List<string>>();
            int startNameIndex = lineValue.IndexOf(" ");
            int start = lineValue.IndexOf("(");
            int end = lineValue.IndexOf(")");
            string funName = lineValue.Substring(startNameIndex + 1, start - startNameIndex - 1);
            funName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(funName);
            string strParam = lineValue.Substring(start + 1, end - start - 1);
            string[] paramArr = strParam.Split(',');
            List<string> paramList = new List<string>();
            foreach (string childParam in paramArr)
            {
                string v = childParam.Trim();
                if (v.Length > 0)
                {
                    paramList.Add("param_" + v);
                }
            }
            funArr.Add("param", paramList);
            List<string> funLineList = new List<string>();
            string child; 
            while((child = LSharpScript.Instance.ShiftLine()).IndexOf("endfunction") < 0){
                foreach (string childParam in paramArr)
                {
                    string v = childParam.Trim();
                    if (v.Length > 0)
                    {
                        child = child.Replace("@"+v,"@param_"+v);
                    }
                }
                funLineList.Add(child);
            }
            funArr.Add("function", funLineList);
            LSharpFunction.Instance.FunList.Add(funName, funArr);

            LSharpScript.Instance.Analysis();
        }
        public override void Analysis(string value){
            string methodName;
            string[] arguments;
            Analysis(value, out methodName, out arguments);
            if (!funList.ContainsKey(methodName))
            {
                LSharpScript.Instance.Analysis();
                return;
            }
            Dictionary<string, List<string>> funArr = funList[methodName];
            List<string> paramList = funArr["param"];
            for (int i = 0; i < paramList.Count; i++)
            {
                LSharpVarlable.SetVarlable(paramList[i], arguments[i]);
            }
            List<string> funLineList = funArr["function"];
            foreach (string line in funLineList)
            {
                LSharpScript.Instance.UnshiftLine(line);
            }
            LSharpScript.Instance.Analysis();
        }
    }
}