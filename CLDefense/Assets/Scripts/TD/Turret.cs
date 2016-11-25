using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TD {
    public class Turret : MonoBehaviour {
        public static Turret Build(string type, Tile tile) {
            if(tile == null)
                throw new Exception("Tile not found");
            if(tile.turret != null)
                throw new InvalidOperationException("Turret already exists on this tile");
            //TODO: figure out how to choose turrets with the same name
            Turret prefab = Resources.Load<Turret>("Prefabs/Turrets/" + type);
            Turret turret = Instantiate<Turret>(prefab);
            turret.transform.SetParent(tile.transform);
            turret.transform.localPosition = Vector3.zero;
            tile.turret = turret;
            return turret;
        }
    }
}
