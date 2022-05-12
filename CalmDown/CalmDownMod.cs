using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.UI.Core;
using static MelonLoader.MelonLogger;

namespace CalmDown
{
    public class CalmDownMod : MelonMod
    {
        private static MelonPreferences_Category CalmDownCat = MelonPreferences.CreateCategory("CalmDown");
        private static MelonPreferences_Entry<bool> Enabled = CalmDownCat.CreateEntry("Enable PanicButton suppression", true);
        private static VRCInput OriginalInput;
        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(WaitForUiManagerInit());
        }

        private IEnumerator WaitForUiManagerInit()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null) yield return null;
            while (UIManager.field_Private_Static_UIManager_0 == null) yield return null;

            OriginalInput = VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0["PanicButton"];

            if(Enabled.Value)
            {
                VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0.Remove("PanicButton");
                LoggerInstance.Msg("Panic Button Disabled");
            }

            Enabled.OnValueChanged += (prev, val) =>
            {
                if (val)
                {
                    VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0.Remove("PanicButton");
                    LoggerInstance.Msg("Panic Button Disabled");
                }
                else
                {
                    VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0.Add("PanicButton", OriginalInput);
                    LoggerInstance.Msg("Panic Button Enabled");
                }
            };
        }
    }
}
