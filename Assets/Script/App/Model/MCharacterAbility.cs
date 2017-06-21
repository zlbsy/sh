using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;
using App.Util.Cacher;


namespace App.Model{
	public class MCharacterAbility : MBase {
        public MCharacterAbility(){
            viewModel = new VMCharacterAbility ();
        }
        public static MCharacterAbility Create(MCharacter mCharacter){
            MCharacterAbility ability = new MCharacterAbility();
            ability.Update(mCharacter);
            return ability;
        }
        public VMCharacterAbility ViewModel { get { return (VMCharacterAbility)viewModel; } }
        public void Update(MCharacter mCharacter){
            App.Model.Master.MCharacter master = mCharacter.Master;
            if (master == null)
            {
                return;
            }
            App.Model.MSkill[] skills = mCharacter.Skills;
            this.Power = master.power;
            this.Knowledge = master.knowledge;
            this.Speed = master.speed;
            this.Trick = master.trick;
            this.Endurance = master.endurance;
            this.MovingPower = master.moving_power;
            this.Riding = master.riding;
            this.Walker = master.walker;
            this.Pike = master.pike;
            this.Sword = master.sword;
            this.LongKnife = master.long_knife;
            this.Knife = master.knife;
            this.LongAx = master.long_ax;
            this.Ax = master.ax;
            this.LongSticks = master.long_sticks;
            this.Sticks = master.sticks;
            this.Archery = master.archery;
            this.HiddenWeapons = master.hidden_weapons;
            this.DualWield = master.dual_wield;
            int hp = 100;
            int mp = 10;
            foreach (App.Model.MSkill skill in skills)
            {
                App.Model.Master.MSkill skillMaster = skill.Master;
                if (!System.Array.Exists(skillMaster.types, s=>s==SkillType.ability))
                {
                    continue;
                }
                hp += skillMaster.hp;
                mp += skillMaster.mp;
                this.Power += skillMaster.power;
                this.Knowledge += skillMaster.knowledge;
                this.Speed += skillMaster.speed;
                this.Trick += skillMaster.trick;
                this.Endurance += skillMaster.endurance;
                this.MovingPower += skillMaster.moving_power;
                this.Riding += skillMaster.riding;
                this.Walker += skillMaster.walker;
                this.Pike += skillMaster.pike;
                this.Sword += skillMaster.sword;
                this.LongKnife += skillMaster.long_knife;
                this.Knife += skillMaster.knife;
                this.LongAx += skillMaster.long_ax;
                this.Ax += skillMaster.ax;
                this.LongSticks += skillMaster.long_sticks;
                this.Sticks += skillMaster.sticks;
                this.Archery += skillMaster.archery;
                this.HiddenWeapons += skillMaster.hidden_weapons;
                this.DualWield += skillMaster.dual_wield;
            }
            if (mCharacter.MoveType == MoveType.cavalry)
            {
                this.MovingPower += 1;
            }
            this.HpMax = mCharacter.Level * 2 + master.hp + this.Endurance + hp;
            this.MpMax = mCharacter.Level + master.mp + this.Knowledge + mp;
            this.PhysicalAttack = Mathf.FloorToInt((this.Power * 2 + this.Knowledge) * (0.5f + (mCharacter.MoveType == MoveType.cavalry ? this.Riding : this.Walker) * 0.5f * 0.01f) * (1f + mCharacter.Level * 0.01f));
            this.MagicAttack = Mathf.FloorToInt(mCharacter.Level + (this.Trick * 2 + this.Knowledge) * (mCharacter.MoveType == MoveType.cavalry ? this.Riding : this.Walker) * 0.01f);
            this.PhysicalDefense = Mathf.FloorToInt((this.Power + this.Knowledge * 0.5f) * (1f + mCharacter.Level * 0.01f));
            this.MagicDefense = mCharacter.Level + this.Trick + this.Knowledge;
        }
        /// <summary>
        /// 物攻 = Lv + (力量*2+技巧)*(骑术|步战)/100
        /// </summary>
        public int PhysicalAttack{
            set{
                this.ViewModel.PhysicalAttack.Value = value;
            }
            get{ 
                return this.ViewModel.PhysicalAttack.Value;
            }
        }
        /// <summary>
        /// 法攻 = Lv + (谋略*2+技巧)*(骑术|步战)/100
        /// </summary>
        public int MagicAttack{
            set{
                this.ViewModel.MagicAttack.Value = value;
            }
            get{ 
                return this.ViewModel.MagicAttack.Value;
            }
        }
        /// <summary>
        /// 物防 = Lv + 力量+技巧
        /// </summary>
        public int PhysicalDefense{
            set{
                this.ViewModel.PhysicalDefense.Value = value;
            }
            get{ 
                return this.ViewModel.PhysicalDefense.Value;
            }
        }
        /// <summary>
        /// 法防 = Lv + 谋略+技巧
        /// </summary>
        public int MagicDefense{
            set{
                this.ViewModel.MagicDefense.Value = value;
            }
            get{ 
                return this.ViewModel.MagicDefense.Value;
            }
        }
        /// <summary>
        /// 力量 = 初始 + 技能
        /// </summary>
        public int Power{
            set{
                this.ViewModel.Power.Value = value;
            }
            get{ 
                return this.ViewModel.Power.Value;
            }
        }
        /// <summary>
        /// 技巧 = 初始 + 技能
        /// </summary>
        public int Knowledge{
            set{
                this.ViewModel.Knowledge.Value = value;
            }
            get{ 
                return this.ViewModel.Knowledge.Value;
            }
        }
        /// <summary>
        /// 速度 = 初始 + 技能
        /// </summary>
        public int Speed{
            set{
                this.ViewModel.Speed.Value = value;
            }
            get{ 
                return this.ViewModel.Speed.Value;
            }
        }
        /// <summary>
        /// 谋略 = 初始 + 技能
        /// </summary>
        public int Trick{
            set{
                this.ViewModel.Trick.Value = value;
            }
            get{ 
                return this.ViewModel.Trick.Value;
            }
        }
        /// <summary>
        /// 耐力 = 初始 + 技能
        /// </summary>
        public int Endurance{
            set{
                this.ViewModel.Endurance.Value = value;
            }
            get{ 
                return this.ViewModel.Endurance.Value;
            }
        }
        /// <summary>
        /// 轻功 = 初始 + 技能
        /// </summary>
        public int MovingPower{
            set{
                this.ViewModel.MovingPower.Value = value;
            }
            get{ 
                return this.ViewModel.MovingPower.Value;
            }
        }
        /// <summary>
        /// 骑术 = 初始 + 技能
        /// </summary>
        public int Riding{
            set{
                this.ViewModel.Riding.Value = value;
            }
            get{ 
                return this.ViewModel.Riding.Value;
            }
        }
        /// <summary>
        /// 步战 = 初始 + 技能
        /// </summary>
        public int Walker{
            set{
                this.ViewModel.Walker.Value = value;
            }
            get{ 
                return this.ViewModel.Walker.Value;
            }
        }
        /// <summary>
        /// 长枪 = 初始 + 技能
        /// </summary>
        public int Pike{
            set{
                this.ViewModel.Pike.Value = value;
            }
            get{ 
                return this.ViewModel.Pike.Value;
            }
        }
        /// <summary>
        /// 短剑 = 初始 + 技能
        /// </summary>
        public int Sword{
            set{
                this.ViewModel.Sword.Value = value;
            }
            get{ 
                return this.ViewModel.Sword.Value;
            }
        }
        /// <summary>
        /// 大刀 = 初始 + 技能
        /// </summary>
        public int LongKnife{
            set{
                this.ViewModel.LongKnife.Value = value;
            }
            get{ 
                return this.ViewModel.LongKnife.Value;
            }
        }
        /// <summary>
        /// 短刀 = 初始 + 技能
        /// </summary>
        public int Knife{
            set{
                this.ViewModel.Knife.Value = value;
            }
            get{ 
                return this.ViewModel.Knife.Value;
            }
        }
        /// <summary>
        /// 长斧 = 初始 + 技能
        /// </summary>
        public int LongAx{
            set{
                this.ViewModel.LongAx.Value = value;
            }
            get{ 
                return this.ViewModel.LongAx.Value;
            }
        }
        /// <summary>
        /// 短斧 = 初始 + 技能
        /// </summary>
        public int Ax{
            set{
                this.ViewModel.Ax.Value = value;
            }
            get{ 
                return this.ViewModel.Ax.Value;
            }
        }
        /// <summary>
        /// 长棍棒 = 初始 + 技能
        /// </summary>
        public int LongSticks{
            set{
                this.ViewModel.LongSticks.Value = value;
            }
            get{ 
                return this.ViewModel.LongSticks.Value;
            }
        }
        /// <summary>
        /// 短棍棒 = 初始 + 技能
        /// </summary>
        public int Sticks{
            set{
                this.ViewModel.Sticks.Value = value;
            }
            get{ 
                return this.ViewModel.Sticks.Value;
            }
        }
        /// <summary>
        /// 箭术 = 初始 + 技能
        /// </summary>
        public int Archery{
            set{
                this.ViewModel.Archery.Value = value;
            }
            get{ 
                return this.ViewModel.Archery.Value;
            }
        }
        /// <summary>
        /// 暗器 = 初始 + 技能
        /// </summary>
        public int HiddenWeapons{
            set{
                this.ViewModel.HiddenWeapons.Value = value;
            }
            get{ 
                return this.ViewModel.HiddenWeapons.Value;
            }
        }
        /// <summary>
        /// 双手 = 初始 + 技能
        /// </summary>
        public int DualWield{
            set{
                this.ViewModel.DualWield.Value = value;
            }
            get{ 
                return this.ViewModel.DualWield.Value;
            }
        }
        /// <summary>
        /// HpMax = 初始HP + 耐力*等级
        /// </summary>
        public int HpMax{
            set{
                this.ViewModel.HpMax.Value = value;
            }
            get{ 
                return this.ViewModel.HpMax.Value;
            }
        }
        /// <summary>
        /// MpMax = 初始MP + 技巧*等级
        /// </summary>
        public int MpMax{
            set{
                this.ViewModel.MpMax.Value = value;
            }
            get{ 
                return this.ViewModel.MpMax.Value;
            }
        }
	}
}