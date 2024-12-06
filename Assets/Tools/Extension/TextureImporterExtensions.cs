using Tools.Extension.Exceptions;
using UnityEditor;
using UnityEditor.U2D.Sprites;

namespace Tools.Extension
{
    public static class TextureImporterExtensions
    {
        public static ISpriteEditorDataProvider GetSpriteEditorDataProvider(this TextureImporter textureImporter)
        {
            SpriteDataProviderFactories dataProviderFactory = new SpriteDataProviderFactories();
            ISpriteEditorDataProvider dataProvider = dataProviderFactory.GetSpriteEditorDataProviderFromObject(textureImporter);
            if (dataProvider == null)
            {
                throw new GetSpriteEditorDataProviderException();
            }
            return dataProvider;
        }
    }
}