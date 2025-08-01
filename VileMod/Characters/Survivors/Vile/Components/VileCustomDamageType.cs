using UnityEngine;
using R2API;
using RoR2;
using static R2API.DamageAPI;

namespace VileMod.Survivors.Vile.Components
{
    internal class VileCustomDamageType : MonoBehaviour
    {
        public static ModdedDamageType NightmareDamage;
        public static ModdedDamageType PlasmaSphereDamage;

        public static void RegisterDamageTypes()
        {
            // Create new custom DamageTypes
            NightmareDamage = DamageAPI.ReserveDamageType();
            PlasmaSphereDamage = DamageAPI.ReserveDamageType();
        }
    }
}