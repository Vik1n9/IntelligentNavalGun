﻿using Newtonsoft.Json.Linq;
using Sakuno.KanColle.Amatsukaze.Game.Models.Raw;

namespace Sakuno.KanColle.Amatsukaze.Game.Parsers.Root.GetMember
{
    [Api("api_get_member/require_info")]
    class RequiredInfoParser : ApiParser<RawRequiredInfo>
    {
        public override void ProcessCore(ApiInfo rpInfo, RawRequiredInfo rpData)
        {
            Game.MasterInfo.WaitForInitialization();

            Game.Port.UpdateItemCount((JArray)rpInfo.Json["api_data"]["api_useitem"]);

            Game.Port.UpdateEquipment(rpData.Equipment);
            Game.Port.UpdateConstructionDocks(rpData.ConstructionDocks);
        }
    }
}
