using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;
using App.Model;
using App.View;
using App.Util.Cacher;
using App.View.Character;
using UnityEngine.UI;
using App.Util;
using App.Controller.Common;


namespace App.Controller.Register{
    public class CRegisterConfirmDialog : CDialog {
        [SerializeField]private InputField account;
        [SerializeField]private InputField password;
        [SerializeField]private InputField passwordCheck;
        [SerializeField]private InputField name;
        private int selectId;
        public override IEnumerator OnLoad( Request request ) 
        {  
            selectId = request.Get<int>("selectId");

            yield return this.StartCoroutine(base.OnLoad(request));
        }
        public void Submit(){
            string accountText = account.text.Trim();
            if (string.IsNullOrEmpty(accountText) || accountText.Length < 6)
            {
                CAlertDialog.Show("账号长度不够");
                return;
            }
            string passwordText = password.text.Trim();
            if (string.IsNullOrEmpty(accountText) || accountText.Length < 8)
            {
                CAlertDialog.Show("密码长度不够");
                return;
            }
            string passwordCheckText = passwordCheck.text.Trim();
            if (passwordText != passwordCheckText)
            {
                CAlertDialog.Show("两次密码不一致");
                return;
            }
            string nameText = name.text.Trim();
            if (string.IsNullOrEmpty(nameText) || nameText.Length < 6)
            {
                CAlertDialog.Show("明子长度不够");
                return;
            }

            SRegister register = new SRegister();
            this.StartCoroutine(register.RequestInsert(selectId, accountText, passwordText, nameText));
        }
	}
}