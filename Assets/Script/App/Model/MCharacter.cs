using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;
using App.Util.Cacher;


namespace App.Model{
    public enum MoveType{
        /// <summary>
        /// 步兵
        /// </summary>
        infantry,
        /// <summary>
        /// 骑兵
        /// </summary>
        cavalry,
    }
    [System.Serializable]
    public enum WeaponType{
        /// <summary>
        /// 短刀
        /// </summary>
        knife,
        /// <summary>
        /// 大刀
        /// </summary>
        longKnife,
        /// <summary>
        /// 短斧
        /// </summary>
        ax,
        /// <summary>
        /// 长斧
        /// </summary>
        longAx,
        /// <summary>
        /// 长枪
        /// </summary>
        pike,
        /// <summary>
        /// 剑
        /// </summary>
        sword,
        /// <summary>
        /// 弓箭
        /// </summary>
        archery,
        /// <summary>
        /// 棍棒
        /// </summary>
        sticks,
        /// <summary>
        /// 拳脚
        /// </summary>
        fist,
        /// <summary>
        /// 双手
        /// </summary>
        dualWield,
    }
    public enum ActionType{
        stand,
        move,
        attack,
        block,
        hert,
    }
    public enum Direction{
        left,
        right,
        leftUp,
        leftDown,
        rightUp,
        rightDown
    }
    public enum Belong{
        self,
        friend,
        enemy
    }
    public enum Mission{
        /// <summary>
        /// 主动出击
        /// </summary>
        initiative,
        /// <summary>
        /// 被动出击
        /// </summary>
        passive,
        /// <summary>
        /// 原地防守
        /// </summary>
        defensive
    }
    public class MCharacter : MBase {
        public MCharacter(){
            viewModel = new VMCharacter ();
        }
        public static MCharacter Create(App.Model.Master.MNpc npc){
            MCharacter mCharacter = new MCharacter();
            mCharacter.Id = npc.id;
            mCharacter.CharacterId = npc.character_id;
            mCharacter.Horse = npc.horse;
            mCharacter.Clothes = npc.clothes;
            mCharacter.Weapon = npc.weapon;
            mCharacter.MoveType = (MoveType)System.Enum.Parse(typeof(MoveType), npc.move_type, true);
            mCharacter.WeaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), npc.weapon_type, true);
            return mCharacter;
        }
        public VMCharacter ViewModel { get { return (VMCharacter)viewModel; } }
        /// <summary>
        /// 枪剑类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is pike; otherwise, <c>false</c>.</value>
        public bool IsPike{
            get{ 
                return this.WeaponType == WeaponType.pike || this.WeaponType == WeaponType.sword;
            }
        }
        /// <summary>
        /// 斧类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is ax; otherwise, <c>false</c>.</value>
        public bool IsAx{
            get{ 
                return this.WeaponType == WeaponType.ax || this.WeaponType == WeaponType.longAx;
            }
        }
        /// <summary>
        /// 刀类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is knife; otherwise, <c>false</c>.</value>
        public bool IsKnife{
            get{ 
                return this.WeaponType == WeaponType.longKnife || this.WeaponType == WeaponType.knife;
            }
        }
        /// <summary>
        /// 长兵器
        /// </summary>
        /// <value><c>true</c> if this instance is long weapon; otherwise, <c>false</c>.</value>
        public bool IsLongWeapon{
            get{ 
                return this.WeaponType == WeaponType.longKnife || this.WeaponType == WeaponType.longAx || this.WeaponType == WeaponType.pike || this.WeaponType == WeaponType.sticks;
            }
        }
        /// <summary>
        /// 短兵器
        /// </summary>
        /// <value><c>true</c> if this instance is short weapon; otherwise, <c>false</c>.</value>
        public bool IsShortWeapon{
            get{ 
                return this.WeaponType == WeaponType.knife || this.WeaponType == WeaponType.ax || this.WeaponType == WeaponType.sword || this.WeaponType == WeaponType.fist;
            }
        }
        /// <summary>
        /// 远程类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is archery; otherwise, <c>false</c>.</value>
        public bool IsArcheryWeapon{
            get{ 
                return this.WeaponType == WeaponType.archery;
            }
        }
        public App.Model.Master.MCharacter Master{
            get{ 
                return CharacterCacher.Instance.Get(CharacterId);
            }
        }
        public void StatusInit(){
            if (this.CurrentSkill == null)
            {
                this.CurrentSkill = System.Array.Find(this.Skills, _=>System.Array.IndexOf(_.Master.weapon_types, this.WeaponType) >= 0);
            }
            if (this.Ability == null)
            {
                this.Ability = MCharacterAbility.Create(this);
            }
            else
            {
                this.Ability.Update(this);
            }
            this.Hp = this.Ability.HpMax;
            this.Mp = this.Ability.MpMax;
        }
        public int Id{
            set{
                this.ViewModel.Id.Value = value;
            }
            get{ 
                return this.ViewModel.Id.Value;
            }
        }
        public Mission Mission{
            set{
                this.ViewModel.Mission.Value = value;
            }
            get{ 
                return this.ViewModel.Mission.Value;
            }
        }
        public MSkill CurrentSkill{
            set{
                this.ViewModel.CurrentSkill.Value = value;
            }
            get{ 
                return this.ViewModel.CurrentSkill.Value;
            }
        }
        public int UserId{
            set{
                this.ViewModel.UserId.Value = value;
            }
            get{ 
                return this.ViewModel.UserId.Value;
            }
        }
        public int CharacterId{
            set{
                App.Model.Master.MCharacter master = CharacterCacher.Instance.Get(value);
                this.ViewModel.Name.Value = master.name;
                this.ViewModel.Nickname.Value = master.nickname;
                this.ViewModel.Head.Value = master.head;
                this.ViewModel.Hat.Value = master.hat;
                this.ViewModel.CharacterId.Value = value;
            }
            get{ 
                return this.ViewModel.CharacterId.Value;
            }
        }
        public bool ActionOver{
            set{
                this.ViewModel.ActionOver.Value = value;
            }
            get{ 
                return this.ViewModel.ActionOver.Value;
            }
        }
        public int Horse{
            set{
                this.ViewModel.Horse.Value = value;
            }
            get{ 
                return this.ViewModel.Horse.Value;
            }
        }
        public int CoordinateX{
            set{
                this.ViewModel.CoordinateX.Value = value;
            }
            get{ 
                return this.ViewModel.CoordinateX.Value;
            }
        }
        public int CoordinateY{
            set{
                this.ViewModel.CoordinateY.Value = value;
            }
            get{ 
                return this.ViewModel.CoordinateY.Value;
            }
        }
        public float X{
            set{
                this.ViewModel.X.Value = value;
            }
            get{ 
                return this.ViewModel.X.Value;
            }
        }
        public float Y{
            set{
                this.ViewModel.Y.Value = value;
            }
            get{ 
                return this.ViewModel.Y.Value;
            }
        }
        public Direction Direction{
            set{
                this.ViewModel.Direction.Value = value;
            }
            get{ 
                return this.ViewModel.Direction.Value;
            }
        }
        public int Hp{
            set{
                this.ViewModel.Hp.Value = value;
            }
            get{ 
                return this.ViewModel.Hp.Value;
            }
        }
        public int Mp{
            set{
                this.ViewModel.Mp.Value = value;
            }
            get{ 
                return this.ViewModel.Mp.Value;
            }
        }
        public int Level{
            set{
                this.ViewModel.Level.Value = value;
            }
            get{ 
                return this.ViewModel.Level.Value;
            }
        }
        public int Star{
            set{
                this.ViewModel.Star.Value = value;
            }
            get{ 
                return this.ViewModel.Star.Value;
            }
        }
        public Belong Belong{
            set{
                this.ViewModel.Belong.Value = value;
            }
            get{ 
                return this.ViewModel.Belong.Value;
            }
        }
        public ActionType Action{
            set{
                this.ViewModel.Action.Value = value;
            }
            get{ 
                return this.ViewModel.Action.Value;
            }
        }
        public WeaponType WeaponType{
            set{
                this.ViewModel.WeaponType.Value = value;
            }
            get{ 
                return this.ViewModel.WeaponType.Value;
            }
        }
        public int Clothes{
            set{ 
                this.ViewModel.Clothes.Value = value;
            }
            get{ 
                return this.ViewModel.Clothes.Value;
            }
        }
        public int Weapon{
            set{ 
                this.ViewModel.Weapon.Value = value;
            }
            get{ 
                return this.ViewModel.Weapon.Value;
            }
        }
        public MoveType MoveType{
            set{ 
                this.ViewModel.MoveType.Value = value;
            }
            get{ 
                return this.ViewModel.MoveType.Value;
            }
        }
        public int Head{
            set{ 
                this.ViewModel.Head.Value = value;
            }
            get{ 
                return this.ViewModel.Head.Value;
            }
        }
        public int Hat{
            set{ 
                this.ViewModel.Hat.Value = value;
            }
            get{ 
                return this.ViewModel.Hat.Value;
            }
        }
        public MSkill[] Skills{
            set{ 
                this.ViewModel.Skills.Value = value;
            }
            get{ 
                return this.ViewModel.Skills.Value;
            }
        }
        public MCharacter Target{
            set{ 
                this.ViewModel.Target.Value = value;
            }
            get{ 
                return this.ViewModel.Target.Value;
            }
        }
        public MCharacterAbility Ability{
            set{ 
                this.ViewModel.Ability.Value = value;
            }
            get{ 
                return this.ViewModel.Ability.Value;
            }
        }
    }
}