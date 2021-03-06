﻿using FCSCommon.Enums;
using UnityEngine;

namespace FCSPowerStorage.Managers
{
    internal class SystemLightManager : MonoBehaviour
    {
        private Color Blue = new Color(0, 0.921875f, 0.9453125f);
        private Color Red = new Color(1, 0, 0);
        private Color Orange = new Color(0.99609375f, 0.62890625f, 0.01953125f);
        private SystemLightState _currentState;
        private SystemLightState _storedState;
        private GameObject _gameObject;
        private bool _initialized;

        internal void Initialize(GameObject gameObject)
        {
            _gameObject = gameObject;
            _initialized = true;
        }

        internal void ChangeSystemLights(SystemLightState state)
        {
            if (state == _currentState || !_initialized) return;
            _currentState = state;

            switch (state)
            {
                case SystemLightState.Default:
                    ChangeMaterialColor("SystemLights", _gameObject, Blue);
                    break;
                case SystemLightState.Warning:
                    ChangeMaterialColor("SystemLights", _gameObject, Orange);
                    break;
                case SystemLightState.Unpowered:
                    ChangeMaterialColor("SystemLights", _gameObject, Red);
                    break;
                case SystemLightState.None:
                    ChangeMaterialColor("SystemLights", _gameObject, Blue);
                    break;
            }
        }

        private void ChangeMaterialColor(string materialName, GameObject gameObject, Color color)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            Shader shader = Shader.Find("MarmosetUBER");
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    if (material.name.StartsWith(materialName))
                    {
                        material.shader = shader;
                        material.SetColor("_Color", color);
                        material.EnableKeyword("MARMO_EMISSION");
                        material.EnableKeyword("_EMISSION");
                        material.SetFloat("_EmissionLM", 0f);
                        material.SetVector("_EmissionColor", color);
                        material.SetColor("_Illum", color);
                        material.SetVector("_Illum_ST", new Vector4(1.0f, 1.0f, 0.0f, 0.0f));
                        material.SetFloat("_EnableGlow", 1);
                        material.SetColor("_GlowColor", color);
                        material.SetFloat("_GlowStrength", 1f);
                        material.SetColor("_Color", color);
                    }
                }
            }
        }

        internal SystemLightState GetCurrentState()
        {
            return _currentState;
        }

        public void StoreCurrentState()
        {
            _storedState = _currentState;
        }

        internal void RestoreStoredState()
        {
            if (!_initialized) return;

            ChangeSystemLights(_storedState);
        }
    }
}
