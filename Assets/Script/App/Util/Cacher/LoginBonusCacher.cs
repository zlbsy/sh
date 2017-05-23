using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace App.Util.Cacher{
    public class LoginBonusCacher: CacherBase<LoginBonusCacher, App.Model.Master.MLoginBonus> {
        public override App.Model.Master.MLoginBonus[] GetAll(){Debug.LogError("App.Service.HttpClient.Now.Month="+App.Service.HttpClient.Now.Month);
            return System.Array.FindAll(datas, d=>d.year == App.Service.HttpClient.Now.Year && d.month == App.Service.HttpClient.Now.Month);
        }
    }
}