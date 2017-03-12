﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util{
	public class ImageAssetBundleManager {
        private static AssetBundle _avatar = null;
        public static string avatarUrl{ get{ return HttpClient.assetBandleURL + "charaimage.unity3d";} }
        public static AssetBundle avatar{ set{ _avatar = value; } }
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
        private static AssetBundle _equipmentIcon = null;
        public static string equipmentIconUrl{ get{ return HttpClient.assetBandleURL + "equipmenticonimage.unity3d";} }
        public static AssetBundle equipmentIcon{ set{ _equipmentIcon = value; } }
        private static AssetBundle _itemIcon = null;
        public static string itemIconUrl{ get{ return HttpClient.assetBandleURL + "itemiconimage.unity3d";} }
        public static AssetBundle itemIcon{ set{ _itemIcon = value; } }
        private static AssetBundle _gachaIcon = null;
        public static string gachaIconUrl{ get{ return HttpClient.assetBandleURL + "gachaimage.unity3d";} }
        public static AssetBundle gachaIcon{ set{ _gachaIcon = value; } }

		public static Sprite GetAvatarBody(string name){
			return GetAvatarSprite(name);
		}
		public static Sprite GetAvatarHead(int id){
            return GetAvatarSprite("head_" + id);
		}
		public static Sprite GetAvatarSprite(string name){
			return _avatar.LoadAsset<Sprite>(name);
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
        public static Sprite GetAvatarHat(int id){
            return _hat.LoadAsset<Sprite>("hat_" + id);
		}
		public static Sprite GetMapTile(string name){
			return _map.LoadAsset<Sprite>(name);
        }
        public static Sprite GetEquipmentIcon(string name){
            return _equipmentIcon.LoadAsset<Sprite>(name);
        }
        public static Sprite GetItemIcon(int id){
            return _itemIcon.LoadAsset<Sprite>("item_"+id);
        }
        public static Sprite GetGachaIcon(int id){
            return _gachaIcon.LoadAsset<Sprite>("gacha_"+id);
        }
	}
}