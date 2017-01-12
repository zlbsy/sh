using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model{
	public class MBase {
		public App.View.VBase view;
		public void SetView(App.View.VBase view = null){
			this.view = view;
		}
		
	}
}