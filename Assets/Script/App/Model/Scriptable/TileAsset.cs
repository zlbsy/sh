using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
	public class TileAsset : ScriptableObject {
        public TileAsset(){
		}
        [SerializeField]public App.Model.Master.MTile[] tiles;

		public const string PATH = "tileAsset";
        private static TileAsset _data;
        public  static TileAsset  Data{
			get{ 
				if (_data == null) {
                    _data = Resources.Load<TileAsset>(PATH);
				}
				return _data;
			}
		}
	}
}