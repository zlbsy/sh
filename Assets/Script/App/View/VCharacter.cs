using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;

namespace App.View{
	public class VCharacter : VBase {

		[SerializeField]Image imgHorse;
		[SerializeField]Image imgBody;
		[SerializeField]Image imgHead;
		[SerializeField]Image imgHat;
		[SerializeField]Image imgWeapon;

		#region VM处理
		public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
		protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
		{

			base.OnBindingContextChanged(oldViewModel, newViewModel);

			VMCharacter oldVm = oldViewModel as VMCharacter;
			if (oldVm != null)
			{
				ViewModel.Head.OnValueChanged -= HeadChanged;
				ViewModel.Hat.OnValueChanged -= HatChanged;
				ViewModel.Horse.OnValueChanged -= HorseChanged;
				ViewModel.Body.OnValueChanged -= BodyChanged;
			}
			if (ViewModel!=null)
			{
				ViewModel.Head.OnValueChanged += HeadChanged;
				ViewModel.Hat.OnValueChanged += HatChanged;
				ViewModel.Horse.OnValueChanged += HorseChanged;
				ViewModel.Body.OnValueChanged += BodyChanged;
			}
		}
		private void HeadChanged(int oldvalue, int newvalue)
		{
			imgHead.sprite = AssetBundleManager.GetAvatarHead("head_" + newvalue);
		}
		private void HatChanged(int oldvalue, int newvalue)
		{
			imgHat.sprite = AssetBundleManager.GetAvatarHat("hat_" + newvalue);
		}
		private void HorseChanged(string oldvalue, string newvalue)
		{
			imgHorse.sprite = AssetBundleManager.GetHorse(string.Format("horse_{0}_attack_1", newvalue));
		}
		private void BodyChanged(int oldvalue, int newvalue)
		{//body_cavalry_longKnife_attack_1
			imgBody.sprite = AssetBundleManager.GetAvatarBody(string.Format("body_cavalry_longKnife_attack_1", newvalue));
		}
		#endregion


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
	}
}