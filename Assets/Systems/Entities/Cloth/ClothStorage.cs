using System;
using System.Collections.Generic;
using System.Text;
using Tools.SpriteDynamicRenderer.Data;
using UnityEngine;

namespace Systems.Entities.Cloth
{
    [CreateAssetMenu(fileName = "ClothStorage", menuName = "Cloth/Storage", order = 0)]
    public class ClothStorage : ScriptableObject
    {
        [Serializable]
        public class ClothCategory
        {
            public string categoryName;
            public SpriteDynamicRendererData renderData;
        }

        public List<ClothCategory> cloths;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ \"category\": \"").Append(name).Append("\", \"cloths\": [");
            for (int i = 0; i < cloths.Count; i++)
            {
                sb.Append("\"").Append(cloths[i].categoryName).Append("\"");
                if (i < cloths.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("] }");
            return sb.ToString();
        }
    }
}