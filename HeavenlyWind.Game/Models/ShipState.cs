﻿using System;

namespace Sakuno.KanColle.Amatsukaze.Game.Models
{
    [Flags]
    public enum ShipState
    {
        None,
        Repairing = 1,
        Evacuated = 1 << 1,
        HeavilyDamaged = 1 << 2,
        RepairingInAnchorage = 1 << 3,
        Expedition = 1 << 4,
    }
}
