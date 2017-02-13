using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util;
using App.Util.Cacher;
using UnityEngine.UI;
using App.Model.Scriptable;


namespace App.Controller{
    public class CLoadingDialog : CDialog {
        [SerializeField]private Image barBackground;
        [SerializeField]private Image barPrevious;
        [SerializeField]private Text progress;
        [SerializeField]private Text message;
        [SerializeField]private Image review;
        private float width;
        private float height;
        private MPromptMessage[] promptMessages;
        private int promptMessageIndex = 0;
        public override void OnEnable(){
            message.gameObject.SetActive(false);
            review.gameObject.SetActive(false);
            width = 4 - barBackground.GetComponent<RectTransform>().rect.width;
            height = barPrevious.GetComponent<RectTransform>().offsetMax.y;
            base.OnEnable();
        }
        public override IEnumerator OnLoad( Request request ) 
        {  
            Progress = 0f;
            yield return StartCoroutine(base.OnLoad(request));
            StartCoroutine(SetDefaultPromptMessage());
		}
        public float Progress{
            get{ 
                return float.Parse(progress.text);
            }
            set{ 
                progress.text = string.Format("{0}%", Mathf.Floor(value * 100f) * 0.01f);
                barPrevious.GetComponent<RectTransform>().offsetMax = new Vector2(width * (100 - value) * 0.01f, height);
            }
        }
        public IEnumerator SetDefaultPromptMessage(){
            if (PromptMessageAsset.Data == null)
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(SetDefaultPromptMessage());
                yield break;
            }
            if (promptMessages == null)
            {
                promptMessages = PromptMessageAsset.Data.promptMessages;
                System.Array.Sort(promptMessages, (a, b)=>{return Random.Range(0f, 1f) > 0.5 ? 1 : -1;});
            }
            if (promptMessageIndex >= promptMessages.Length)
            {
                promptMessageIndex = 0;
            }
            MPromptMessage promptMessage = promptMessages[promptMessageIndex++];
            message.text = promptMessage.message;
            review.material.mainTexture = promptMessage.image;
            message.gameObject.SetActive(true);
            review.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            StartCoroutine(SetDefaultPromptMessage());
        }
        public static void ToShow(){
            SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.LoadingDialog));
        }
        public static void SetProgress(float value){
            if (Global.SceneManager.CurrentDialog is CLoadingDialog)
            {
                (Global.SceneManager.CurrentDialog as CLoadingDialog).Progress = value;
            }
        }
        public static void ToClose(){
            if (Global.SceneManager.CurrentDialog is CLoadingDialog)
            {
                Global.SceneManager.CurrentDialog.Close();
            }
        }
	}
}