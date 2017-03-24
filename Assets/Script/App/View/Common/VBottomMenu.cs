
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using Holoville.HOTween;
using App.Util;
using App.Controller;
using App.View.Common;

namespace App.View.Common{
    public class VBottomMenu : VBase{
        public virtual void Open(){
        }
        public virtual void Close(System.Action complete){
        }
	}
}