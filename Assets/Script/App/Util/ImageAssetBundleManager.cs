using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Service;

namespace App.Util{
    public class ImageAssetBundleManager {
        private static AssetBundle _weapon = null;
        public static string weaponUrl{ get{ return HttpClient.assetBandleURL + "weaponmesh.unity3d";} }
        //public static string weaponUrl{ get{ return HttpClient.assetBandleURL + "weaponimage.unity3d";} }
        public static Anima2D.SpriteMeshInstance[] weapon;
        private static AssetBundle _head = null;
        public static string headUrl{ get{ return HttpClient.assetBandleURL + "headmesh.unity3d";} }
        public static Anima2D.SpriteMeshInstance[] head;
        private static AssetBundle _horse = null;
        public static string horseUrl{ get{ return HttpClient.assetBandleURL + "horsemesh.unity3d";} }
        //public static string horseUrl{ get{ return HttpClient.assetBandleURL + "horseimage.unity3d";} }
        public static Anima2D.SpriteMeshInstance[] horse;
        private static AssetBundle _clothes = null;
        public static string clothesUrl{ get{ return HttpClient.assetBandleURL + "clothesmesh.unity3d";} }
        //public static string clothesUrl{ get{ return HttpClient.assetBandleURL + "clothesimage.unity3d";} }
        public static Anima2D.SpriteMeshInstance[] clothes;

        private static AssetBundle _avatar = null;
        public static string avatarUrl{ get{ return HttpClient.assetBandleURL + "charaimage.unity3d";} }
        public static AssetBundle avatar{ set{ _avatar = value; } }
        private static AssetBundle _hat = null;
        public static string hatUrl{ get{ return HttpClient.assetBandleURL + "hatimage.unity3d";} }
        public static AssetBundle hat{ set{ _hat = value; } }

        private static AssetBundle _map = null;
        public static string mapUrl{ get{ return HttpClient.assetBandleURL + "mapimage.unity3d";} }
        public static AssetBundle map{ set{ _map = value; } }
        private static AssetBundle _equipmentIcon = null;
        public static string equipmentIconUrl{ get{ return HttpClient.assetBandleURL + "equipmenticonimage.unity3d";} }
        public static AssetBundle equipmentIcon{ set{ _equipmentIcon = value; } }
        private static AssetBundle _itemIcon = null;
        public static string itemIconUrl{ get{ return HttpClient.assetBandleURL + "itemiconimage.unity3d";} }
        public static AssetBundle itemIcon{ set{ _itemIcon = value; } }
        private static AssetBundle _gachaIcon = null;
        public static string gachaIconUrl{ get{ return HttpClient.assetBandleURL + "gachaimage.unity3d";} }
        public static AssetBundle gachaIcon{ set{ _gachaIcon = value; } }
        private static AssetBundle _skillIcon = null;
        public static string skillIconUrl{ get{ return HttpClient.assetBandleURL + "skilliconimage.unity3d";} }
        public static AssetBundle skillIcon{ set{ _skillIcon = value; } }

        public static Anima2D.SpriteMeshInstance GetClothesUpMesh(int id){
            string name = string.Format("clothes_{0}_up", id);
            return System.Array.Find(clothes, _=>_.name == name);
            //return _clothes.LoadAsset<Anima2D.SpriteMeshInstance>(string.Format("clothes_{0}_up", id));
        }
        public static Anima2D.SpriteMeshInstance GetClothesDownMesh(int id){
            string name = string.Format("clothes_{0}_down", id);
            return System.Array.Find(clothes, _=>_.name == name);
            //return _clothes.LoadAsset<Anima2D.SpriteMeshInstance>(string.Format("clothes_{0}_down", id));
        }
        public static Anima2D.SpriteMeshInstance GetHatMesh(int id){
            string name = string.Format("hat_{0}", id);
            return System.Array.Find(head, _=>_.name == name);
            //return _head.LoadAsset<Anima2D.SpriteMeshInstance>(string.Format("hat_{0}", id));
        }
        public static Anima2D.SpriteMeshInstance GetHeadMesh(int id){
            string name = string.Format("head_{0}", id);
            return System.Array.Find(head, _=>_.name == name);
            //return _head.LoadAsset<Anima2D.SpriteMeshInstance>(string.Format("head_{0}", id));
        }
        public static Anima2D.SpriteMeshInstance GetWeaponMesh(int id){
            string name = string.Format("weapon_{0}", id);
            return System.Array.Find(weapon, _=>_.name == name);
            //return _weapon.LoadAsset<Anima2D.SpriteMeshInstance>(string.Format("weapon_{0}", id));
        }
        public static Anima2D.SpriteMeshInstance GetHorseBodyMesh(int id){
            string name = string.Format("horse_body_{0}", id);
            return System.Array.Find(horse, _=>_.name == name);
        }
        public static Anima2D.SpriteMeshInstance GetHorseFrontLegLeftMesh(int id){
            string name = string.Format("horse_front_lleg_{0}", id);
            return System.Array.Find(horse, _=>_.name == name);
        }
        public static Anima2D.SpriteMeshInstance GetHorseFrontLegRightMesh(int id){
            string name = string.Format("horse_front_rleg_{0}", id);
            return System.Array.Find(horse, _=>_.name == name);
        }
        public static Anima2D.SpriteMeshInstance GetHorseHindLegLeftMesh(int id){
            string name = string.Format("horse_hind_lleg_{0}", id);
            return System.Array.Find(horse, _=>_.name == name);
        }
        public static Anima2D.SpriteMeshInstance GetHorseHindLegRightMesh(int id){
            string name = string.Format("horse_hind_rleg_{0}", id);
            return System.Array.Find(horse, _=>_.name == name);
        }
        public static Anima2D.SpriteMeshInstance GetHorseSaddleMesh(int id){
            string name = string.Format("horse_saddle_{0}", id);
            return System.Array.Find(horse, _=>_.name == name);
            //return _horse.LoadAsset<Anima2D.SpriteMeshInstance>(string.Format("horse_saddle_{0}", id));
        }

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
        public static Sprite GetSkillIcon(int id){
            return _skillIcon.LoadAsset<Sprite>("skill_"+id);
        }
	}
}