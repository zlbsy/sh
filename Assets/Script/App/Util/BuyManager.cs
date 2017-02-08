using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model;

namespace App.Util{
    public class BuyManager{
        public static bool CanBuy(int price, string priceType = "silver"){
            MUser mUser = Global.SUser.user;
            if (priceType == "gold")
            {
                return mUser.gold >= price;
            }else if (priceType == "silver")
            {
                return mUser.silver >= price;
            }
            return false;
        }
    }
}