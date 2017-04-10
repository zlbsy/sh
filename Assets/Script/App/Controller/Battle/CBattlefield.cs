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


namespace App.Controller.Battle{
    public partial class CBattlefield : CBaseMap {
        [SerializeField]Text title;
        [SerializeField]VBottomMenu operatingMenu;
        [SerializeField]CBattleCharacterPreviewDialog battleCharacterPreview;
        [SerializeField]GameObject attackTween;
        [SerializeField]VBottomMenu battleMenu;
        private int battlefieldId;
        List<int> characterIds;
        private List<VCharacter> dynamicCharacters = new List<VCharacter>();
        public enum BattleMode
        {
            none,
            show_move_tiles,
            moving,
            move_end,
            attacking

        }
        private Belong currentBelong;
        public BattleMode battleMode{ get; set;}
        public BattleManager manager{ get; set;}
        public BattleTilesManager tilesManager{ get; set;}
        public BattleCharactersManager charactersManager{ get; set;}
        public BattleCalculateManager calculateManager{ get; set;}
        public CharacterAI ai{ get; set;}
        private App.Util.SceneManager.Scenes fromScene;
        private Request fromRequest;
        private int boutCount = 0;
        public override IEnumerator OnLoad( Request request ) 
        {  
            battleCharacterPreview.gameObject.SetActive(false);
            battlefieldId = request.Get<int>("battlefieldId");
            characterIds = request.Get<List<int>>("characterIds");

            fromScene = request.Get<App.Util.SceneManager.Scenes>("fromScene");
            fromRequest = request.Get<Request>("fromRequest");
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        protected override void InitMap(){
            App.Model.Master.MBattlefield battlefieldMaster = App.Util.Cacher.BattlefieldCacher.Instance.Get(battlefieldId);
            title.text = battlefieldMaster.name;
            mBaseMap = new MBaseMap();
            mBaseMap.MapId = battlefieldMaster.map_id;
            mBaseMap.Tiles = battlefieldMaster.tiles.Clone() as App.Model.MTile[];
            List<MCharacter> characters = new List<MCharacter>();
            for (int i = 0; i < characterIds.Count; i++)
            {
                MCharacter mCharacter = System.Array.Find(App.Util.Global.SUser.self.characters, _=>_.Id==characterIds[i]);
                mCharacter.Belong = Belong.self;
                //己方出战坐标
                App.Model.Master.MBattleOwn mBattleOwn = battlefieldMaster.owns[i];
                mCharacter.CoordinateX = mBattleOwn.x;
                mCharacter.CoordinateY = mBattleOwn.y;
                CharacterInit(mCharacter);
                characters.Add(mCharacter);
            }
            foreach(App.Model.Master.MBattleNpc battleNpc in battlefieldMaster.enemys){
                MCharacter mCharacter = NpcCacher.Instance.GetFromBattleNpc(battleNpc);
                mCharacter.Belong = Belong.enemy;
                CharacterInit(mCharacter);
                characters.Add(mCharacter);
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
            vBaseMap.MoveToCenter();
            base.InitMap();
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
            System.Action closeEvent = null;
            if (belong != Belong.self)
            {
                closeEvent = () =>
                    {
                        ai.Execute(belong);
                    };
            }
            Request req = Request.Create("belong", belong, "bout", boutCount, "maxBout", 20, "closeEvent", closeEvent);
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
                    manager.ClickAttackNode(index);
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
            if (manager.CurrentCharacter == null)
            {
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
            manager.ActionOver();
        }
        /// <summary>
        /// 选择技能
        /// </summary>
        public void OpenSkillList(){
            System.Action closeEvent = () =>
            {
                    tilesManager.ClearCurrentTiles();
                    tilesManager.ShowCharacterAttackArea(manager.CurrentCharacter);
            };
            Request req = Request.Create("character", this.manager.CurrentCharacter, "closeEvent", closeEvent);
            this.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.BattleSkillListDialog, req));
        }
        public void OpenBattleMenu(){
            Request req = Request.Create("title", title.text, "bout", "5/20");
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