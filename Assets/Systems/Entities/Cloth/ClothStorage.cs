using System;
using System.Collections.Generic;
using System.Linq;
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
            public List<SpriteDynamicRendererData> renderData;
        }

        public List<ClothCategory> cloths;

        public SpriteDynamicRendererData GetCloth(string categoryName, string clothName)
        {
            return cloths.Where(clothCategory => clothCategory.categoryName == categoryName)
                .SelectMany(clothCategory => clothCategory.renderData)
                .FirstOrDefault(spriteDynamicRendererData => spriteDynamicRendererData.AnimationSectionName == clothName);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ \"category\": \"").Append(name).Append("\", \"cloths\": [");
            foreach (ClothCategory clothCategory in cloths)
            {
                sb.Append("{ \"categoryName\": \"").Append(clothCategory.categoryName).Append("\", \"renderData\": [");

                foreach (SpriteDynamicRendererData spriteDynamicRendererData in clothCategory.renderData)
                {
                    sb.Append($"\"{spriteDynamicRendererData}\"").Append(", ");
                }

                sb.Append("]}, ");
            }
            sb.Append("]}");

            return sb.ToString();
        }
    }
}