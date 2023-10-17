using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.U2D;

namespace TMP_SpriteExtension.Runtime
{
    public class TMPRuntimeSpritesService : ITMPRuntimeSpritesService, IDisposable
    {
        private TMP_SpriteAsset _rootAsset = new TMP_SpriteAsset();
        private uint _indexer = 0;

        private void RegisterSpritesHolder()
        {
            if (_rootAsset != null)
            {
                if (TMP_Settings.defaultSpriteAsset == null)
                {
                    typeof(TMP_Settings)
                        .GetField("m_defaultSpriteAsset", BindingFlags.Instance | BindingFlags.NonPublic)
                        .SetValue(TMP_Settings.instance, _rootAsset);
                }
                else
                {
                    TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.Add(_rootAsset);
                }
            }
        }
        
        public void Dispose()
        {
            if (TMP_Settings.defaultSpriteAsset == _rootAsset)
            {
                UnityEngine.Object.Destroy(TMP_Settings.defaultSpriteAsset);
            }
            else
            {
                TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.Remove(_rootAsset);
            }
            UnityEngine.Object.Destroy(_rootAsset);
        }

        public void Init()
        {
            _rootAsset.material = GetOrCreateSpriteMaterial(null);
            _rootAsset.fallbackSpriteAssets = new List<TMP_SpriteAsset>();
            _rootAsset.spriteInfoList = new List<TMP_Sprite>();
            RegisterSpritesHolder();
        }

        public void RegisterSprites(IEnumerable<Sprite> sprites)
        {
            var groups = sprites.GroupBy(e => e.texture).Where(e=>e.Key != null).Select(e=>(e.Key,e.ToList())).ToList();
            foreach (var group in groups)
            {
                _rootAsset.fallbackSpriteAssets.Add(CreateSpriteAsset(group.Item2.Where(e=>e != null)));
            }
        }

        private TMP_SpriteAsset CreateSpriteAsset(IEnumerable<Sprite> sprites)
        {
            var asset = TMP_SpriteAsset.CreateInstance<TMP_SpriteAsset>();
            asset.spriteSheet = sprites.First().texture;
            asset.material = GetOrCreateSpriteMaterial(asset.spriteSheet);
            asset.spriteInfoList = new List<TMP_Sprite>();
            asset.spriteGlyphTable.Clear();
            asset.UpdateLookupTables();
            foreach (var sprite in sprites)
            {
                var spriteRect = sprite.textureRect;
                var spriteGlyph = new TMP_SpriteGlyph();
                spriteGlyph.index = _indexer;
                spriteGlyph.scale = 1f;
                spriteGlyph.metrics = new GlyphMetrics((int)spriteRect.width, (int)spriteRect.height,
                    0, spriteRect.height * 0.8f, 
                    spriteRect.width);
                spriteGlyph.glyphRect = new GlyphRect(spriteRect);
                asset.spriteGlyphTable.Add(spriteGlyph);

                var spriteCharacter = new TMP_SpriteCharacter(0, spriteGlyph);
                spriteCharacter.name = sprite.name.Replace("(Clone)", "").Trim();
                spriteCharacter.unicode = 0xFFFE;
                spriteCharacter.scale = 1.5f;
                asset.spriteCharacterTable.Add(spriteCharacter);
                _indexer++;
            }
            asset.UpdateLookupTables();
            return asset;
        }
        
        public void RegisterAtlas(SpriteAtlas atlas)
        {
            RegisterSprites(GetSpritesFromAtlas(atlas));
        }

        public void RegisterAtlases(IEnumerable<SpriteAtlas> atlases)
        {
            RegisterSprites(atlases.Where(e=>e).SelectMany(e => GetSpritesFromAtlas(e)));
        }

        private IEnumerable<Sprite> GetSpritesFromAtlas(SpriteAtlas atlas)
        {
            var sprites = new Sprite[atlas.spriteCount];
            atlas.GetSprites(sprites);
            return sprites;
        }

        private Material GetOrCreateSpriteMaterial(Texture texture)
        {
            var material = new Material(Shader.Find("TextMeshPro/Sprite"));
            material.mainTexture = texture;
            return material;
        }
    }
}