using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.ViewModel;
using App.Util.Cacher;


namespace App.Model{
	public enum MoveType{
		infantry,//步兵
		cavalry,//骑兵
	}
	public enum WeaponType{
		shortKnife,//短刀
		longKnife,//长刀
		sword,//剑
		gun,//枪
		shortAx,//短斧
		longAx,//长斧
	}
	public enum ActionType{
		stand,
		move,
		attack,
        block,
        hert,
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
            return mCharacter;
        }
		public VMCharacter ViewModel { get { return (VMCharacter)viewModel; } }
		
        public App.Model.Master.MCharacter Master{
            get{ 
                return CharacterCacher.Instance.Get(CharacterId);
            }
        }
        public int Id{
            set{
                this.ViewModel.Id.Value = value;
            }
            get{ 
                return this.ViewModel.Id.Value;
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
                this.ViewModel.CharacterId.Value = value;
            }
            get{ 
                return this.ViewModel.CharacterId.Value;
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
	}
}