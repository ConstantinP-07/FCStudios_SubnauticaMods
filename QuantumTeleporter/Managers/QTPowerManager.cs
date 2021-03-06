﻿using System;
using FCSCommon.Abstract;
using FCSCommon.Utilities;
using QuantumTeleporter.Enumerators;
using UnityEngine;

namespace QuantumTeleporter.Managers
{
    internal class QTPowerManager
    {
        private readonly float _interPowerUsage = QPatch.Configuration.GlobalTeleportPowerUsage;
        private readonly float _intraPowerUsage = QPatch.Configuration.InternalTeleportPowerUsage;
        private PowerRelay _connectedRelay;
        private readonly FCSController _mono;

        private PowerRelay ConnectedRelay
        {
            get
            {
                while (_connectedRelay == null)
                    UpdatePowerRelay();

                return _connectedRelay;
            }
        }

        public QTPowerManager(FCSController mono)
        {
            _mono = mono;
            UpdatePowerRelay();
        }

        private void UpdatePowerRelay()
        {
            PowerRelay relay = PowerSource.FindRelay(_mono.transform);
            if (relay != null && relay != _connectedRelay)
            {
                _connectedRelay = relay;
                QuickLogger.Debug("PowerRelay found at last!");
            }
            else
            {
                _connectedRelay = null;
            }
        }
        
        internal bool TakePower(QTTeleportTypes type)
        {
            QuickLogger.Debug($"Available power {ConnectedRelay?.GetPower()}",true);

            if (HasEnoughPower(type))
            {
                float amountConsumed;
                switch (type)
                {
                    case QTTeleportTypes.Global:
                        ConnectedRelay.ConsumeEnergy(_interPowerUsage, out amountConsumed);
                        break;
                    case QTTeleportTypes.Intra:
                        ConnectedRelay.ConsumeEnergy(_intraPowerUsage, out amountConsumed);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }

                QuickLogger.Debug($"Consumed {amountConsumed} amount of power for this operation",true);
                return true;
            }

            return false;
        }

        internal bool HasEnoughPower(QTTeleportTypes type)
        {
            bool requiresEnergy = GameModeUtils.RequiresPower();

            if (!requiresEnergy) return true;

            switch (type)
            {
                case QTTeleportTypes.Global:
                     return ConnectedRelay != null && ConnectedRelay.GetPower() >= _interPowerUsage;

                case QTTeleportTypes.Intra:
                    return ConnectedRelay != null && ConnectedRelay.GetPower() >= _intraPowerUsage;
                    
            }
            return false;
        }

        public float PowerAvailable()
        {
            return Mathf.RoundToInt(_connectedRelay.GetPower());
        }
    }
}
