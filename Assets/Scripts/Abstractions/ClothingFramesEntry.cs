using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abstractions
{
    [Serializable]
    public struct ClothingFramesEntry
    {
        public Directions direction;
        public List<Sprite> frames;
    }
}