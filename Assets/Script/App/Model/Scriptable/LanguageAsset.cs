using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class LanguageAsset : AssetBase<LanguageAsset> {
        [SerializeField]public App.Model.Master.MWord[] words;

	}
}