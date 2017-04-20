using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class TutorialAsset : AssetBase<TutorialAsset> {
        [SerializeField]public List<string>[] tutorials;
	}
}