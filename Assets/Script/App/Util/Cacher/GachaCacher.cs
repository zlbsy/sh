using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Service;

namespace App.Util.Cacher{
    public class GachaCacher: CacherBase<GachaCacher, App.Model.Master.MGacha> {

        public App.Model.Master.MGacha[] GetAllOpen(){
            return System.Array.FindAll(datas, _=>_.fromTime <= HttpClient.Now && _.toTime >= HttpClient.Now);
        }
    }
}