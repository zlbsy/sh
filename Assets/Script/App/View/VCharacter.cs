using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;

namespace App.View{
	public class VCharacter : VBase {

		[SerializeField]Image imgHorse;
		[SerializeField]Image imgBody;
		[SerializeField]Image imgHead;
		[SerializeField]Image imgHat;
		[SerializeField]Image imgWeapon;
		private int animationIndex = 0;
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
				//ViewModel.Body.OnValueChanged -= BodyChanged;
			}
			if (ViewModel!=null)
			{
				ViewModel.Head.OnValueChanged += HeadChanged;
				ViewModel.Hat.OnValueChanged += HatChanged;
				ViewModel.Horse.OnValueChanged += HorseChanged;
				//ViewModel.Body.OnValueChanged += BodyChanged;
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
			ResetAll();
		}
		/*private void BodyChanged(int oldvalue, int newvalue)
		{
			ResetAll();
		}*/
		private void ResetAll(){
			AvatarAction avatarAction = AvatarAsset.Data.GetAvatarAction(ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, animationIndex);
			string key;
			//Horse
			if (avatarAction.horse == null) {
				imgHorse.gameObject.SetActive (false);
			} else {
				imgHorse.gameObject.SetActive (true);
				key = string.Format("horse_{0}_{1}_{2}", ViewModel.Horse.Value, ViewModel.Action.Value, avatarAction.horse.index);
				imgHorse.sprite = AssetBundleManager.GetHorse(key);
				imgHorse.SetNativeSize ();
				imgHorse.GetComponent<RectTransform> ().localPosition = avatarAction.horse.position;
			}
			//Body
			key = string.Format("body_{0}_{1}_{2}_{3}", ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.body.index);
			imgBody.sprite = AssetBundleManager.GetAvatarBody(key);
			imgBody.SetNativeSize ();
			//Weapon
			key = string.Format("weapon_{0}_{1}_{2}_{3}_{4}", ViewModel.Weapon.Value, ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.body.index);
			imgWeapon.sprite = AssetBundleManager.GetWeapon(key);
			imgWeapon.SetNativeSize ();

			imgBody.GetComponent<RectTransform> ().localPosition = avatarAction.body.position;
			imgHead.GetComponent<RectTransform> ().localPosition = avatarAction.head.position;
			imgWeapon.GetComponent<RectTransform> ().localPosition = avatarAction.weapon.position;
		}
		#endregion


		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}
		public void ChangeAction(int index){
			//ViewModel.Body.Value = index;

		}
		public void ChangeAnimationIdex(int index){
			animationIndex = index;
			ResetAll ();
		}
		public void EmptyAction(){
		}
	}
}