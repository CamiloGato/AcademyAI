using System;
using Tools.Extension;
using UnityEngine;

namespace Tools.Objects.Mappers
{
    [Serializable]
    public class Texture2DCutDataDto
    {
        public int gridWidth;
        public int gridHeight;
        public int offsetX;
        public int offsetY;
        public int paddingX;
        public int paddingY;
    }

    public static class Texture2DCutDataMapper
    {
        public static Texture2DCutDataDto ToDto(Texture2DCutData texture2DCutData)
        {
            return new Texture2DCutDataDto
            {
                gridWidth = texture2DCutData.gridWidth,
                gridHeight = texture2DCutData.gridHeight,
                offsetX = texture2DCutData.offsetX,
                offsetY = texture2DCutData.offsetY,
                paddingX = texture2DCutData.paddingX,
                paddingY = texture2DCutData.paddingY
            };
        }

        public static Texture2DCutData ToEntity(Texture2DCutDataDto texture2DCutDataDto)
        {
            var texture2DCutData = ScriptableObject.CreateInstance<Texture2DCutData>();
            texture2DCutData.gridWidth = texture2DCutDataDto.gridWidth;
            texture2DCutData.gridHeight = texture2DCutDataDto.gridHeight;
            texture2DCutData.offsetX = texture2DCutDataDto.offsetX;
            texture2DCutData.offsetY = texture2DCutDataDto.offsetY;
            texture2DCutData.paddingX = texture2DCutDataDto.paddingX;
            texture2DCutData.paddingY = texture2DCutDataDto.paddingY;

            return texture2DCutData;
        }
    }
}