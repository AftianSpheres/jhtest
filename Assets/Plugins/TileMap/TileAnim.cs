using UnityEngine;
using System;

namespace UnityTileMap
{
    [ExecuteInEditMode]
    [Serializable]
    public class TileAnim
    {
        public int[] frameIds;
        public int frameInterval;
        public int Id;
        public bool initialized = false;
        private int frameCtr;
        public int place;
        public Sprite[] sprite_cycle;
        private TileEntry entry;
        private TileSheet sheet;

        public void Start (TileSheet _sheet)
        {
            sheet = _sheet;
            entry = sheet.GetEntry(Id);
            if (frameIds.Length > 1)
            {
                sprite_cycle = new Sprite[frameIds.Length];
                for (int i = 0; i < frameIds.Length; i++)
                {
                    sprite_cycle[i] = sheet.Get(frameIds[i]);
                }
                entry.originalSprite = entry.Sprite;
                entry.Sprite = sprite_cycle[0];
                frameCtr = 0;
                place = 0;
            }
            initialized = true;
        }

        public bool Update ()
        {
            bool r = false;
            if (sprite_cycle != null && entry != null)
            {
                frameCtr++;
                r = frameCtr > frameInterval;
                if (r == true)
                {
                    frameCtr = 0;
                    place++;
                    if (place >= sprite_cycle.Length)
                    {
                        place = 0;
                    }
                    entry.Sprite = sprite_cycle[place];
                }
            }
            return r;
        }

    }
}


