using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TD {
    public class Tile : MonoBehaviour{
        [SerializeField]
        public float _width;

        private int _x, _y;   //coords
        public int X {
            get {
                return _x;
            }
            set {
                //TODO: set position accordingly
                _x = value;
                transform.position = new Vector3(_x * _width + Map.X_OFFSET,
                    transform.position.y, transform.position.z);
            }
        }
        public int Y {
            get { 
                return _y;
            }
            set {
                //TODO: set position accordingly
                _y = value;
                transform.position = new Vector3(transform.position.x,
                    _y * _width + Map.Y_OFFSET, transform.position.z);
            }
        }
    }
}
