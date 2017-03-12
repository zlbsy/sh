﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;
using App.Util;
using App.View.Item;
using App.View.Equipment;
using App.Model;

namespace App.View.Common{
    public class VContentsChild : VBase {
        [SerializeField]private VItemIcon vItemIcon;
        [SerializeField]private VEquipmentIcon vEquipmentIcon;
        private bool _showComplete = false;
        public bool showComplete{
            get{ 
                return _showComplete;
            }
            set{ 
                _showComplete = value;
            }
        }
        public void UpdateView(MContent mContent){
            vItemIcon.gameObject.SetActive(false);
            vEquipmentIcon.gameObject.SetActive(false);
            switch (mContent.type)
            {
                case ContentType.item:
                    SetItem(mContent);
                    break;
                case ContentType.horse:
                case ContentType.weapon:
                case ContentType.clothes:
                    SetEquipment(mContent);
                    break;
            }
        }
        private void SetEquipment(MContent mContent){
            App.Model.MEquipment equipment = new MEquipment();
            equipment.EquipmentId = mContent.content_id;
            equipment.EquipmentType = (App.Model.Master.MEquipment.EquipmentType)System.Enum.Parse(typeof(App.Model.Master.MEquipment.EquipmentType), mContent.type.ToString(), true);
            vEquipmentIcon.gameObject.SetActive(true);
            vEquipmentIcon.BindingContext = equipment.ViewModel;
            vEquipmentIcon.UpdateView();
        }
        private void SetItem(MContent mContent){
            App.Model.MItem item = new MItem();
            item.ItemId = mContent.content_id;
            item.Cnt = 1;
            vItemIcon.gameObject.SetActive(true);
            vItemIcon.BindingContext = item.ViewModel;
            vItemIcon.UpdateView();
        }
    }
}