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

        private TMPRuntimeSpritesService _service;

        [ContextMenu("Register")]
        private void Awake()
        {
            _service = new TMPRuntimeSpritesService();
            _service.Init();
            if (_sprites != null && _sprites.Length > 0)
            {
                _service.RegisterSprites(_sprites);
            }

            if (_atlases != null && _atlases.Length > 0)
            {
                _service.RegisterAtlases(_atlases);
            }
        }

        [ContextMenu("UnRegister")]
        private void OnDestroy()
        {
            _service.Dispose();
        }
    }
}