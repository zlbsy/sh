using App.Model;

namespace App.View.Character{
    public partial class VCharacter{
        public void OnDamage(App.Model.Battle.MDamageParam arg){
            this.ChangeAction(ActionType.hert);
        }
    }
}