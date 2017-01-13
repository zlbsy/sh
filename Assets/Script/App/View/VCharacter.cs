using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.View{
	public class VCharacter : VBase {
		[SerializeField]Image image;
		private string spriteName = "b-";
		public static AssetBundle assetbundle = null;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		public void ChangeAction(string strIndex){
			//image.sprite.name = spriteName + strIndex;
			image.sprite = loadSprite(spriteName + strIndex);

		}
		private Sprite loadSprite(string spriteName){
			if(assetbundle == null)
				assetbundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/chara.assetbundle");
			return assetbundle.LoadAsset<Sprite>(spriteName);
			//return Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
		}
	}
}