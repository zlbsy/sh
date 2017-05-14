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
        private Anima2D.SpriteMeshInstance Weapon{
            get{ 
                return weapon;
            }
        }
        private Anima2D.SpriteMeshInstance ClothesUp{
            get{ 
                return clothesUpShort;
            }
        }
        private Anima2D.SpriteMeshInstance ClothesDown{
            get{ 
                return clothesDownShort;
            }
        }
        private Anima2D.SpriteMeshInstance ArmLeft{
            get{ 
                return armLeftShort;
            }
        }
        private Anima2D.SpriteMeshInstance ArmRight{
            get{ 
                return armRightShort;
            }
        }

        [SerializeField]SpriteRenderer imgHorse;
        [SerializeField]SpriteRenderer imgBody;
        [SerializeField]SpriteRenderer imgClothes;
        [SerializeField]SpriteRenderer imgHead;
        [SerializeField]SpriteRenderer imgHat;
        [SerializeField]SpriteRenderer imgWeapon;
        [SerializeField]RectTransform content;
        [SerializeField]Transform hpTransform;
        [SerializeField]TextMesh num;
        [SerializeField]MeshRenderer meshRenderer;
        private static Shader shaderGray;
        private static Shader shaderDefault;
        private static Dictionary<App.Model.Belong, Material> hpMaterials;
        private bool init = false;
        private int animationIndex = 0;
        private Animator _animator;
        private Animator animator{
            get{ 
                if (_animator == null)
                {
                    _animator = this.GetComponent<Animator>();
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
            if (shaderDefault == null)
            {
                shaderGray = Shader.Find("Sprites/Gray");
                shaderDefault = Shader.Find("Sprites/Default");
            }
            if (hpMaterials == null)
            {
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
                Shader shader = value ? shaderGray : shaderDefault;
                imgClothes.material.shader = shader;
                imgBody.material.shader = shader;
                imgHorse.material.shader = shader;
                imgHead.material.shader = shader;
                imgHat.material.shader = shader;
                imgWeapon.material.shader = shader;
            }
            get{ 
                return imgClothes.material.shader.Equals(shaderGray);
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
                oldVm.Action.OnValueChanged -= ActionChanged;
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
                ViewModel.Action.OnValueChanged += ActionChanged;
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
            //this.transform.localPosition = vTile.transform.localPosition;
            ViewModel.Y.Value = vTile.transform.localPosition.y;
        }
        private void ActionChanged(App.Model.ActionType oldvalue, App.Model.ActionType newvalue)
        {
            //Debug.LogError(ViewModel.Id.Value + ","+ViewModel.Name.Value + " : " + newvalue.ToString());
            animator.Play(newvalue.ToString());
            animationIndex = 0;
            UpdateView();
            if (newvalue == App.Model.ActionType.stand || newvalue == App.Model.ActionType.move)
            {
                if (ViewModel.Hp.Value == 0)
                {
                    Holoville.HOTween.HOTween.To(this.transform, 1f, new Holoville.HOTween.TweenParms().Prop("localPosition", this.transform.localPosition).OnComplete(()=>{
                        this.gameObject.SetActive(false);
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
        private void HeadChanged(int oldvalue, int newvalue)
        {
            head.spriteMesh = ImageAssetBundleManager.GetHeadMesh(newvalue).spriteMesh;
            //imgHead.sprite = ImageAssetBundleManager.GetAvatarHead(newvalue);
            //UpdateView();
        }
        private void HatChanged(int oldvalue, int newvalue)
        {
            hat.spriteMesh = ImageAssetBundleManager.GetHatMesh(newvalue).spriteMesh;
            //imgHat.sprite = ImageAssetBundleManager.GetAvatarHat(newvalue);
            //UpdateView();
        }
        private void BelongChanged(App.Model.Belong oldvalue, App.Model.Belong newvalue)
        {
            meshRenderer.material = hpMaterials[newvalue];
        }
        private void WeaponChanged(int oldvalue, int newvalue)
        {
            this.Weapon.spriteMesh = ImageAssetBundleManager.GetWeaponMesh(newvalue).spriteMesh;
            //UpdateView();
        }
        private void HorseChanged(int oldvalue, int newvalue)
        {
            horseBody.spriteMesh = ImageAssetBundleManager.GetHorseBodyMesh(newvalue).spriteMesh;
            horseSaddle.spriteMesh = ImageAssetBundleManager.GetHorseSaddleMesh(newvalue).spriteMesh;
            //UpdateView();
        }
        private void ClothesChanged(int oldvalue, int newvalue)
        {
            Debug.LogError("ClothesChanged = " + newvalue);
            Anima2D.SpriteMeshInstance clothesDownMesh = ImageAssetBundleManager.GetClothesDownMesh(newvalue);
            ClothesUp.spriteMesh = ImageAssetBundleManager.GetClothesUpMesh(newvalue).spriteMesh;
            ClothesDown.spriteMesh = clothesDownMesh.spriteMesh;
            //UpdateView();
        }
        public override void UpdateView(){
            this.Init();
            this.HatChanged(0, ViewModel.Hat.Value);
            this.HeadChanged(0, ViewModel.Head.Value);
            this.ClothesChanged(0, ViewModel.Clothes.Value);
            this.WeaponChanged(0, ViewModel.Weapon.Value);
            this.HorseChanged(0, ViewModel.Horse.Value);

            return;
            Debug.Log(ViewModel.MoveType.Value+", " + ViewModel.WeaponType.Value + ", " + ViewModel.Action.Value + ", " + animationIndex);
            AvatarAction avatarAction = AvatarAsset.Data.GetAvatarAction(ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, animationIndex);
            string key;
            //Horse
            if (avatarAction.horse == null) {
                imgHorse.gameObject.SetActive (false);
            } else {
                imgHorse.gameObject.SetActive (true);
                key = string.Format("horse_{0}_{1}_{2}", ViewModel.Horse.Value, ViewModel.Action.Value, avatarAction.horse.index);
                imgHorse.sprite = ImageAssetBundleManager.GetHorse(key);
                imgHorse.transform.localPosition = avatarAction.horse.position;
                imgHorse.sortingOrder = avatarAction.horse.sibling;
            }

            //Body
            key = string.Format("body_{0}_{1}_{2}_{3}", ViewModel.MoveType.Value, avatarAction.clothesType, ViewModel.Action.Value, avatarAction.body.index);
            imgBody.sprite = ImageAssetBundleManager.GetAvatarBody(key);
            //Clothes
            key = string.Format("clothes_{0}_{1}_{2}_{3}_{4}", ViewModel.Clothes.Value, ViewModel.MoveType.Value, avatarAction.clothesType, ViewModel.Action.Value, avatarAction.clothes.index);
            imgClothes.sprite = ImageAssetBundleManager.GetClothes(key);
            //Head
            if (imgHead.gameObject.activeSelf && avatarAction.head.index == 0)
            {
                imgHead.gameObject.SetActive(false);
            }else if (!imgHead.gameObject.activeSelf && avatarAction.head.index > 0)
            {
                imgHead.gameObject.SetActive(true);
            }
            if (imgHead.gameObject.activeSelf)
            {
                imgHead.sprite = ImageAssetBundleManager.GetAvatarHead(ViewModel.Head.Value);
                imgHat.sprite = ImageAssetBundleManager.GetAvatarHat(ViewModel.Hat.Value);
                imgHead.sortingOrder = avatarAction.head.sibling;
                imgHat.sortingOrder = avatarAction.hat.sibling;
            }

            //Weapon
            key = string.Format("weapon_{0}_{1}_{2}_{3}_{4}", ViewModel.Weapon.Value, ViewModel.MoveType.Value, ViewModel.WeaponType.Value, ViewModel.Action.Value, avatarAction.weapon.index);
            //Debug.LogError("Weapon key="+key);
            imgWeapon.sprite = ImageAssetBundleManager.GetWeapon(key);

            imgBody.transform.localPosition = avatarAction.body.position;
            imgClothes.transform.localPosition = avatarAction.clothes.position;
            imgHead.transform.localPosition = avatarAction.head.position;
            imgWeapon.transform.localPosition = avatarAction.weapon.position;

            imgBody.sortingOrder = avatarAction.body.sibling;
            imgClothes.sortingOrder = avatarAction.clothes.sibling;
            imgHead.sortingOrder = avatarAction.head.sibling;
            imgWeapon.sortingOrder = avatarAction.weapon.sibling;
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
            ChangeAction(ViewModel.ActionOver.Value ? App.Model.ActionType.stand : App.Model.ActionType.move);
        }
        public void ChangeAnimationIdex(int index){
            animationIndex = index;
            UpdateView ();
        }
        /// <summary>
        /// Empties the action.
        /// </summary>
        public void EmptyAction(){
        }
    }
}