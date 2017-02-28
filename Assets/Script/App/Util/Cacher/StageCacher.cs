using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace App.Util.Cacher{
    public class StageCacher: CacherBase<StageCacher, App.Model.Master.MStage> {
        public App.Model.Master.MStage[] GetStages(int areaId){
            return System.Array.FindAll(datas, _=>_.area_id == areaId);
        }
    }
}