﻿using FCSCommon.Converters;
using FCSCommon.Enums;
using FCSCommon.Utilities;
using FCSTechWorkBench.Models;
using SMLHelper.V2.Utility;
using System;
using System.IO;
using UnityEngine;

namespace FCSTechWorkBench.Mono
{
    public class FilterArgs : EventArgs
    {
        public string CurrentTime { get; set; }
    }

    public abstract class Filter : MonoBehaviour
    {
        public abstract PrefabIdentifier PrefabId { get; set; }

        protected readonly string _saveDirectory = Path.Combine(SaveUtils.GetCurrentSaveDataDir(), Mod.ModName);
        protected string SaveFile => Path.Combine(_saveDirectory, PrefabId.Id + ".json");

        /// <summary>
        /// The remaining time in seconds
        /// </summary>
        public virtual string RemainingTime { get; protected set; } = TimeConverters.SecondsToHMS(0);

        public abstract FilterTypes FilterType { get; set; }
        protected virtual bool DoOnce { get; set; }

        /// <summary>
        /// The max time for the count down
        /// </summary>
        public virtual float MaxTime { get; set; }

        public bool IsExpired { get; set; }

        /// <summary>
        /// Event that is triggered when the timer ends
        /// </summary>
        public EventHandler TimerEnd;

        /// <summary>
        /// Event that is triggered when the timer start
        /// </summary>
        public EventHandler TimerStart;

        /// <summary>
        /// Event that is triggered when the timer ticks
        /// </summary>
        public EventHandler<FilterArgs> TimerTick;

        private bool _filterIsDead;

        protected virtual bool FromSave { get; set; }

        public virtual FilterState FilterState { get; set; }

        public virtual void UpdateFilterState()
        {
            FilterState = _filterIsDead ? FilterState.Dirty : FilterState.Filtering;
        }

        internal virtual bool RunTimer { get; set; }

        public abstract float GetMaxTime();

        /// <summary>
        /// Starts the timer
        /// </summary>

        public abstract void StartTimer();

        public abstract void StopTimer();

        public abstract void Initialize(bool fromSave = false);

        protected virtual void OnTimerStart()
        {
            TimerStart?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnTimerEnd()
        {
            TimerEnd?.Invoke(this, EventArgs.Empty);
            IsExpired = true;
        }

        protected virtual void OnTimerTick(string args)
        {
            QuickLogger.Debug("Timer Tick");

            TimerTick?.Invoke(this, new FilterArgs { CurrentTime = args });
        }

        public abstract string GetRemainingTime();

        public virtual void UpdateTimer()
        {
            if (!RunTimer) return;

            UpdateFilterState();

            if (MaxTime > 0f)
            {
                MaxTime -= DayNightCycle.main.deltaTime;
                RemainingTime = TimeConverters.SecondsToHMS(MaxTime);
                _filterIsDead = false;
            }
            else if (MaxTime <= 0f && !DoOnce)
            {
                DoOnce = true;
                RemainingTime = TimeConverters.SecondsToHMS(0);
                MaxTime = 0f;
                _filterIsDead = true;
            }
        }

        protected virtual void SetMaxTime()
        {

            if (FromSave) return;

            switch (FilterType)
            {
                case FilterTypes.LongTermFilter:
                    MaxTime = 36000f;
                    break;
                case FilterTypes.ShortTermFilter:
                    MaxTime = 2400f;
                    break;
                case FilterTypes.None:
                    MaxTime = 0f;
                    break;
                default:
                    MaxTime = 0f;
                    break;
            }
        }

        public virtual void SetRemainingTime()
        {
            RemainingTime = TimeConverters.SecondsToHMS(MaxTime);
        }
    }
}
