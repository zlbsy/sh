using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
	public class MSkill : MBase {
        public MSkill(){
            viewModel = new VMSkill ();
        }
        public VMSkill ViewModel { get { return (VMSkill)viewModel; } }
        public int Id{
            set{ 
                ViewModel.Id.Value = value;
            }
            get{ 
                return ViewModel.Id.Value;
            }
        }
        public int SkillId{
            set{ 
                ViewModel.SkillId.Value = value;
            }
            get{ 
                return ViewModel.SkillId.Value;
            }
        }
        public int Level{
            set{ 
                ViewModel.Level.Value = value;
            }
            get{ 
                return ViewModel.Level.Value;
            }
        }
        public App.Model.Master.MSkill Master{
            get{ 
                return App.Util.Cacher.SkillCacher.Instance.Get(this.SkillId);
            }
        }
	}
}