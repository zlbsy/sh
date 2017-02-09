using System.Collections;
using System.Collections.Generic;
using App.ViewModel;


namespace App.Model{
	public class MUser : MBase {
        public MUser(){
            viewModel = new VMUser ();
        }
        public VMUser ViewModel { get { return (VMUser)viewModel; } }
        public string name;
        public string password;
        public string Nickname{
            get{ 
                return this.ViewModel.Nickname.Value;
            }
            set{ 
                this.ViewModel.Nickname.Value = value;
            }
        }
        public int Level{
            get{ 
                return this.ViewModel.Level.Value;
            }
            set{ 
                this.ViewModel.Level.Value = value;
            }
        }
        public int Gold{
            get{ 
                return this.ViewModel.Gold.Value;
            }
            set{ 
                this.ViewModel.Gold.Value = value;
            }
        }
        public int Silver{
            get{ 
                return this.ViewModel.Silver.Value;
            }
            set{ 
                this.ViewModel.Silver.Value = value;
            }
        }
        public int Ap{
            get{ 
                return this.ViewModel.Ap.Value;
            }
            set{ 
                this.ViewModel.Ap.Value = value;
            }
        }
        public int MaxAp{
            get{ 
                return 99;
            }
        }
        public int GetCurrentAp(System.DateTime now)
        {
            int actionPoint = this.Ap;
            if(this.Ap < this.MaxAp){
                System.DateTime lastStaminaDate = this.LastApDate;
                System.TimeSpan ts = now - lastStaminaDate;
                int totalSeconds = (int)ts.TotalSeconds;
                actionPoint = (int)(totalSeconds / App.Util.Global.Constant.recover_ap_time) + this.Ap;
                if(actionPoint > this.MaxAp){
                    actionPoint = this.MaxAp;
                }
            }
            return actionPoint;
        }
        public int MapId{
            get{ 
                return this.ViewModel.MapId.Value;
            }
            set{ 
                this.ViewModel.MapId.Value = value;
            }
        }
        public MTile[] TopMap{
            get{ 
                return this.ViewModel.TopMap.Value;
            }
            set{ 
                this.ViewModel.TopMap.Value = value;
            }
        }
        public System.DateTime LastApDate{
            get{ 
                return this.ViewModel.LastApDate.Value;
            }
            set{ 
                this.ViewModel.LastApDate.Value = value;
            }
        }
        public MTile GetTile(int x, int y){
            return System.Array.Find(TopMap, _=>_.x == x && _.y == y);
        }
	}
}