using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;
using System;
using System.Reflection;

namespace App.Util.LSharp{
    public class LSharpBase<T> where T : class,new(){
        protected static T instance;

        public static T Instance 
        {
            get { 
                return instance ?? (instance = new T()); 
            }
        }
        public virtual void analysis(){
        }
        protected void analysis(string value, out string methodName, out string[] arguments){
            int methodStart = value.IndexOf('.');
            int start = value.IndexOf("(");
            int end = value.IndexOf(")");
            methodName = value.Substring(methodStart + 1, start - methodStart - 1).Trim();
            arguments = value.Substring(start + 1, end - start - 1).Split(',');
        }
        public virtual void analysis(string value){
            string methodName;
            string[] arguments;
            analysis(value, out methodName, out arguments);
            Type t = this.GetType();
            MethodInfo mi = t.GetMethod(methodName,new Type[]{typeof(string[])});
            if (mi != null)
            {
                mi.Invoke(this, new string[][]{arguments});
            }
            else
            {
                LSharpScript.Instance.analysis();
            }
        }
	}
}