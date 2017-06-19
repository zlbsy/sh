using App.Model;
using App.View.Character;
using System;
using System.Collections;
using System.Collections.Generic;


namespace App.Controller.Battle{
    public enum CharacterEvent{
        OnDamage,
        OnHeal,
        OnHealWithoutAction,
    }
    public partial class CBattlefield{
        public delegate void EventHandler();
        public event EventHandler ActionEndHandler;
        private void ActionEnd(){
            if (ActionEndHandler != null)
            {
                ActionEndHandler();
            }
        }
        public void WaitActionEnd(){
            this.StartCoroutine(WaitActionEndCoroutine());
        }
        public IEnumerator WaitActionEndCoroutine(){
            while (HasDynamicCharacter())
            {
                yield return new UnityEngine.WaitForEndOfFrame();
            }
            ActionEnd();
        }
        public void OnDamage(VCharacter vCharacter){
            MCharacter mCharacter = this.GetCharacterModel(vCharacter);
            MCharacter targetModel = vCharacter.ViewModel.Target.Value;
            VCharacter target = this.GetCharacterView(targetModel);
            List<VCharacter> characters = this.charactersManager.GetTargetCharacters(vCharacter, target, vCharacter.ViewModel.CurrentSkill.Value.Master);
            App.View.VTile tile = mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            foreach (VCharacter child in characters)
            {
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-this.calculateManager.Hert(mCharacter, this.GetCharacterModel(child), tile));
                child.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
                if (child.ViewModel.CharacterId.Value == targetModel.CharacterId)
                {
                    if (mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.vampire && mCharacter.CurrentSkill.Master.effect.enemy.time == App.Model.Master.SkillEffectBegin.enemy_hert)
                    {
                        App.Model.Master.MStrategy strategy = App.Util.Cacher.StrategyCacher.Instance.Get(mCharacter.CurrentSkill.Master.effect.enemy.strategys[0]);
                        VCharacter currentCharacter = this.GetCharacterView(mCharacter);

                        int addHp = -UnityEngine.Mathf.FloorToInt(arg.value * strategy.hert * 0.01f);
                        App.Model.Battle.MDamageParam arg2 = new App.Model.Battle.MDamageParam(addHp);
                        currentCharacter.SendMessage(CharacterEvent.OnHealWithoutAction.ToString(), arg2);
                    }
                }
            }
        }
        public void OnHeal(VCharacter vCharacter){
            //UnityEngine.Debug.LogError("OnHeal");
            MCharacter mCharacter = this.GetCharacterModel(vCharacter);
            MCharacter targetModel = vCharacter.ViewModel.Target.Value;
            VCharacter target = this.GetCharacterView(targetModel);
            List<VCharacter> characters = this.charactersManager.GetTargetCharacters(vCharacter, target, vCharacter.ViewModel.CurrentSkill.Value.Master);
            App.View.VTile tile = mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            foreach (VCharacter child in characters)
            {
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(this.calculateManager.Heal(mCharacter, this.GetCharacterModel(child), tile));
                child.SendMessage(CharacterEvent.OnHeal.ToString(), arg);
            }
        }
    }
}