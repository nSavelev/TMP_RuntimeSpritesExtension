using UnityEngine;
using UnityEngine.U2D;

namespace TMP_SpriteExtension.Runtime
{
    public class SpritesRegistrator : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _sprites;
        [SerializeField]
        private SpriteAtlas[] _atlases;
        [SerializeField]
        private float _vOffset = 0.5f;

        private TMPRuntimeSpritesService _service;

        [ContextMenu("Register")]
        private void Awake()
        {
            _service = new TMPRuntimeSpritesService();
            _service.Init();
            if (_sprites != null && _sprites.Length > 0)
            {
                _service.RegisterSprites(_sprites, _vOffset);
            }

            if (_atlases != null && _atlases.Length > 0)
            {
                _service.RegisterAtlases(_atlases, _vOffset);
            }
        }

        [ContextMenu("UnRegister")]
        private void OnDestroy()
        {
            _service.Dispose();
        }
    }
}