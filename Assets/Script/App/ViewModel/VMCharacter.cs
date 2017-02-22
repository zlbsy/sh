﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.View;
using App.Model;

namespace App.ViewModel
{
	public class VMCharacter : VMBase
    {
        public VMProperty<int> CharacterId = new VMProperty<int>();
        public VMProperty<int> Hp = new VMProperty<int>();
        public VMProperty<int> Mp = new VMProperty<int>();
        public VMProperty<string> Name = new VMProperty<string>();
        public VMProperty<string> Nickname = new VMProperty<string>();
        public VMProperty<int> Level = new VMProperty<int>();
        public VMProperty<int> Star = new VMProperty<int>();
		public VMProperty<int> Head = new VMProperty<int>();
		public VMProperty<int> Hat = new VMProperty<int>();
		public VMProperty<int> Horse = new VMProperty<int>();
		public VMProperty<int> Weapon = new VMProperty<int>();
		public VMProperty<int> Clothes = new VMProperty<int>();
		public VMProperty<WeaponType> WeaponType = new VMProperty<WeaponType>();
		public VMProperty<MoveType> MoveType = new VMProperty<MoveType>();
		public VMProperty<ActionType> Action = new VMProperty<ActionType>();
	}
}