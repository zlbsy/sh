using UnityEngine.UI;
using App.Model;
using UnityEngine;
using App.ViewModel;
using App.Util;
using App.Util.Cacher;
using App.View.Common;


namespace App.View.LoginBonus{
    public class VLoginBonusChild : VBase {
        [SerializeField]private Image receivedIcon;
        [SerializeField]private VContentsChild vContentsChild;

        #region VM处理
        //public VMBattleChild ViewModel { get { return (VMBattleChild)BindingContext; } }
        #endregion
        public override void UpdateView()
        {
            
        }
    }
}