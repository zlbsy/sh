using App.Model;
using App.View.Character;
using System;
using System.Collections;
using System.Collections.Generic;


namespace App.Controller.Battle{
    public enum CharacterEvent{
        OnDamage,
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
            List<VCharacter> characters = this.charactersManager.GetDamageCharacters(vCharacter, target, vCharacter.ViewModel.CurrentSkill.Value.Master);
            App.View.VTile tile = mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            foreach (VCharacter child in characters)
            {
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-this.calculateManager.Hert(mCharacter, this.GetCharacterModel(child), tile));
                child.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
            }
        }
    }
}