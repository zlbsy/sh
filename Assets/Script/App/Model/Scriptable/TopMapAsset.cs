using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class TopMapAsset : AssetBase<TopMapAsset> {
        [SerializeField]public App.Model.Master.MTopMap[] topMaps;
        [SerializeField]public int version;

	}
}