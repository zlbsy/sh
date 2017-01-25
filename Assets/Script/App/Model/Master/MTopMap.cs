using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master{
	/**
	 * 玩家主页面原始地图
	*/
	public class MTopMap : MBase {
        public MTopMap(){
		}
        public int width;//横向格数
        public int height;//纵向格数
		public MTile[] tiles;//小地图块
	}
}