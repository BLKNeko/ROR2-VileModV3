using EntityStates;
using RoR2;
using UnityEngine;
using VileMod.Modules;
using VileMod.SkillStates.BaseStates;

namespace VileMod.SkillStates
{
    public class Triple7 : BaseSkillState
    {
        public static float damageCoefficient = 0.5f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        public static float force = 100f;
        public static float recoil = 0f;
        public static float range = 180f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public float shootdelay = 1.5f;
        public float timer = 2f;
        public bool shootsfx = true;

        public static bool heat;
        public static int buffSkillIndex;
        public static float Chilldelay;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Triple7.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Weapon";

            base.PlayAnimation("Gesture, Override", "CannonShoot", "attackSpeed", this.duration);

            if (heat)
                shootdelay = (Chilldelay - 0.3f);
            else
                shootdelay = Chilldelay;

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireShotgun.effectPrefab, base.gameObject, this.muzzleString, false);
                //Util.PlaySound("HenryShootPistol", base.gameObject);

                if (shootsfx)
                    Util.PlaySound(Sounds.vileCherryBlast, base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    //base.AddRecoil(-1f * Shoot.recoil, -2f * Shoot.recoil, -0.5f * Shoot.recoil, 0.5f * Shoot.recoil);


                    BulletAttack T7 = new BulletAttack();
                    T7.bulletCount = 1;
                    T7.aimVector = aimRay.direction;
                    T7.origin = aimRay.origin;
                    T7.damage = Triple7.damageCoefficient * this.damageStat;
                    T7.damageColorIndex = DamageColorIndex.Default;
                    //T7.damageType = DamageType.Generic;
                    T7.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                    T7.maxDistance = Triple7.range;
                    T7.force = Triple7.force;
                    T7.hitMask = LayerIndex.CommonMasks.bullet;
                    T7.minSpread = 0.1f;
                    T7.maxSpread = 2.5f;
                    T7.isCrit = base.RollCrit();
                    T7.owner = base.gameObject;
                    T7.muzzleName = muzzleString;
                    T7.smartCollision = false;
                    T7.procChainMask = default(ProcChainMask);
                    T7.procCoefficient = procCoefficient;
                    T7.radius = 0.75f;
                    T7.sniper = false;
                    T7.stopperMask = LayerIndex.CommonMasks.bullet;
                    T7.weapon = null;
                    T7.tracerEffectPrefab = Triple7.tracerEffectPrefab;
                    T7.spreadPitchScale = 1.5f;
                    T7.spreadYawScale = 2f;
                    T7.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                    T7.hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireShotgun.hitEffectPrefab;


                    switch (buffSkillIndex)
                    {
                        case 0:
                            T7.damageType = (Util.CheckRoll(5f, base.characterBody.master) ? DamageType.SlowOnHit : DamageType.Generic);
                            break;
                        case 1:
                            T7.damageType = (Util.CheckRoll(8f, base.characterBody.master) ? DamageType.Stun1s : DamageType.Generic);
                            break;
                        case 2:
                            T7.damageType = (Util.CheckRoll(8f, base.characterBody.master) ? DamageType.Shock5s : DamageType.Generic);
                            break;
                        case 3:
                            T7.damageType = (Util.CheckRoll(8f, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic);
                            break;
                        default:
                            T7.damageType = (Util.CheckRoll(5f, base.characterBody.master) ? DamageType.SlowOnHit : DamageType.Generic);
                            break;

                    }

                    T7.Fire();


                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            timer += Time.deltaTime;

            //if (base.fixedAge >= this.fireTime)
            //{
            //    this.Fire();
            //}

            //if (base.fixedAge >= this.duration && base.isAuthority)
            //{
            //    this.outer.SetNextStateToMain();
            //    return;
            //}

            if (base.inputBank.skill1.down)
            {

                // if (Fury.isInFury)
                if (base.characterBody.HasBuff(Modules.Buffs.VileFuryBuff))
                {

                    if (timer > shootdelay)
                    {
                        shootdelay = 0.05f;

                        if (shootsfx)
                            shootsfx = false;
                        else
                            shootsfx = true;




                        timer = 0;
                        hasFired = false;
                        base.characterBody.SetAimTimer(1f);
                        base.PlayAnimation("Gesture, Override", "CannonShoot", "attackSpeed", this.duration);
                        base.characterBody.isSprinting = false;
                        Fire();
                    }


                }
                else
                {

                    if (timer > shootdelay)
                    {


                        if (shootdelay <= 0.085f)
                        {
                            shootdelay = 0.085f;
                            if (shootsfx)
                                shootsfx = false;
                            else
                                shootsfx = true;
                        }
                        else
                        {
                            if (shootdelay <= 1.6 && shootdelay >= 1.2)
                                shootdelay -= (0.25f + (base.attackSpeedStat / 50));

                            else if (shootdelay <= 1.19 && shootdelay >= 1)
                                shootdelay -= (0.18f + (base.attackSpeedStat / 50));

                            else if (shootdelay <= 0.99 && shootdelay >= 0.8)
                                shootdelay -= (0.085f + (base.attackSpeedStat / 50));

                            else if (shootdelay <= 0.79 && shootdelay >= 0.5)
                                shootdelay -= (0.045f + (base.attackSpeedStat / 50));

                            else if (shootdelay <= 0.49 && shootdelay >= 0.2)
                                shootdelay -= (0.015f + (base.attackSpeedStat / 50));

                            else if (shootdelay <= 0.19)
                                shootdelay -= (0.008f + (base.attackSpeedStat / 50));

                            else
                                shootdelay -= (0.010f + (base.attackSpeedStat / 50));
                        }



                        timer = 0;
                        hasFired = false;
                        base.characterBody.SetAimTimer(1f);
                        base.PlayAnimation("Gesture, Override", "CannonShoot", "attackSpeed", this.duration);
                        base.characterBody.isSprinting = false;
                        Fire();
                    }


                }


                

                


            }

            if (base.fixedAge >= this.duration && base.isAuthority && !base.inputBank.skill1.down)
            {

                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}