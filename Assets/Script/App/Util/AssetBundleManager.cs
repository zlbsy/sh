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
		private static AssetBundle clothes = null;
		private static AssetBundle weapon = null;

		public static Sprite GetAvatarBody(string name){
			return GetAvatarSprite(name);
		}
		public static Sprite GetAvatarHead(string name){
			return GetAvatarSprite(name);
		}
		public static Sprite GetAvatarSprite(string name){
			if (character == null) {
				character = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/chara.unity3d");
			}
			return character.LoadAsset<Sprite>(name);
		}
		public static Sprite GetClothes(string name){
			if (clothes == null) {
				clothes = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/clothes.unity3d");
			}
			return clothes.LoadAsset<Sprite>(name);
		}
		public static Sprite GetWeapon(string name){
			if (weapon == null) {
				weapon = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/weapon.unity3d");
			}
			return weapon.LoadAsset<Sprite>(name);
		}
		public static Sprite GetHorse(string name){
			if (horse == null) {
				horse = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/horse.unity3d");
			}
			return horse.LoadAsset<Sprite>(name);
		}
		public static Sprite GetAvatarHat(string name){
			if (hat == null) {
				hat = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/hat.unity3d");
			}
			return hat.LoadAsset<Sprite>(name);
		}
		public static Sprite GetMapTile(string name){
			if (map == null) {
				map = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/map.unity3d");
			}
			return map.LoadAsset<Sprite>(name);
		}
	}
}