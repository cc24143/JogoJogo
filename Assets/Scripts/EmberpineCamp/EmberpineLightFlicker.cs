using UnityEngine;

namespace EmberpineCamp
{
    /// <summary>
    /// Optional cosmetic helper: gently varies a Light's intensity and range like a living flame.
    /// Used by the Campfire and Camp Lantern prefabs. Remove it if you drive lights yourself.
    /// </summary>
    [RequireComponent(typeof(Light))]
    [DisallowMultipleComponent]
    public class EmberpineLightFlicker : MonoBehaviour
    {
        [Tooltip("Relative intensity change (0.15 = +/-15%).")]
        [Range(0f, 0.5f)] public float amount = 0.15f;
        [Tooltip("Flicker speed.")]
        [Range(0.1f, 10f)] public float speed = 3f;

        Light _light;
        float _baseIntensity, _baseRange, _seed;

        void Awake()
        {
            _light = GetComponent<Light>();
            _baseIntensity = _light.intensity;
            _baseRange = _light.range;
            _seed = Random.value * 100f;
        }

        void Update()
        {
            float n = Mathf.PerlinNoise(_seed, Time.time * speed) * 2f - 1f;
            _light.intensity = _baseIntensity * (1f + n * amount);
            _light.range = _baseRange * (1f + n * amount * 0.3f);
        }
    }
}
