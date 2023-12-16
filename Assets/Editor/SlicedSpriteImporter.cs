using UnityEngine;
using UnityEditor;
public class SlicedSpriteImporter : AssetPostprocessor
{
    public string prefix = "r";
    public bool disabled = true;

    void OnPreprocessTexture()
    {
        if (disabled)
        {
            return;
        }

        string[] path_split = assetPath.Split('/');

        if (assetPath.EndsWith(".png") && path_split[^1].StartsWith(prefix))
        {
            Debug.Log("imported!");

            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.spritePivot = new Vector2(0.5f, 0f);
            textureImporter.mipmapEnabled = false;
        }
    }
}
