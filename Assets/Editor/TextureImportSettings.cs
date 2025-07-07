using UnityEngine;
using UnityEditor;

public class TextureImportSettings : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;

        // Varsay�lan ayarlar� burada belirleyin
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
    }
}
