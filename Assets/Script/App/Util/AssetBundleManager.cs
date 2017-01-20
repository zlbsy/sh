using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;

namespace App.Util{
	public class AssetBundleManager {
		private static AssetBundle character = null;
		private static AssetBundle horse = null;
		private static AssetBundle hat = null;
		private static AssetBundle map = null;
		private static AssetBundle weapon = null;

		public static Sprite GetAvatarBody(string name){
			return GetAvatarSprite(name);
		}
		public static Sprite GetAvatarHead(string name){
			return GetAvatarSprite(name);
		}
		public static Sprite GetAvatarSprite(string name){
			if (character == null) {
				character = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/chara.assetbundle");
			}
			return character.LoadAsset<Sprite>(name);
		}
		public static Sprite GetWeapon(string name){
			if (weapon == null) {
				weapon = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/weapon.assetbundle");
			}
			return weapon.LoadAsset<Sprite>(name);
		}
		public static Sprite GetHorse(string name){
			if (horse == null) {
				horse = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/horse.assetbundle");
			}
			return horse.LoadAsset<Sprite>(name);
		}
		public static Sprite GetAvatarHat(string name){
			if (hat == null) {
				hat = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/hat.assetbundle");
			}
			return hat.LoadAsset<Sprite>(name);
		}
		public static Sprite GetMapTile(string name){
			if (map == null) {
				map = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/map.assetbundle");
			}
			return map.LoadAsset<Sprite>(name);
		}
	}
}