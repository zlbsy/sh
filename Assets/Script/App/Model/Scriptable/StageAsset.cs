using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable{
    public class StageAsset : AssetBase<StageAsset> {
        [SerializeField]public App.Model.Master.MStage[] stages;

	}
}