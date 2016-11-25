using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TD {
    public class Map {
        public const float X_OFFSET = 0;
        public const float Y_OFFSET = 0;
        private const string PREFAB_PATH = "Prefabs/Tile";
        private static Tile _prefab;
        public static Map Generate(string pattern) {
            if(_prefab == null) 
                _prefab = Resources.Load<Tile>(PREFAB_PATH);

            Map map = new Map();

            using(StringReader reader = new StringReader(pattern)) {
                string line;
                int y = 0;
                while((line = reader.ReadLine()) != null) {
                    for(int x = 0; x < line.Length; x++) {
                        char c = line[x];
                        //TODO: mark start and end
                        if(c == 'S') {
                            map._startX = x;
                            map._startY = y;
                            continue;
                        }
                        if(c == 'E') {
                            map._endX = x;
                            map._endY = y;
                            continue;
                        }
                        if(c != '#')
                            continue;
                        Tile tile = Tile.Instantiate(_prefab);
                        tile.X = x;
                        tile.Y = y;
                        map._tiles.Add(tile);
                    }
                    y++;
                }
            }
            return map;
        }

        private int _startX;
        private int _startY;

        private int _endX;
        private int _endY;

        private List<Tile> _tiles = new List<Tile>();
        //TODO: add accessors for tiles?
        public Tile GetTile(int x, int y) {
            return _tiles.FirstOrDefault(t => t.X == x && t.Y == y);
        }
    }
}
