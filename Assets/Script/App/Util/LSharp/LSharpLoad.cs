using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using System;
using System.Reflection;

namespace App.Util.LSharp{
    public class LSharpLoad : LSharpBase {
        string[] data;
        protected void analysis(string value, out string methodName, out string[] arguments){
            int methodStart = value.IndexOf('.');
            int start = value.IndexOf("(");
            int end = value.IndexOf(")");
            methodName = value.Substring(methodStart + 1, start).Trim();
            arguments = value.Substring(start + 1, end).Split(',');
        }
        public override void analysis(string value){
            string methodName;
            string[] arguments;
            analysis(value, out methodName, out arguments);
            Type t = this.GetType();
            MethodInfo mi = t.GetMethod(methodName);
            if (mi != null)
            {
                mi.Invoke(this, arguments);
            }
            else
            {
                LSharpScript.Instance.analysis();
            }
        }
	}
}