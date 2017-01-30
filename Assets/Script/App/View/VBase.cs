using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;

namespace App.View{
	public interface IView
	{
		VMBase BindingContext { get; set; }
	}
	public class VBase : MonoBehaviour, IView {
		public readonly VMProperty<VMBase> vmProperty = new VMProperty<VMBase>();
		public VMBase BindingContext
		{
			get { return vmProperty.Value; }
			set { vmProperty.Value = value; }
		}
		protected virtual void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
		{
		}
		public VBase()
		{
			this.vmProperty.OnValueChanged += OnBindingContextChanged;
		}

        public void ClearChild()
        {
            var t = this.transform;
            for ( int i = 0; i< t.childCount; i++) {
                GameObject.Destroy(t.GetChild(i).gameObject);
            }
            t.DetachChildren();    //子要素情報が残っているとテーブルが崩れるため解除処理
        }
	}
}