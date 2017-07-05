using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using UnityEngine.UI;
using App.Util.Battle;
using App.View.Common;
using App.View.Character;
using App.Util;
using App.Controller.Common;
using App.View.Effect;


namespace App.Controller.Battle{
    public partial class CBattlefield : CBaseMap {
        [SerializeField]Text title;
        [SerializeField]VBottomMenu operatingMenu;
        [SerializeField]CBattleCharacterPreviewDialog battleCharacterPreview;
        [SerializeField]GameObject attackTween;
        [SerializeField]VBottomMenu battleMenu;

        public static VEffectAnimation effectAnimation;

        private int battlefieldId;
        List<int> characterIds;
        private List<VCharacter> dynamicCharacters = new List<VCharacter>();
        public enum BattleMode
        {
            none,
            show_move_tiles,
            moving,
            move_end,
            actioning,
            move_after_attack

        }
        public Belong currentBelong{ get; set;}
        public BattleMode battleMode{ get; set;}
        public BattleManager manager{ get; set;}
        public BattleTilesManager tilesManager{ get; set;}
        public BattleCharactersManager charactersManager{ get; set;}
        public BattleCalculateManager calculateManager{ get; set;}
        public CharacterAI ai{ get; set;}
        private App.Util.SceneManager.Scenes fromScene;
        private Request fromRequest;
        private int boutCount = 0;
        private int maxBout = 0;
        public override IEnumerator OnLoad( Request request ) 
        {  
            battleCharacterPreview.gameObject.SetActive(false);
            battlefieldId = request.Get<int>("battlefieldId");
            characterIds = request.Get<List<int>>("characterIds");

            fromScene = request.Get<App.Util.SceneManager.Scenes>("fromScene");
            fromRequest = request.Get<Request>("fromRequest");
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        public GameObject CreateEffect(string name, Transform trans){
            GameObject obj = Instantiate(CBattlefield.effectAnimation.gameObject);
            obj.transform.SetParent(trans);
            obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            obj.transform.localPosition = new Vector3(0f, 0.2f, 0f);
            obj.transform.localEulerAngles = new Vector3(-30f, 0f, 0f);
            VEffectAnimation.AddEffectAnimation(obj);
            VEffectAnimation effect = obj.GetComponent<VEffectAnimation>();
            effect.animator.Play(name);
            return obj;
        }
        protected override void InitMap(){
            App.Model.Master.MBattlefield battlefieldMaster = App.Util.Cacher.BattlefieldCacher.Instance.Get(battlefieldId);
            maxBout = battlefieldMaster.max_bout;
            title.text = battlefieldMaster.name;
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = battlefieldMaster.map_id;
            mBaseMap.Tiles = battlefieldMaster.tiles.Clone() as App.Model.MTile[];
            List<MCharacter> characters = new List<MCharacter>();
            for (int i = 0; i < characterIds.Count; i++)
            {
                MCharacter mCharacter = System.Array.Find(App.Util.Global.SUser.self.characters, _=>_.CharacterId==characterIds[i]);
                mCharacter.Belong = Belong.self;
                //己方出战坐标
                App.Model.Master.MBattleOwn mBattleOwn = battlefieldMaster.owns[i];
                mCharacter.CoordinateX = mBattleOwn.x;
                mCharacter.CoordinateY = mBattleOwn.y;
                CharacterInit(mCharacter);
                characters.Add(mCharacter);
                mCharacter.Hp -= 50;
            }
            foreach(App.Model.Master.MBattleNpc battleNpc in battlefieldMaster.enemys){
                MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
                mCharacter.Belong = Belong.enemy;
                CharacterInit(mCharacter);
                characters.Add(mCharacter);
                //mCharacter.Hp = 1;
            }
            foreach(App.Model.Master.MBattleNpc battleNpc in battlefieldMaster.friends){
                MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
                mCharacter.Belong = Belong.friend;
                CharacterInit(mCharacter);
                characters.Add(mCharacter);
            }
            mBaseMap.Characters = characters.ToArray();
            vBaseMap.BindingContext = mBaseMap.ViewModel;
            vBaseMap.UpdateView();
            vBaseMap.transform.parent.localScale = Vector3.one;
            vBaseMap.MoveToPosition();
            base.InitMap();
            characters.ForEach(character=>character.Action = ActionType.idle);
            battlefieldMaster.script.Add("Character.hide(3);");
            battlefieldMaster.script.Add("Character.hide(4);");
            battlefieldMaster.script.Add("Character.hide(8);");
            battlefieldMaster.script.Add("Talk.setnpc(5,0,来了个不怕死的黄毛小鬼！把他一起干掉！,false);");
            battlefieldMaster.script.Add("Talk.setplayer(@player_id,0,（那个山大王凶神恶煞的样子好可怕啊，怎么办？怎么办？）,true);");
            battlefieldMaster.script.Add("Talk.setnpc(1,0,多谢小英雄仗义出手，前面两个强盗我可以应付！,false);");
            battlefieldMaster.script.Add("Talk.setnpc(1,0,左面的小强盗比较弱一些，就麻烦小英雄先挡一挡了！,false);");
            battlefieldMaster.script.Add("Talk.setplayer(@player_id,0,（正合我意）,true);");
            battlefieldMaster.script.Add("Talk.setplayer(@player_id,0,王教头放心，交给我了。,true);");
            battlefieldMaster.script.Add("Character.move(1,2,3);");

            battlefieldMaster.script.Add("Battle.boutwave(self);");
            App.Util.LSharp.LSharpScript.Instance.Analysis(battlefieldMaster.script);
        }
        public void BoutWave(Belong belong){
            currentBelong = belong;
            if (belong == Belong.self)
            {
                boutCount++;
            }
            charactersManager.ActionRestore();
            System.Action closeEvent = ()=>{
                this.StartCoroutine(OnBoutStart());
            };
            foreach(MCharacter mCharacter in mBaseMap.Characters){
                mCharacter.boutEventComplete = false;
            }
            Request req = Request.Create("belong", belong, "bout", boutCount, "maxBout", maxBout, "closeEvent", closeEvent);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BoutWaveDialog, req));
        }
        /// <summary>
        /// 武将初始化
        /// </summary>
        /// <param name="mCharacter">M character.</param>
        private void CharacterInit(MCharacter mCharacter){
            mCharacter.StatusInit();
        }
        public override void OnClickTile(int index){
            if (currentBelong != Belong.self)
            {
                return;
            }
            switch (battleMode)
            {
                case BattleMode.none:
                    manager.ClickNoneNode(index);
                    break;
                case BattleMode.show_move_tiles:
                    manager.ClickMovingNode(index);
                    break;
                case BattleMode.move_end:
                    manager.ClickSkillNode(index);
                    break;
                case BattleMode.move_after_attack:
                    manager.ClickMovingNode(index);
                    break;
            }
        }
        protected override void InitManager(){
            base.InitManager();
            manager = new BattleManager(this, mBaseMap, vBaseMap);
            tilesManager = new BattleTilesManager(this, mBaseMap, vBaseMap);
            charactersManager = new BattleCharactersManager(this, mBaseMap, vBaseMap);
            calculateManager = new BattleCalculateManager(this, mBaseMap, vBaseMap);
            ai = new CharacterAI(this, mBaseMap, vBaseMap);
        }
        /// <summary>
        /// 返回一个动态的攻击图标
        /// </summary>
        /// <returns>The attack tween.</returns>
        public GameObject CreateAttackTween(){
            GameObject obj = GameObject.Instantiate(attackTween);
            obj.SetActive(true);
            return obj;
        }
        #region 操作菜单
        public void OpenOperatingMenu(){
            operatingMenu.Open();
            battleMenu.Close(null);
        }
        public void CloseOperatingMenu(){
            operatingMenu.Close(null);
            if (manager.CurrentCharacter != null){
                return;
            }
            if (currentBelong != Belong.self)
            {
                ai.Execute(currentBelong);
            }else{
                battleMenu.Open();
            }
        }
        #endregion

        #region 武将状态
        /// <summary>
        /// 武将状态显示
        /// </summary>
        /// <param name="mCharacter">M character.</param>
        public void OpenBattleCharacterPreviewDialog(MCharacter mCharacter){
            battleCharacterPreview.gameObject.SetActive(true);
            Request req = Request.Create("character", mCharacter);
            this.StartCoroutine(battleCharacterPreview.OnLoad(req));
        }
        /// <summary>
        /// 武将状态隐藏
        /// </summary>
        public void HideBattleCharacterPreviewDialog(){
            battleCharacterPreview.Close();
        }
        #endregion
        /// <summary>
        /// 待命
        /// </summary>
        public void AwaitOrders(){
            this.StartCoroutine(manager.ActionOver());
        }
        /// <summary>
        /// 选择技能
        /// </summary>
        public void OpenSkillList(){
            System.Action closeEvent = () =>
            {
                    tilesManager.ClearCurrentTiles();
                    if(this.battleMode == BattleMode.show_move_tiles){
                        tilesManager.ShowCharacterMovingArea(manager.CurrentCharacter);
                    }
                    tilesManager.ShowCharacterSkillArea(manager.CurrentCharacter);
            };
            Request req = Request.Create("character", this.manager.CurrentCharacter, "closeEvent", closeEvent);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleSkillListDialog, req));
        }
        public void OpenBattleMenu(){
            Request req = Request.Create("title", title.text, "bout", string.Format("{0}/{1}", boutCount, maxBout));
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleMenuDialog, req));
        }
        /// <summary>
        /// 结束回合
        /// </summary>
        public void CtrlEnd(){
            CConfirmDialog.Show("结束本回合操作吗？",()=>{manager.ChangeBelong(Belong.self);});
        }
        /// <summary>
        /// 结束战斗
        /// </summary>
        public void BattleEnd(){
            App.Util.SceneManager.LoadScene( this.fromScene.ToString(), this.fromRequest );
        }
        /// <summary>
        /// 结束胜利
        /// </summary>
        public void BattleWin(){
            MCharacter[] characters = App.Util.Global.SUser.self.characters;
            Request req = Request.Create("characterIds", characterIds, 
                "dieIds", characterIds.FindAll(id=>System.Array.Exists(characters, m=>m.Id == id && m.Hp == 0)),
                "star", 2
            );
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleWinDialog, req));
        }
        public override void OnDestroy(){
            CBattlefield.effectAnimation = null;
            base.OnDestroy();
        }
        #region 行动中武将
        /// <summary>
        /// 行动中武将挂起
        /// </summary>
        /// <param name="mCharacter">M character.</param>
        public void AddDynamicCharacter(MCharacter mCharacter){
            AddDynamicCharacter(GetCharacterView(mCharacter));
        }
        /// <summary>
        /// 行动中武将挂起
        /// </summary>
        /// <param name="vCharacter">V character.</param>
        public void AddDynamicCharacter(VCharacter vCharacter){
            if (dynamicCharacters.Contains(vCharacter))
            {
                return;
            }
            //Debug.LogError("行动中武将 " + vCharacter.ViewModel.Action.Value + ","+vCharacter.ViewModel.Name.Value + ","+vCharacter.ViewModel.Belong.Value);
            dynamicCharacters.Add(vCharacter);
        }
        /// <summary>
        /// 行动中武将移除
        /// </summary>
        /// <param name="mCharacter">M character.</param>
        public void RemoveDynamicCharacter(MCharacter mCharacter){
            RemoveDynamicCharacter(GetCharacterView(mCharacter));
        }
        /// <summary>
        /// 行动中武将移除
        /// </summary>
        /// <param name="vCharacter">V character.</param>
        public void RemoveDynamicCharacter(VCharacter vCharacter){
            dynamicCharacters.Remove(vCharacter);
            if (!HasDynamicCharacter())
            {
                this.ActionEnd();
            }
        }
        /// <summary>
        /// 是否存在行动中武将
        /// </summary>
        /// <returns><c>true</c> if this instance has dynamic character; otherwise, <c>false</c>.</returns>
        public bool HasDynamicCharacter(){
            return dynamicCharacters.Count > 0;   
        }
        #endregion
	}
}