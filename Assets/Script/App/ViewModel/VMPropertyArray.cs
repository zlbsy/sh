using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.View;
using App.Model;

namespace App.ViewModel{
    public class VMPropertyArray<T> : VMProperty<T> {
        public override T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (object.ReferenceEquals(_value, value))
                {
                    Debug.LogError("OK");
                    return;
                }
                else
                {
                    MTile[] ary1 = _value as MTile[];
                    MTile[] ary2 = value as MTile[];
                    Debug.LogError((ary1 == null)+","+(ary2 == null));
                    if (ary1 == null || ary2 == null
                        || ary1.Length != ary2.Length)
                    {
                        T old = _value;
                        _value = value;Debug.LogError("ValueChanged");
                        ValueChanged(old, _value);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < ary1.Length; i++)
                        {
                            if (ary1[i].id != ary2[i].id)
                            {
                                T old = _value;
                                _value = value;Debug.LogError("ValueChanged");
                                ValueChanged(old, _value);
                            }
                        }
                    }

                }
            }
        }
	}
}