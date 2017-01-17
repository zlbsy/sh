using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;

namespace App.View{
	public class VCharacter : VBase {

		[SerializeField]Image image;
		[SerializeField]Image imgHead;
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
			//image.sprite = loadSprite(spriteName + strIndex);

		}
		private Sprite loadSprite(string spriteName){
			if(assetbundle == null)
				assetbundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath +"/chara.assetbundle");
			return assetbundle.LoadAsset<Sprite>(spriteName);
			//return Resources.Load<GameObject>("Sprite/" + spriteName).GetComponent<SpriteRenderer>().sprite;
		}

		public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
		protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
		{

			base.OnBindingContextChanged(oldViewModel, newViewModel);

			VMCharacter oldVm = oldViewModel as VMCharacter;
			if (oldVm != null)
			{
				ViewModel.Head.OnValueChanged -= HeadChanged;
			}
			if (ViewModel!=null)
			{
				ViewModel.Head.OnValueChanged += HeadChanged;
			}
		}
		private void HeadChanged(int oldvalue, int newvalue)
		{
			imgHead.sprite = loadSprite("head_" + newvalue);
		}
	}
}