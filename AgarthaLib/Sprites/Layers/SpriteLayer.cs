using System;
using UnityEngine;

namespace AgarthaLib.Sprites.Layers
{
    [Serializable] public class SpriteLayer
    {
        public string Name = "base";

        public Sprite Sprite = null;

        [Tooltip("Null materials get ignored.")]
        public Material Material = null;

        [Tooltip("Change it to anything but 0 for it to not use standard list ordering.")] 
        public int SortingLayer = 0;
    }
}