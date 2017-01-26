﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class AssetBase<TClass> : ScriptableObject 
        where TClass : class,new()
    {
        private static TClass _data;
        public static TClass Data{
			get{ 
				if (_data == null) {
                    _data = Resources.Load(Name) as TClass;
				}
				return _data;
			}
        }
        public static string Name{
            get{ 
                string[] paths = typeof(TClass).ToString().Split('.');
                string path = paths[paths.Length - 1].ToLower();
                return path;
            }
        }
        public static void Clear(){
            _data = null;
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
	}
}