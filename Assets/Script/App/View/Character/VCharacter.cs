using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.Model.Avatar;
using App.Model.Master;
using App.Util.Cacher;
using App.Util.Battle;

namespace App.View.Character{
    public partial class VCharacter : VBase {
        [SerializeField]Anima2D.SpriteMeshInstance head;
        [SerializeField]Anima2D.SpriteMeshInstance hat;
        [SerializeField]Anima2D.SpriteMeshInstance weapon;
        [SerializeField]Anima2D.SpriteMeshInstance weaponRight;
        [SerializeField]Anima2D.SpriteMeshInstance weaponArchery;
        [SerializeField]Anima2D.SpriteMeshInstance weaponString;
        [SerializeField]Anima2D.SpriteMeshInstance weaponArrow;
        [SerializeField]Anima2D.SpriteMeshInstance clothesUpShort;
        [SerializeField]Anima2D.SpriteMeshInstance clothesDownShort;
        [SerializeField]Anima2D.SpriteMeshInstance clothesUpLong;
        [SerializeField]Anima2D.SpriteMeshInstance clothesDownLong;
        [SerializeField]Anima2D.SpriteMeshInstance armLeftShort;
        [SerializeField]Anima2D.SpriteMeshInstance armRightShort;
        [SerializeField]Anima2D.SpriteMeshInstance armLeftLong;
        [SerializeField]Anima2D.SpriteMeshInstance armRightLong;
        [SerializeField]Anima2D.SpriteMeshInstance horseBody;
        [SerializeField]Anima2D.SpriteMeshInstance horseFrontLegLeft;
        [SerializeField]Anima2D.SpriteMeshInstance horseFrontLegRight;
        [SerializeField]Anima2D.SpriteMeshInstance horseHindLegLeft;
        [SerializeField]Anima2D.SpriteMeshInstance horseHindLegRight;
        [SerializeField]Anima2D.SpriteMeshInstance horseSaddle;
        [SerializeField]Anima2D.SpriteMeshInstance legLeft;
        [SerializeField]Anima2D.SpriteMeshInstance legRight;
        private Dictionary<string,Anima2D.SpriteMeshInstance> meshs = new Dictionary<string, Anima2D.SpriteMeshInstance>();
        private Anima2D.SpriteMeshInstance Weapon{
            get{ 
                return weapon.gameObject.activeSelf ? weapon : weaponArchery;
            }
        }
        private Anima2D.SpriteMeshInstance ClothesUp{
            get{ 
                return clothesUpShort.gameObject.activeSelf ? clothesUpShort : clothesUpLong;
            }
        }
        private Anima2D.SpriteMeshInstance ClothesDown{
            get{ 
                return clothesDownShort.gameObject.activeSelf ? clothesDownShort : clothesDownLong;
            }
        }
        private Anima2D.SpriteMeshInstance ArmLeft{
            get{ 
                return armLeftShort.gameObject.activeSelf ? armLeftShort : armLeftLong;
            }
        }
        private Anima2D.SpriteMeshInstance ArmRight{
            get{ 
                return armRightShort.gameObject.activeSelf ? armRightShort : armRightLong;
            }
        }
        [SerializeField]Transform content;
        [SerializeField]Transform hpTransform;
        [SerializeField]TextMesh num;
        [SerializeField]MeshRenderer meshRenderer;
        private static Material materialGray;
        private static Material materialDefault;
        private static Dictionary<App.Model.Belong, Material> hpMaterials;
        private Anima2D.SpriteMeshInstance[] allSprites;
        private bool init = false;
        private int animationIndex = 0;
        private Animator _animator;
        private Animator animator{
            get{ 
                if (_animator == null)
                {
                    _animator = this.GetComponentInChildren<Animator>();
                }
                return _animator;
            }
        }
        private void Init(){
            if (init)
            {
                return;
            }
            init = true;
            if (meshs.Count == 0)
            {
                meshs.Add("head", head);
                meshs.Add("hat", hat);
                meshs.Add("weapon", weapon);
                meshs.Add("weaponRight", weaponRight);
                meshs.Add("weaponArchery", weaponArchery);
                meshs.Add("weaponString", weaponString);
                meshs.Add("weaponArrow", weaponArrow);
                meshs.Add("clothesUpShort", clothesUpShort);
                meshs.Add("clothesDownShort", clothesDownShort);
                meshs.Add("clothesUpLong", clothesUpLong);
                meshs.Add("clothesDownLong", clothesDownLong);
                meshs.Add("armLeftShort", armLeftShort);
                meshs.Add("armRightShort", armRightShort);
                meshs.Add("armLeftLong", armLeftLong);
                meshs.Add("armRightLong", armRightLong);
                meshs.Add("horseBody", horseBody);
                meshs.Add("horseFrontLegLeft", horseFrontLegLeft);
                meshs.Add("horseFrontLegRight", horseFrontLegRight);
                meshs.Add("horseHindLegLeft", horseHindLegLeft);
                meshs.Add("horseHindLegRight", horseHindLegRight);
                meshs.Add("horseSaddle", horseSaddle);
                meshs.Add("legLeft", legLeft);
                meshs.Add("legRight", legRight);
            }
            if (hpMaterials == null)
            {
                materialGray = Resources.Load("Material/GrayMaterial") as Material;
                materialDefault = head.sharedMaterial;

                hpMaterials = new Dictionary<App.Model.Belong, Material>();
                hpMaterials.Add(App.Model.Belong.self, Resources.Load("Material/SelfHp") as Material);
                hpMaterials.Add(App.Model.Belong.friend, Resources.Load("Material/FriendHp") as Material);
                hpMaterials.Add(App.Model.Belong.enemy, Resources.Load("Material/EnemyHp") as Material);
            }
            num.GetComponent<MeshRenderer>().sortingOrder = clothesDownLong.sortingOrder + 10;
            num.gameObject.SetActive(false);
            BelongChanged(ViewModel.Belong.Value, ViewModel.Belong.Value);
        }
        private bool Gray{
            set{
                Material material = value ? materialGray : materialDefault;
                allSprites = (allSprites == null ? this.GetComponentsInChildren<Anima2D.SpriteMeshInstance>(true) : allSprites);
                foreach (Anima2D.SpriteMeshInstance sprite in allSprites)
                {
                    sprite.sharedMaterial = material;
                }
            }
            get{ 
                return head.sharedMaterial.Equals(materialGray);
            }
        }
        #region VM处理
        public VMCharacter ViewModel { get { return (VMCharacter)BindingContext; } }
        protected override void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
        {

            base.OnBindingContextChanged(oldViewModel, newViewModel);

            VMCharacter oldVm = oldViewModel as VMCharacter;
            if (oldVm != null)
            {
                oldVm.Head.OnValueChanged -= HeadChanged;
                oldVm.Hat.OnValueChanged -= HatChanged;
                oldVm.Horse.OnValueChanged -= HorseChanged;
                oldVm.Clothes.OnValueChanged -= ClothesChanged;
                oldVm.Weapon.OnValueChanged -= WeaponChanged;
                oldVm.WeaponType.OnValueChanged -= WeaponTypeChanged;
                oldVm.Action.OnValueChanged -= ActionChanged;
                oldVm.MoveType.OnValueChanged -= MoveTypeChanged;
                oldVm.CoordinateX.OnValueChanged -= CoordinateXChanged;
                oldVm.CoordinateY.OnValueChanged -= CoordinateYChanged;
                oldVm.X.OnValueChanged -= XChanged;
                oldVm.Y.OnValueChanged -= YChanged;
                oldVm.Direction.OnValueChanged -= DirectionChanged;
                oldVm.Hp.OnValueChanged -= HpChanged;
                oldVm.ActionOver.OnValueChanged -= ActionOverChanged;
            }
            if (ViewModel!=null)
            {
                ViewModel.Head.OnValueChanged += HeadChanged;
                ViewModel.Hat.OnValueChanged += HatChanged;
                ViewModel.Horse.OnValueChanged += HorseChanged;
                ViewModel.Clothes.OnValueChanged += ClothesChanged;
                ViewModel.Weapon.OnValueChanged += WeaponChanged;
                ViewModel.WeaponType.OnValueChanged += WeaponTypeChanged;
                ViewModel.Action.OnValueChanged += ActionChanged;
                ViewModel.MoveType.OnValueChanged += MoveTypeChanged;
                ViewModel.CoordinateX.OnValueChanged += CoordinateXChanged;
                ViewModel.CoordinateY.OnValueChanged += CoordinateYChanged;
                ViewModel.X.OnValueChanged += XChanged;
                ViewModel.Y.OnValueChanged += YChanged;
                ViewModel.Direction.OnValueChanged += DirectionChanged;
                ViewModel.Hp.OnValueChanged += HpChanged;
                ViewModel.ActionOver.OnValueChanged += ActionOverChanged;
            }
        }
        private App.Controller.CBaseMap cBaseMap{
            get{
                return this.Controller as App.Controller.CBaseMap;
            }
        }
        private void ActionOverChanged(bool oldvalue, bool newvalue)
        {
            Gray = newvalue;
            Debug.LogError("animator.speed="+animator.speed);
            animator.speed = newvalue ? 0 : 1;
        }
        private void HpChanged(int oldvalue, int newvalue)
        {
            float hpValue = newvalue * 1f / ViewModel.Ability.Value.HpMax;
            hpTransform.localPosition = new Vector3(0f, 1f - hpValue, 0f);
            hpTransform.localScale = new Vector3(1f, hpValue, 1f);
        }
        private void DirectionChanged(App.Model.Direction oldvalue, App.Model.Direction newvalue)
        {
            content.localScale = new Vector3(newvalue == App.Model.Direction.left ? 1 : -1, 1, 1);
        }
        private void XChanged(float oldvalue, float newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            this.transform.localPosition = new Vector3(newvalue, this.transform.localPosition.y, 0f);
            if (newvalue > oldvalue)
            {
                ViewModel.Direction.Value = App.Model.Direction.right;
            }
            else if (newvalue < oldvalue)
            {
                ViewModel.Direction.Value = App.Model.Direction.left;
            }
        }
        private void YChanged(float oldvalue, float newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, newvalue, 0f);
        }
        private void CoordinateXChanged(int oldvalue, int newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            VBaseMap vBaseMap = cBaseMap.GetVBaseMap();
            int i = ViewModel.CoordinateY.Value * vBaseMap.mapWidth + newvalue;
            VTile vTile = vBaseMap.tileUnits[i];
            ViewModel.X.Value = vTile.transform.localPosition.x;
        }
        private void CoordinateYChanged(int oldvalue, int newvalue)
        {
            if (cBaseMap == null)
            {
                return;
            }
            VBaseMap vBaseMap = cBaseMap.GetVBaseMap();
            int i = ViewModel.CoordinateY.Value * vBaseMap.mapWidth + newvalue;
            VTile vTile = vBaseMap.tileUnits[i];
            ViewModel.Y.Value = vTile.transform.localPosition.y;
        }
        public float alpha{
            set{ 
                foreach (Anima2D.SpriteMeshInstance sprite in allSprites)
                {
                    sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,value);
                }
            }
            get{ 
                return allSprites[0].color.a;
            }
        }
        private void ActionChanged(App.Model.ActionType oldvalue, App.Model.ActionType newvalue)
        {
            /*this.StartCoroutine(ActionChanged(newvalue));
        }
        private IEnumerator ActionChanged(App.Model.ActionType newvalue)
        {
            yield return new WaitForEndOfFrame();*/
            string animatorName = string.Format("{0}_{1}_{2}", ViewModel.MoveType.ToString(), App.Util.WeaponManager.GetWeaponTypeAction(ViewModel.WeaponType.Value, newvalue), newvalue.ToString());
            //Debug.LogError(ViewModel.CharacterId.Value + " = " + animatorName);
            animator.Play(animatorName);
            animationIndex = 0;
            if (newvalue == App.Model.ActionType.idle)
            {
                if (ViewModel.Hp.Value == 0)
                {
                    Holoville.HOTween.HOTween.To(this, 1f, new Holoville.HOTween.TweenParms().Prop("alpha", 0f).OnComplete(()=>{
                        this.gameObject.SetActive(false);
                        this.alpha = 1f;
                        if(App.Util.SceneManager.CurrentScene != null){
                            App.Util.SceneManager.CurrentScene.StartCoroutine(RemoveDynamicCharacter());
                        }
                    }));
                }
                else
                {
                    this.StartCoroutine(RemoveDynamicCharacter());
                }
            }
            else
            {
                this.Controller.SendMessage("AddDynamicCharacter", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        private IEnumerator RemoveDynamicCharacter(){
            while (this.gameObject.activeSelf && this.num.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            this.Controller.SendMessage("RemoveDynamicCharacter", this, SendMessageOptions.DontRequireReceiver);
        }
        private void MoveTypeChanged(App.Model.MoveType oldvalue, App.Model.MoveType newvalue)
        {
            if (newvalue == App.Model.MoveType.cavalry)
            {
                content.localPosition = new Vector3(content.localPosition.x, 0.48f, content.localPosition.z);
            }
            else
            {
                content.localPosition = new Vector3(content.localPosition.x, 0.24f, content.localPosition.z);
            }
            ActionChanged(ViewModel.Action.Value, ViewModel.Action.Value);
        }
        private void HeadChanged(int oldvalue, int newvalue)
        {
            head.spriteMesh = ImageAssetBundleManager.GetHeadMesh(newvalue);
        }
        private void HatChanged(int oldvalue, int newvalue)
        {
            hat.spriteMesh = ImageAssetBundleManager.GetHatMesh(newvalue);
        }
        private void BelongChanged(App.Model.Belong oldvalue, App.Model.Belong newvalue)
        {
            meshRenderer.material = hpMaterials[newvalue];
        }
        private void WeaponTypeChanged(App.Model.WeaponType oldvalue, App.Model.WeaponType newvalue){
            ActionChanged(ViewModel.Action.Value, ViewModel.Action.Value);
        }
        private void WeaponChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MEquipment mEquipment = EquipmentCacher.Instance.GetEquipment(newvalue, MEquipment.EquipmentType.weapon);
            bool isArchery = (mEquipment.weapon_type == App.Model.WeaponType.archery);
            weapon.gameObject.SetActive(!isArchery);
            weaponArchery.gameObject.SetActive(isArchery);
            if (mEquipment.weapon_type == App.Model.WeaponType.dualWield)
            {
                //this.weaponRight.gameObject.SetActive(true);
                this.Weapon.spriteMesh = ImageAssetBundleManager.GetLeftWeaponMesh(newvalue);
                this.weaponRight.spriteMesh = ImageAssetBundleManager.GetRightWeaponMesh(newvalue);
            }
            else
            {
                //this.weaponRight.gameObject.SetActive(false);
                this.Weapon.spriteMesh = ImageAssetBundleManager.GetWeaponMesh(newvalue);
            }
        }
        private void HorseChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MEquipment mEquipment = EquipmentCacher.Instance.GetEquipment(newvalue, MEquipment.EquipmentType.horse);
            if (mEquipment.move_type == App.Model.MoveType.cavalry)
            {
                horseBody.spriteMesh = ImageAssetBundleManager.GetHorseBodyMesh(mEquipment.image_index);
                horseFrontLegLeft.spriteMesh = ImageAssetBundleManager.GetHorseFrontLegLeftMesh(mEquipment.image_index);
                horseFrontLegRight.spriteMesh = ImageAssetBundleManager.GetHorseFrontLegRightMesh(mEquipment.image_index);
                horseHindLegLeft.spriteMesh = ImageAssetBundleManager.GetHorseHindLegLeftMesh(mEquipment.image_index);
                horseHindLegRight.spriteMesh = ImageAssetBundleManager.GetHorseHindLegRightMesh(mEquipment.image_index);

                horseSaddle.spriteMesh = ImageAssetBundleManager.GetHorseSaddleMesh(mEquipment.saddle);
                legLeft.spriteMesh = ImageAssetBundleManager.GetShoeLeftMesh(App.Util.Global.Constant.shoe_default_index);
                legRight.spriteMesh = ImageAssetBundleManager.GetShoeRightMesh(App.Util.Global.Constant.shoe_default_index);
            }
            else
            {
                legLeft.spriteMesh = ImageAssetBundleManager.GetShoeLeftMesh(mEquipment.image_index);
                legRight.spriteMesh = ImageAssetBundleManager.GetShoeRightMesh(mEquipment.image_index);
            }
        }
        private void ClothesChanged(int oldvalue, int newvalue)
        {
            App.Model.Master.MEquipment mEquipment = EquipmentCacher.Instance.GetEquipment(newvalue, MEquipment.EquipmentType.clothes);
            bool isArmor = (mEquipment.clothes_type == MEquipment.ClothesType.armor);

            clothesUpShort.gameObject.SetActive(isArmor);
            clothesDownShort.gameObject.SetActive(isArmor);
            armLeftShort.gameObject.SetActive(isArmor);
            armRightShort.gameObject.SetActive(isArmor);

            clothesUpLong.gameObject.SetActive(!isArmor);
            clothesDownLong.gameObject.SetActive(!isArmor);
            armLeftLong.gameObject.SetActive(!isArmor);
            armRightLong.gameObject.SetActive(!isArmor);

            ClothesUp.spriteMesh = ImageAssetBundleManager.GetClothesUpMesh(newvalue);
            ClothesDown.spriteMesh = ImageAssetBundleManager.GetClothesDownMesh(newvalue);
        }
        public override void UpdateView(){
            this.Init();
            this.HatChanged(0, ViewModel.Hat.Value);
            this.HeadChanged(0, ViewModel.Head.Value);
            this.ClothesChanged(0, ViewModel.Clothes.Value);
            this.WeaponChanged(0, ViewModel.Weapon.Value);
            this.HorseChanged(0, ViewModel.Horse.Value);
            this.MoveTypeChanged(ViewModel.MoveType.Value, ViewModel.MoveType.Value);
        }
        #endregion

        public void AttackToHert(){
            //Debug.LogError("AttackToHert:"+ViewModel.Name+", " + ViewModel.Target.Value);
            if (ViewModel.Target.Value == null)
            {
                return;
            }
            if (ViewModel.CurrentSkill.Value.UseToEnemy)
            {
                this.Controller.SendMessage("OnDamage", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                this.Controller.SendMessage("OnHeal", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        public void ChangeAction(App.Model.ActionType type){
            animationIndex = 0;
            ViewModel.Action.Value = type;
        }
        public void ActionEnd(){
            ChangeAction(App.Model.ActionType.idle);
            //ChangeAction(ViewModel.ActionOver.Value ? App.Model.ActionType.idle : App.Model.ActionType.move);
        }
        public void SetOrders(Dictionary<string,int> meshOrders){
            foreach(string key in meshOrders.Keys){
                meshs[key].sortingOrder = meshOrders[key];
                Debug.LogError("SetOrders : " + key + " = " + meshs[key].sortingOrder);
            }
        }
        public void ChangeAnimationIdex(int index){
            animationIndex = index;
            //UpdateView ();
        }
        /// <summary>
        /// Empties the action.
        /// </summary>
        public void EmptyAction(){
        }
    }
}