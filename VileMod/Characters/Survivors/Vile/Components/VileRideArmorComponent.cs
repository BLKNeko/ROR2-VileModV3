using UnityEngine;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Wamp;
using System;
using UnityEngine.TextCore.Text;

namespace VileMod.Survivors.Vile.Components
{
    internal class VileRideArmorComponent : MonoBehaviour
    {
        private Transform modelTransform;

        private HealthComponent HealthComp;

        private CharacterBody Body;


        private CharacterModel model;
        private ChildLocator childLocator;

        private EntityStateMachine entityState;


        private float r_MaxHealth;
        private float r_Health;

        private float r_RegemValue = 1f;
        private float r_RegemCooldown = 0.5f; // 0.5 seconds cooldown for regen
        private float r_RegenTimer;

        private bool r_InitializeSuperRegem = false;
        private float r_InitializeSuperRegemTimer = 0f;


        private void Start()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
            if (Body == null)
            {
                Body = GetComponent<CharacterBody>();
            }

            HealthComp = Body.GetComponent<HealthComponent>();

            modelTransform = Body.transform;

            model = Body.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();

            childLocator = Body.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().GetComponent<ChildLocator>();

            entityState = EntityStateMachine.FindByCustomName(Body.gameObject, "Body");

            Hook();

        }

        private void FixedUpdate()
        {
            if (!Body.hasAuthority) return;

            if (Body.HasBuff(VileBuffs.GoliathBuff))
            {
                UpdateMaxHealth();
                CheckHealthUpdate();
            }

            if (r_InitializeSuperRegem)
                InitializeSuperRegem();


            if (Body.HasBuff(VileBuffs.RideArmorEnabledBuff) && !Body.HasBuff(VileBuffs.GoliathBuff))
                RegenRideArmorWhileNotInUse();

        }

        private void UpdateMaxHealth()
        {
            r_MaxHealth = HealthComp.fullCombinedHealth;
        }

        private void CheckHealthUpdate()
        {
            if(r_Health <= 0f)
            {
                
                entityState.SetNextState(new SkillStates.DestroyGoliath());

            }
        }

        private void RegenRideArmorWhileNotInUse()
        {
            r_RegenTimer += Time.fixedDeltaTime;

            if(r_RegenTimer >= r_RegemCooldown)
            {
                r_Health += r_RegemValue;

                r_Health = Mathf.Clamp(r_Health, 1f, r_MaxHealth);

                r_RegenTimer = 0f;

                Debug.Log($"VileRideArmorComponent - Ride Armor Regenerated: {r_RegemValue}, New Health: {r_Health}");
            }
        }

        public void InitializeRideArmor()
        {
            if (!Body.hasAuthority) return;

            UpdateMaxHealth();

            r_Health = r_MaxHealth;

            if (NetworkServer.active)
            {
                Body.AddBuff(VileBuffs.RideArmorEnabledBuff);
            }

            r_InitializeSuperRegem = true;
            r_InitializeSuperRegemTimer = 0f;

            Debug.Log($"VileRideArmorComponent - Initialized Ride Armor with Max Health: {r_MaxHealth}");

        }

        private void InitializeSuperRegem()
        {
            if (r_Health < r_MaxHealth || r_InitializeSuperRegemTimer <= 2f)
            {
                r_Health += Time.fixedDeltaTime * (r_MaxHealth / (2f - Time.fixedDeltaTime));
                r_Health = Mathf.Clamp(r_Health, 1f, r_MaxHealth);

                r_InitializeSuperRegemTimer += Time.fixedDeltaTime;
                
            }
            else
            {
                r_InitializeSuperRegem = false;
            }
        }

        public void RepairRideArmor(float amount)
        {
            r_Health += amount;

            r_Health = Mathf.Clamp(r_Health, 1f, r_MaxHealth);
        }

        public float GetRHealthValue()
        {
            return r_Health;
        }

        public float GetRMaxHealthValue()
        {
            return r_MaxHealth;
        }

        public float GetInverseLerpRHealthValue()
        {
            // Inverse lerp the bolt value to a range of 0 to 1
            return Mathf.InverseLerp(0, r_MaxHealth, r_Health);
        }

        private void Hook()
        {
            //On.RoR2.CameraRigController.Update += CameraRigController_Update;
            //On.RoR2.UI.HUD.Update += HUD_Update;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {


            //Debug.Log(self.name);
            //Debug.Log(damageInfo.damage);
            //Debug.Log(damageInfo.inflictor);
            if (self == null || damageInfo == null)
            {
                orig(self, damageInfo);
                return;
            }


            if (damageInfo.inflictor == null || damageInfo.attacker == null)
            {
                orig(self, damageInfo);
                return;
            }

            if (self.GetComponent<CharacterBody>() == null)
            {
                orig(self, damageInfo);
                return;
            }

            if (damageInfo.attacker.name != Body.name && self == HealthComp)
            {

                if (self.GetComponent<CharacterBody>().HasBuff(VileBuffs.GoliathBuff))
                {
                    float finalDamage = damageInfo.damage;

                    if (self.GetComponent<CharacterBody>())
                    {
                        float armor = self.GetComponent<CharacterBody>().armor;
                        float armorMultiplier = 100f / (100f + armor);
                        finalDamage *= armorMultiplier;
                    }

                    r_Health -= finalDamage;

                    
                    Debug.Log($"VileRideArmorComponent - Ride Armor Health Decreased: {damageInfo.damage}, New Health: {r_Health}");

                    if (modelTransform)
                    {
                        TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                        temporaryOverlayInstance.duration = 1f;
                        temporaryOverlayInstance.animateShaderAlpha = true;
                        temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                        temporaryOverlayInstance.destroyComponentOnEnd = true;
                        temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matFullCrit");
                        temporaryOverlayInstance.inspectorCharacterModel = model;
                        temporaryOverlayInstance.AddToCharacterModel(model);
                    }

                    damageInfo.damage = 0f;
                    damageInfo.rejected = true;
                }

            }

            orig(self, damageInfo);
        }

        public void Unhook()
        {
            //On.RoR2.CameraRigController.Update -= CameraRigController_Update;
            //On.RoR2.UI.HUD.Update -= HUD_Update;
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        public void OnDestroy()
        {

            Unhook();
        }

    }
}