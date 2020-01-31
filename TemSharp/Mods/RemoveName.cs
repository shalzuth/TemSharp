using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TemSharp
{
    public class RemoveName : MonoBehaviour
    {
        void OnEnable()
        {
            typeof(Temtem.UI.InGameMenuUI).GetField<Temtem.UI.InGameMenuUI>().GetField<TMPro.TextMeshProUGUI>("versionLabel").text = "";
            typeof(Temtem.UI.MonsterBattleStatsUI).GetField<Temtem.UI.MonsterBattleStatsUI>().GetField<TMPro.TextMeshProUGUI>("versionLabel").text = "";
        }
    }
}
