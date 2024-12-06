using System.Collections.Generic;
using UnityEngine;

namespace Tools.SpriteSheetCutter.Editor
{
    public class ListWrapper<T> : ScriptableObject {
        public List<Texture2D> elements = new ();
    }
}