using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.EventSystems;

namespace ClickToEquip
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ClickToEquipPlugin : BaseUnityPlugin

    {
        internal const string ModName = "ClickToEquip";
        internal const string ModVersion = "1.0.0";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;
        private readonly Harmony _harmony = new(ModGUID);
        public static readonly ManualLogSource ClickToEquipLogger = BepInEx.Logging.Logger.CreateLogSource(ModName);

        private void Awake()
        {
            _harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(UIInventory), nameof(UIInventory.RightClickOnBackpackSlot))]
    static class UIInventoryRightClickOnBackpackSlotPatch
    {
        static void Prefix(UIInventory __instance, PointerEventData eventData, Item itemInSlot)
        {
            if (!itemInSlot)
                return;
            foreach (UIEquipmentSlot instanceEquipmentSlot in __instance._equipmentSlots.Where(x => x.equipmentSlotType == itemInSlot.equipmentSlotType))
            {
                instanceEquipmentSlot.TryDropItem(itemInSlot);
                __instance.Refresh();
            }
        }
    }
}