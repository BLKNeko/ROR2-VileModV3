using EntityStates;
using RoR2;
using UnityEngine;
using VileMod.Modules;
using VileMod.SkillStates.BaseStates;

namespace VileMod.SkillStates
{
    public class CherryBlast : BaseSkillState
    {
        public float damageCoefficient = 0.25f;
        public float baseDuration = 1f;
        public float recoil = 1f;
        public static float procCoefficient = 1f;
        public static float force = 100f;
        public static float range = 200f;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerClayBruiserMinigun");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");
        public int bulletcount;
        public bool shootsfx = true;

        public static bool heat;
        public static int buffSkillIndex;

        public static float Chilldelay;
        public float shootdelay = 1.5f;
        public float timer = 2f;

        private float duration;
        private float fireDuration;
        private bool hasFired = true;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            if (bulletcount == 0)
                bulletcount = 1;

            this.duration = this.baseDuration / base.attackSpeedStat;
            this.fireDuration = 0.25f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Weapon";
            base.characterBody.isSprinting = false;

            //base.characterBody.healthComponent.AddBarrier(base.characterBody.damage);


            if (heat)
                shootdelay = (Chilldelay - 0.3f);
            else
                shootdelay = Chilldelay;


            //shootdelay -= (base.attackSpeedStat / 10);

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireArrow()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.SetSpreadBloom(0.8f);
                Ray aimRay = base.GetAimRay();
                //EffectManager.SimpleMuzzleFlash(EntityStates.Bandit2.Weapon.Bandit2FireShiv.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    //ProjectileManager.instance.FireProjectile(MegamanXVileSurvivor.MegamanXVile.arrowProjectile, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    /*
                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0.1f,
                        maxSpread = 0.6f,
                        damage = damageCoefficient * this.damageStat,
                        damageType = (Util.CheckRoll(5f, base.characterBody.master) ? DamageType.SlowOnHit : DamageType.Generic),
                        procChainMask = default(ProcChainMask),
                        force = 45f,
                        radius = 0.4f,
                        sniper = true,
                        spreadPitchScale = 0.5f,
                        spreadYawScale = 0.5f,
                        tracerEffectPrefab = CherryBlast.tracerEffectPrefab,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        falloffModel = BulletAttack.FalloffModel.None,
                        muzzleName = muzzleString,
                        hitEffectPrefab = hitEffectPrefab,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        isCrit = Util.CheckRoll(base.critStat, base.characterBody.master)
                    }.Fire();
                    */

                    BulletAttack BT = new BulletAttack();
                    BT.owner = base.gameObject;
                    //BT.weapon = base.gameObject;
                    BT.origin = aimRay.origin;
                    BT.aimVector = aimRay.direction;
                    BT.minSpread = 0.1f;
                    BT.maxSpread = 1f;
                    BT.damage = damageCoefficient * this.damageStat;
                    BT.spreadPitchScale = 0.1f;
                    BT.spreadYawScale = 1f;
                    BT.tracerEffectPrefab = CherryBlast.tracerEffectPrefab;
                    BT.muzzleName = muzzleString;
                    BT.hitEffectPrefab = hitEffectPrefab;
                    BT.isCrit = Fury.isCrit;
                    BT.bulletCount = 1;
                    BT.damageColorIndex = DamageColorIndex.Default;
                    BT.falloffModel = BulletAttack.FalloffModel.DefaultBullet;
                    BT.maxDistance = CherryBlast.range;
                    BT.force = CherryBlast.force;
                    BT.hitMask = LayerIndex.CommonMasks.bullet;
                    BT.smartCollision = false;
                    BT.procChainMask = default(ProcChainMask);
                    BT.procCoefficient = procCoefficient;
                    BT.radius = 0.75f;
                    BT.sniper = false;
                    BT.stopperMask = LayerIndex.CommonMasks.bullet;
                    BT.weapon = null;
                    BT.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;




                    switch (buffSkillIndex)
                    {
                        case 0:
                            BT.damageType = (Util.CheckRoll(5f, base.characterBody.master) ? DamageType.SlowOnHit : DamageType.Generic);
                            break;
                        case 1:
                            BT.damageType = (Util.CheckRoll(8f, base.characterBody.master) ? DamageType.Stun1s : DamageType.Generic);
                            break;
                        case 2:
                            BT.damageType = (Util.CheckRoll(8f, base.characterBody.master) ? DamageType.Shock5s : DamageType.Generic);
                            break;
                        case 3:
                            BT.damageType = (Util.CheckRoll(8f, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic);
                            break;
                        default:
                            BT.damageType = (Util.CheckRoll(5f, base.characterBody.master) ? DamageType.SlowOnHit : DamageType.Generic);
                            break;

                    }

                     if(shootsfx)
                     Util.PlaySound(Sounds.vileCherryBlast, base.gameObject);
                    // Util.PlaySound(Sounds.vileCherryBlast, base.gameObject);


                    BT.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            timer += Time.deltaTime;

            if (base.inputBank.skill1.down && hasFired)
            {

                //if (Fury.isInFury)
                if (base.characterBody.HasBuff(Modules.Buffs.VileFuryBuff))
                {

                    if (timer > shootdelay)
                    {
                        shootdelay = 0.045f;

                        if (shootsfx)
                            shootsfx = false;
                        else
                            shootsfx = true;




                        timer = 0;
                        hasFired = false;
                        base.characterBody.SetAimTimer(1f);
                        base.PlayAnimation("Gesture, Override", "CannonShoot", "attackSpeed", this.duration);
                        base.characterBody.isSprinting = false;
                        FireArrow();
                    }

                 
                }
                else
                {

                    if (timer > shootdelay)
                    {
                        if (shootdelay <= 0.075f)
                        {
                            shootdelay = 0.075f;
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
                        FireArrow();
                    }

                }

                
            }


            // if (base.fixedAge >= this.fireDuration)
            // {
            //FireArrow();
            //}

            if (base.fixedAge >= this.duration && base.isAuthority && !base.inputBank.skill1.down)
            {
                shootdelay = 1.5f;
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
