using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util{
	public class AssetBundleManager {
        private static AssetBundle _character = null;
        public static string characterUrl{ get{ return HttpClient.assetBandleURL + "charaimage.unity3d";} }
        public static AssetBundle character{ set{ _character = value; } }
        private static AssetBundle _horse = null;
        public static string horseUrl{ get{ return HttpClient.assetBandleURL + "horseimage.unity3d";} }
        public static AssetBundle horse{ set{ _horse = value; } }
        private static AssetBundle _hat = null;
        public static string hatUrl{ get{ return HttpClient.assetBandleURL + "hatimage.unity3d";} }
        public static AssetBundle hat{ set{ _hat = value; } }
        private static AssetBundle _map = null;
        public static string mapUrl{ get{ return HttpClient.assetBandleURL + "mapimage.unity3d";} }
        public static AssetBundle map{ set{ _map = value; } }
        private static AssetBundle _clothes = null;
        public static string clothesUrl{ get{ return HttpClient.assetBandleURL + "clothesimage.unity3d";} }
        public static AssetBundle clothes{ set{ _clothes = value; } }
        private static AssetBundle _weapon = null;
        public static string weaponUrl{ get{ return HttpClient.assetBandleURL + "weaponimage.unity3d";} }
        public static AssetBundle weapon{ set{ _weapon = value; } }

		public static Sprite GetAvatarBody(string name){
			return GetAvatarSprite(name);
		}
		public static Sprite GetAvatarHead(string name){
			return GetAvatarSprite(name);
		}
		public static Sprite GetAvatarSprite(string name){
			return _character.LoadAsset<Sprite>(name);
		}
		public static Sprite GetClothes(string name){
            return _clothes.LoadAsset<Sprite>(name);
		}
		public static Sprite GetWeapon(string name){
            return _weapon.LoadAsset<Sprite>(name);
		}
		public static Sprite GetHorse(string name){
			return _horse.LoadAsset<Sprite>(name);
		}
		public static Sprite GetAvatarHat(string name){
			return _hat.LoadAsset<Sprite>(name);
		}
		public static Sprite GetMapTile(string name){
			return _map.LoadAsset<Sprite>(name);
		}
	}
}