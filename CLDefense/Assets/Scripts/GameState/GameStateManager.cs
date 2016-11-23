using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameState {
    //TODO: make this a proper singleton
    public class GameStateManager : MonoBehaviour {
        private static GameStateManager _instance;
        public static GameStateManager Instance {
            get {
                return _instance;
            }
        }
        private void Awake() {
            _instance = this;
        }

    }
}
