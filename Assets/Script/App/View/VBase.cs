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

        public void Destroy( )
        {
            if ( Application.isPlaying ) {
                if ( Application.isEditor ) {
                    UnityEngine.Object.DestroyImmediate( gameObject, true );
                } else {
                    UnityEngine.Object.Destroy( gameObject );
                }
            } else {
                UnityEngine.Object.DestroyImmediate( gameObject, true );
            }    
        }
	}
}