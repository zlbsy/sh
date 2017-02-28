using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Util{
    public class SceneManager {
        public enum Scenes{
            Logo,
            Top,
            Stage,
            Area,
            World,
            Battlefield
        }
        public enum Prefabs{
            BuildingDialog,
            LoadingDialog,
            ConnectingDialog,
            AlertDialog,
            CharacterListDialog,
            CharacterDetailDialog,
            EquipmentListDialog,
            TalkDialog
        }
        public static App.Controller.CScene CurrentScene;
        public static App.Controller.Request CurrentSceneRequest;
        private List<App.Controller.CDialog> Dialogs = new List<App.Controller.CDialog>();
        public static void LoadScene(string name, App.Controller.Request req = null){
            App.Controller.CConnectingDialog.ToShow();
            CurrentScene.StartCoroutine(LoadSceneCoroutine(name, req));
        }
        public static IEnumerator LoadSceneCoroutine(string name, App.Controller.Request req = null){
            yield return new WaitForSeconds(0.1f);
            CurrentSceneRequest = req;
            UnityEngine.SceneManagement.SceneManager.LoadScene( name );
            Global.SceneManager.DestoryDialog();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        public IEnumerator ShowDialog(Prefabs prefab, App.Controller.Request req = null)
        {
            App.Controller.CDialog dialog = Dialogs.Find(_=>_ is App.Controller.CTalkDialog);
            if (dialog == null)
            {
                GameObject instance = LoadPrefab(prefab.ToString());
                dialog = instance.GetComponent<App.Controller.CDialog>();
                dialog.SetIndex();
                Dialogs.Add(dialog);
            }
            else
            {
                dialog.gameObject.SetActive(true);
            }
            yield return CurrentScene.StartCoroutine(dialog.OnLoad(req));
        }
        public GameObject LoadPrefab(string prefabName)
        {
            GameObject prefab = Resources.Load(string.Format("Prefabs/{0}", prefabName)) as GameObject;
            return CurrentScene.GetObject( prefab );
        }
        public bool DialogIsShow(App.Controller.CDialog checkDialog = null)
        {
            foreach(App.Controller.CDialog dialog in Dialogs){
                if(!dialog.gameObject.activeSelf){
                    continue;
                }
                if (checkDialog == null || checkDialog.index == dialog.index)
                {
                    return true;
                }
            }
            return false;
        }
        public App.Controller.CDialog CurrentDialog
        {
            get{ 
                for (int i = Dialogs.Count - 1; i >= 0; i--)
                {
                    App.Controller.CDialog dialog = Dialogs[i];
                    if (dialog.gameObject.activeSelf)
                    {
                        return dialog;
                    }
                }
                return null;
            }
        }
        public void DestoryDialog(App.Controller.CDialog deleteDialog = null)
        {
            for (int i = Dialogs.Count - 1; i >= 0; i--)
            {
                App.Controller.CDialog dialog = Dialogs[i];
                if (deleteDialog == null)
                {
                    Dialogs.RemoveAt(i);
                }
                else if (deleteDialog.index == dialog.index)
                {
                    Dialogs.RemoveAt(i);
                    GameObject.Destroy(deleteDialog.gameObject);
                    break;
                }
            }
        }
	}
}