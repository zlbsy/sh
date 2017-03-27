using App.Model;
using App.View.Character;


namespace App.Controller.Battle{
    public enum CharacterEvent{
        OnDamage,
    }
    public partial class CBattlefield{
        public void OnDamage(VCharacter vCharacter){
            MCharacter targetModel = vCharacter.ViewModel.Target.Value;
            VCharacter target = this.vBaseMap.Characters.Find(_=>_.ViewModel.CharacterId.Value == targetModel.CharacterId && _.ViewModel.Belong.Value == targetModel.Belong);
            App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-20);
            target.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
        }
        /*
        #region Event处理
        public void SendEvent(CharacterEvent et, VCharacter target, object arg = null){

            target.SendMessage(et.ToString(), arg);
        }
        #endregion
        */
    }
}