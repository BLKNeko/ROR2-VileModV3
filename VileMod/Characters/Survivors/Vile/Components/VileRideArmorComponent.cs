using UnityEngine;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Wamp;
using System;
using UnityEngine.TextCore.Text;
using System.Xml.Linq;

namespace VileMod.Survivors.Vile.Components
{
    internal class VileRideArmorComponent : NetworkBehaviour
    {
        private Transform modelTransform;

        private HealthComponent HealthComp;

        private CharacterBody Body;


        private CharacterModel model;
        private ChildLocator childLocator;

        private EntityStateMachine entityState;


        private float r_MaxHealth;
        private float r_Health;

        private float r_MaxShield;
        private float r_Shield;

        private float r_RegemValue = 1f;
        private float r_RegemCooldown = 0.5f; // 0.5 seconds cooldown for regen
        private float r_RegenTimer;

        private bool r_InitializeSuperRegem = false;
        private float r_InitializeSuperRegemTimer = 0f;

        private float h_FlyingSpeed; // Speed at which the ride armor flies
        private float h_FlyingSpeedMultplier; // Speed multiplier for flying
        private float h_FlyingDuration = 3f; // Duration of the flying state
        private float h_FlyingTimer = 0f; // Timer to track flying duration
        private bool h_FlyingActivated = false;
        private float h_FallSlowStrength = 1f;   // força para desacelerar a queda
        private float h_MaxFallSpeed = -20f;


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


            h_FlyingSpeedMultplier = VileConfig.hk_flyingSpeedMultiplier.Value;

            Hook();

        }

        private void FixedUpdate()
        {
            if (!Body.hasAuthority) return;

            if (HasRideArmorBuff())
            {
                UpdateMaxHealth();
                CheckHealthUpdate();
            }

            if (r_InitializeSuperRegem)
                InitializeSuperRegem();


            if (Body.HasBuff(VileBuffs.RideArmorEnabledBuff) && !HasRideArmorBuff())
                RegenRideArmorWhileNotInUse();

            if (Body.HasBuff(VileBuffs.HawkBuff))
                HawkFlyingBehavior();

            DeplateShieldOverTime();

        }

        private bool HasRideArmorBuff()
        {
            return Body.HasBuff(VileBuffs.GoliathBuff) || Body.HasBuff(VileBuffs.HawkBuff) || Body.HasBuff(VileBuffs.CyclopsBuff);
        }

        private void UpdateMaxHealth()
        {
            r_MaxHealth = HealthComp.fullCombinedHealth;

            r_MaxShield = r_MaxHealth;
        }

        private void CheckHealthUpdate()
        {
            if(r_Health <= 0f && r_InitializeSuperRegemTimer >= 1.9f)
            {
                
                entityState.SetNextState(new SkillStates.DestroyRideArmor());

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

                //Debug.Log($"VileRideArmorComponent - Ride Armor Regenerated: {r_RegemValue}, New Health: {r_Health}");
            }
        }

        private void DeplateShieldOverTime()
        {
            if (r_Shield > 0f)
            {
                r_Shield -= Time.fixedDeltaTime; // Deplete shield over time
                r_Shield = Mathf.Clamp(r_Shield, 0f, r_MaxShield);

                //Debug.Log($"VileRideArmorComponent - Ride Armor Shield Depleted: New Shield: {r_Shield}");
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

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_SFX_Recov_HP, this.gameObject);

            //Debug.Log($"VileRideArmorComponent - Initialized Ride Armor with Max Health: {r_MaxHealth}");

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

        public void AddShieldRA(float amount)
        {
            r_Shield += amount;

            r_Shield = Mathf.Clamp(r_Shield, 1f, r_MaxShield);
        }

        public float GetRHealthValue()
        {
            return r_Health;
        }

        public float GetRMaxHealthValue()
        {
            return r_MaxHealth;
        }

        public float GetRShieldValue()
        {
            return r_Shield;
        }

        public float GetRMaxShieldValue()
        {
            return r_MaxShield;
        }

        public float GetInverseLerpRHealthValue()
        {
            // Inverse lerp the bolt value to a range of 0 to 1
            return Mathf.InverseLerp(0, r_MaxHealth, r_Health);
        }

        public float GetInverseLerpRShieldValue()
        {
            // Inverse lerp the bolt value to a range of 0 to 1
            return Mathf.InverseLerp(0, r_MaxShield, r_Shield);
        }

        public bool IsRideArmorFullHealth()
        {
            
            return r_Health >= r_MaxHealth;
        }

        public bool IsRideArmorFullShield()
        {

            return r_Shield >= r_MaxShield;
        }

        private void HawkFlyingBehavior()
        {
            if (!Body.hasAuthority) return;

            if (!Body.HasBuff(VileBuffs.HawkBuff)) return;

            if (Body.characterMotor.isGrounded)
            {
                h_FlyingTimer = 0f;
                h_FlyingActivated = false;
                return;
            }

            // Ativa o voo se pulo for pressionado no ar e ainda não voou
            if (!h_FlyingActivated && Body.inputBank.jump.justPressed)
            {
                h_FlyingActivated = true;
                //Debug.Log("Hawk Flight Activated!");
            }

            if (h_FlyingActivated && Body.inputBank.jump.down)
            {
                if (h_FlyingTimer < h_FlyingDuration)
                {
                    h_FlyingSpeed = Body.moveSpeed * h_FlyingSpeedMultplier;

                    // Obtém direção horizontal do movimento (sem Y)
                    Vector3 moveInput = Body.inputBank.moveVector;
                    Vector3 horizontalDirection = new Vector3(moveInput.x, 0f, moveInput.z).normalized;

                    float horizontalSpeed = h_FlyingSpeed * 0.5f; // boost leve nos lados -- resolvi mudar para diminuir um pouco
                    float verticalSpeed = h_FlyingSpeed * 0.75f;     // impulso mais forte pra cima -- resolvi mudar para diminuir um pouco

                    // Aplica impulso horizontal
                    Vector3 horizontalVelocity = horizontalDirection * horizontalSpeed;

                    // Aplica impulso vertical (mantém o Y positivo)
                    float currentY = Mathf.Max(Body.characterMotor.velocity.y, 0f);
                    float newY = currentY + verticalSpeed;

                    // Define nova velocidade final
                    Vector3 finalVelocity = new Vector3(horizontalVelocity.x, newY, horizontalVelocity.z);

                    // Limita a velocidade total para evitar exagero
                    finalVelocity.x = Mathf.Clamp(finalVelocity.x, -h_FlyingSpeed, h_FlyingSpeed);
                    finalVelocity.z = Mathf.Clamp(finalVelocity.z, -h_FlyingSpeed, h_FlyingSpeed);
                    finalVelocity.y = Mathf.Clamp(finalVelocity.y, 0f, h_FlyingSpeed);

                    Body.characterMotor.velocity = finalVelocity;

                    Debug.Log("Hawk Flying - Horizontal Dir: " + horizontalDirection);
                    Debug.Log("Hawk Flying - Final Velocity: " + finalVelocity);
                }

                h_FlyingTimer += Time.fixedDeltaTime;
            }

            // Queda suave quando não está segurando o botão de pulo
            //if (h_FlyingActivated && !Body.inputBank.jump.down)
            //{
            //    if (Body.characterMotor.velocity.y < 0f)
            //    {
            //        //// Suaviza a queda reduzindo a velocidade vertical negativamente
            //        //Body.characterMotor.velocity += Vector3.up * Time.fixedDeltaTime * h_FallSlowStrength;

            //        //// Opcional: limita a queda para não ficar flutuando demais
            //        //Body.characterMotor.velocity.y = Mathf.Clamp(Body.characterMotor.velocity.y, -h_MaxFallSpeed, 999f);

            //        //Debug.Log("Hawk Falling - Softening Fall. Y Velocity: " + Body.characterMotor.velocity.y);

            //        float num = Body.characterMotor.velocity.y;
            //        num = Mathf.MoveTowards(num, h_MaxFallSpeed, h_FallSlowStrength);
            //        Body.characterMotor.velocity = new Vector3(Body.characterMotor.velocity.x, num, Body.characterMotor.velocity.z);

            //        Debug.Log("Hawk Falling - Softening Fall. Y Velocity: " + Body.characterMotor.velocity.y);
            //    }
            //}
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

                if (self.GetComponent<CharacterBody>().HasBuff(VileBuffs.GoliathBuff) || self.GetComponent<CharacterBody>().HasBuff(VileBuffs.HawkBuff) || self.GetComponent<CharacterBody>().HasBuff(VileBuffs.CyclopsBuff))
                {
                    float finalDamage = damageInfo.damage;

                    if (self.GetComponent<CharacterBody>())
                    {
                        float armor = self.GetComponent<CharacterBody>().armor;
                        float armorMultiplier = 100f / (100f + armor);
                        finalDamage *= armorMultiplier;
                    }

                    if(r_Shield > 0f)
                    {
                        r_Shield -= finalDamage;

                        //Debug.Log($"VileRideArmorComponent - Ride Armor Shield Decreased: {finalDamage}, New Shield: {r_Shield}");
                    }
                    else
                    {
                        r_Health -= finalDamage;


                        //Debug.Log($"VileRideArmorComponent - Ride Armor Health Decreased: {finalDamage}, New Health: {r_Health}");
                    }

                    

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