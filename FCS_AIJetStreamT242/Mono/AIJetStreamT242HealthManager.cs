﻿using FCS_AIMarineTurbine.Buildable;
using FCSCommon.Helpers;
using FCSCommon.Objects;
using FCSCommon.Utilities;
using System;
using UnityEngine;

namespace FCS_AIMarineTurbine.Mono
{
    internal class AIJetStreamT242HealthManager : MonoBehaviour
    {
        public LiveMixin LiveMixin { get; set; } = new LiveMixin();
        public bool IsDamagedFlag { get; set; } = true;

        public int HealthMultiplyer { get; set; } = 1;
        private const float DayNight = 1200f;
        private int _damagePerDay = 10;
        private float _damagePerSecond;
        private float _passedTime;
        private GameObject _damage;
        private bool _shaderApplied;
        public Action OnDamaged { get; set; }
        public Action OnRepaired { get; set; }


        #region Unity Methods

        private void Awake()
        {

        }

        private void Update()
        {
            //TODO Check if will cause problem
            //if (!IsSafeToContinue) return;

            UpdateHealthSystem();
        }
        #endregion

        private void UpdateHealthSystem()
        {
            _passedTime += DayNightCycle.main.deltaTime;

            //QuickLogger.Debug($"Passed Time: {_passedTime} || Damage Per Sec {_damagePerSecond}");

            if (_passedTime >= _damagePerSecond)
            {
                QuickLogger.Debug("Damaging Unit");
                ApplyDamage();
            }
        }

        public bool IsDamageApplied()
        {
            if (LiveMixin == null) return true;

            return LiveMixin.health <= 0;
        }

        internal float GetHealth()
        {
            return LiveMixin.health;
        }

        internal void SetHealth(float health)
        {
            LiveMixin.health = health;
        }

        internal void Initialize(AIJetStreamT242Controller mono)
        {
            LiveMixin = GetComponentInParent<LiveMixin>();

            _damagePerSecond = DayNight / _damagePerDay;

            if (LiveMixin != null)
            {
                if (LiveMixin.data == null)
                {
                    QuickLogger.Debug($"Creating Data");
                    LiveMixin.data = CustomLiveMixinData.Get();
                    QuickLogger.Debug($"Created Data");
                }
                else
                {
                    LiveMixin.data.weldable = true;
                }

                InvokeRepeating("HealthChecks", 0, 1);
            }
            else
            {
                QuickLogger.Error($"LiveMixing not found!");
            }
        }

        public void ApplyDamage()
        {
            if (LiveMixin.health > 0 && AIJetStreamT242Buildable.JetStreamT242Config.EnableWear)
            {
                LiveMixin.health = Mathf.Clamp(LiveMixin.health - HealthMultiplyer, 0f, 100f);
                ResetTimer();
            }
        }

        private void HealthChecks() // In and InvokeRepeating
        {
            try
            {
                if (GetHealth() >= 1f && !IsDamagedFlag)
                {
                    QuickLogger.Debug("Turbine Repaired", true);
                    OnRepaired?.Invoke();
                    UpdateDamageState();
                    IsDamagedFlag = true;
                }

                if (GetHealth() <= 0f && IsDamagedFlag)
                {
                    QuickLogger.Debug("Turbine Damaged", true);
                    OnDamaged?.Invoke();
                    UpdateDamageState();
                    IsDamagedFlag = false;
                }
            }
            catch (Exception e)
            {
                QuickLogger.Error(e.Message);
            }
        }

        private void UpdateDamageState()
        {
            if (IsDamagedFlag)
            {
                _damage.SetActive(true);
                MaterialHelpers.ApplyEmissionShader("SystemLights_BaseColor", "SystemLights_OffMode_Emissive", gameObject, QPatch.Bundle, new Color(0.08235294f, 1f, 1f));
                if (!_shaderApplied)
                {
                    ApplyDamageShader();
                }
            }
            else
            {
                _damage.SetActive(false);
                MaterialHelpers.ApplyEmissionShader("SystemLights_BaseColor", "SystemLights_OnMode_Emissive", gameObject, QPatch.Bundle, new Color(01f, 0.09803922f, 0.09803922f));

            }
        }

        private void ApplyDamageShader()
        {
            #region FCS_SUBMods_GlobalDecals
            MaterialHelpers.ApplyAlphaShader("FCS_SUBMods_GlobalDecals", _damage);
            MaterialHelpers.ApplyEmissionShader("FCS_SUBMods_GlobalDecals", "FCS_SUBMods_GlobalDecals_Emissive", _damage, QPatch.Bundle, Color.white);
            MaterialHelpers.ApplyNormalShader("FCS_SUBMods_GlobalDecals", "FCS_SUBMods_GlobalDecals_Norm", _damage, QPatch.Bundle);
            #endregion

            #region FCS_MarineTurbine_Tex
            MaterialHelpers.ApplyMetallicShader("FCS_MarineTurbine_Tex", "JetStreamT242_MarineTurbineMat_MetallicSmoothness", _damage, QPatch.Bundle, 0.2f);
            #endregion
            _shaderApplied = true;
        }

        private void ResetTimer()
        {
            _passedTime = 0;
        }

        internal float GetPassedTime()
        {
            return _passedTime;
        }

        public void SetPassedTime(float savedDataPassedTime)
        {
            _passedTime = savedDataPassedTime;
        }

        public void SetDamageModel(GameObject damage)
        {
            _damage = damage;
        }
    }
}
