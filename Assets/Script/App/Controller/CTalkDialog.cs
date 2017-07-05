﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using System.Linq;
using UnityEngine.UI;
using App.View.Character;
using App.Controller.Common;


namespace App.Controller{
    /// <summary>
    /// 对话框
    /// </summary>
    public class CTalkDialog : CSingleDialog {
        [SerializeField]private VFace face;
        [SerializeField]private Text characterName;
        [SerializeField]private Text characterTalk;
        private const float leftTextPosition = 410f;
        private const float leftFacePosition = -172f;
        private const float rightTextPosition = 184f;
        private const float rightFacePosition = 168f;
        private RectTransform faceTransform;
        private RectTransform nameTransform;
        private RectTransform talkTransform;
        private string message;
        public override void OnEnable(){
            faceTransform = face.GetComponent<RectTransform>();
            nameTransform = characterName.GetComponent<RectTransform>();
            talkTransform = characterTalk.GetComponent<RectTransform>();
            base.OnEnable();
        }
        public override IEnumerator OnLoad( Request request ) 
		{  
            int faceId;
            string name;
            if (request.Has("userId"))
            {
                int userId = request.Get<int>("userId");
                App.Model.MUser user = App.Util.Cacher.UserCacher.Instance.Get(userId);
                faceId = user.Face;
                name = user.name;
            }
            else if (request.Has("npcId"))
            {
                int npcId = request.Get<int>("npcId");
                CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
                App.Model.MCharacter mCharacter = cBaseMap.GetCharacterFromNpc(npcId);
                faceId = mCharacter.CharacterId;
                name = mCharacter.Master.name;
            }
            else
            {
                faceId = request.Get<int>("characterId");
                App.Model.Master.MCharacter mCharacter = CharacterCacher.Instance.Get(faceId);
                name = mCharacter.name;
            }
            message = request.Get<string>("message");
            bool isLeft = request.Get<bool>("isLeft");
            if (isLeft)
            {
                faceTransform.anchoredPosition = new Vector2(leftFacePosition, faceTransform.anchoredPosition.y);
                nameTransform.anchoredPosition = new Vector2(leftTextPosition, nameTransform.anchoredPosition.y);
                talkTransform.anchoredPosition = new Vector2(leftTextPosition, talkTransform.anchoredPosition.y);
            }
            else
            {
                faceTransform.anchoredPosition = new Vector2(rightFacePosition, faceTransform.anchoredPosition.y);
                nameTransform.anchoredPosition = new Vector2(rightTextPosition, nameTransform.anchoredPosition.y);
                talkTransform.anchoredPosition = new Vector2(rightTextPosition, talkTransform.anchoredPosition.y);
            }
            characterTalk.text = string.Empty;
            face.CharacterId = faceId;
            characterName.text = name;
            StartCoroutine(UpdateMessage());
            yield return StartCoroutine(base.OnLoad(request));
        }
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="characterId">武将ID</param>
        /// <param name="message">对话内容</param>
        /// <param name="isLeft">武将立绘是否出现在左侧</param>
        /// <param name="onComplete">对话框结束回掉</param>
        public static void ToShow(int characterId, string message, bool isLeft = true, System.Action onComplete = null){
            Request req = Request.Create("characterId",characterId,"message",message,"isLeft",isLeft,"closeEvent",onComplete);
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.TalkDialog, req));
        }
        public static void ToShowNpc(int npcId, string message, bool isLeft = true, System.Action onComplete = null){
            Request req = Request.Create("npcId",npcId,"message",message,"isLeft",isLeft,"closeEvent",onComplete);
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.TalkDialog, req));
        }
        public static void ToShowPlayer(int userId, string message, bool isLeft = true, System.Action onComplete = null){
            Request req = Request.Create("userId",userId,"message",message,"isLeft",isLeft,"closeEvent",onComplete);
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.TalkDialog, req));
        }
        IEnumerator UpdateMessage(){
            int index = characterTalk.text.Length;
            if (index >= message.Length)
            {
                yield break;
            }
            characterTalk.text = message.Substring(0, index + 1);
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(UpdateMessage());
        }
        public void ClickMessage(){
            if (characterTalk.text.Length < message.Length)
            {
                characterTalk.text = message;
            }
            else
            {
                this.Close();
            }
        }
	}
}