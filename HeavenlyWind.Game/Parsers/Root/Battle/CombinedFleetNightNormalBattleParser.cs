﻿using Sakuno.KanColle.Amatsukaze.Game.Models.Raw.Battle;

namespace Sakuno.KanColle.Amatsukaze.Game.Parsers.Root.Battle
{
    [Api("api_req_combined_battle/midnight_battle")]
    class CombinedFleetNightNormalBattleParser : ApiParser<RawCombinedFleetNight>
    {
        public override void Process(RawCombinedFleetNight rpData)
        {
        }
    }
}
