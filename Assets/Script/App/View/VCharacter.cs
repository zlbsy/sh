using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;

namespace App.View{
	public class VCharacter : VBase {

		[SerializeField]Image image;
		[SerializeField]Image imgHead;
		[SerializeField]Image imgHat;
		private string spriteName = "b-";
		public static AssetBundle assetbundleCharacter = null;
		public static AssetBundle assetbundleHat = null;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		public void ChangeAction(string strIndex){
			//image.sprite.name = spriteName + strIndex;
			//image.sprite = loadSprite(spriteName + strIndex);

		}
		private Sprite loadSprite(string spriteName){
			if(assetbundleCharacter == null)
				assetbundleCharacter = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/chara.assetbundle");
			return assetbundleCharacter.LoadAsset<Sprite>(spriteName);
			//return Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
		}
		private Sprite loadHat(string spriteName){
			if(assetbundleHat == null)
				assetbundleHat = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/hat.assetbundle");
			return assetbundleHat.LoadAsset<Sprite>(spriteName);
		}

		public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
		protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
		{

			base.OnBindingContextChanged(oldViewModel, newViewModel);

			VMCharacter oldVm = oldViewModel as VMCharacter;
			if (oldVm != null)
			{
				ViewModel.Head.OnValueChanged -= HeadChanged;
				ViewModel.Hat.OnValueChanged -= HatChanged;
			}
			if (ViewModel!=null)
			{
				ViewModel.Head.OnValueChanged += HeadChanged;
				ViewModel.Hat.OnValueChanged += HatChanged;
			}
		}
		private void HeadChanged(int oldvalue, int newvalue)
		{
			imgHead.sprite = loadSprite("head_" + newvalue);
		}
		private void HatChanged(int oldvalue, int newvalue)
		{
			imgHat.sprite = loadHat("hat_" + newvalue);
		}
	}
}