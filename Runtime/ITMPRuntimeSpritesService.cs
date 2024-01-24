using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace TMP_SpriteExtension.Runtime
{
    public interface ITMPRuntimeSpritesService
    {
        void Init();
        void RegisterSprites(IEnumerable<Sprite> sprites, float vOffset);
        void RegisterAtlas(SpriteAtlas atlas, float vOffset);
        void RegisterAtlases(IEnumerable<SpriteAtlas> atlases, float vOffset);
    }
}