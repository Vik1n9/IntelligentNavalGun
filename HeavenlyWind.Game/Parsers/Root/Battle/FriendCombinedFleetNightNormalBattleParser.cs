﻿using Sakuno.KanColle.Amatsukaze.Game.Models.Raw.Battle;

namespace Sakuno.KanColle.Amatsukaze.Game.Parsers.Root.Battle
{
    [Api("api_req_combined_battle/midnight_battle")]
    class FriendCombinedFleetNightNormalBattleParser : ApiParser<RawCombinedFleetNight>
    {
        public override void ProcessCore(ApiInfo rpInfo, RawCombinedFleetNight rpData)
        {
        }
    }
}
