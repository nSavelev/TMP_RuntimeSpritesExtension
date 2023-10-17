using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace TMP_SpriteExtension.Runtime
{
    public interface ITMPRuntimeSpritesService
    {
        void Init();
        void RegisterSprites(IEnumerable<Sprite> sprites);
        void RegisterAtlas(SpriteAtlas atlas);
        void RegisterAtlases(IEnumerable<SpriteAtlas> atlases);
    }
}