using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class EnterCyclopsEnd : BaseSkillState
    {

        public static float baseDuration = 1f;
        private float duration;
        private int boltCost;
        private VileComponent VC;
        private VileBoltComponent VBC;
        private VileRideArmorComponent VRAC;

        private ChildLocator childLocator;

        private Animator customAnimator;

        private string playbackRateParam;

        private bool rideFinished = false;

        private GameObject rideArmorInstance;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);

            VC = GetComponent<VileComponent>();

            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            customAnimator = childLocator.FindChildGameObject("CY").GetComponents<Animator>()[0];

            playbackRateParam = "Slash.playbackRate";

            //EffectManager.SimpleMuzzleFlash(VileAssets.gFallEffect, gameObject, "BasePos", true);

            Vector3 spawnPosition = characterBody.corePosition + Vector3.up * 20f;
            Quaternion rotation = Quaternion.identity;
            rideArmorInstance = UnityEngine.Object.Instantiate(VileAssets.cFallEffect, spawnPosition, rotation);


            //PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "Login", playbackRateParam, duration * 0.5f, 0.1f * duration);

        }

        public override void OnExit()
        {

            //AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Ride_Armor_In_SFX, this.gameObject);
            //PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "R_Login", playbackRateParam, duration * 0.5f, 0.1f * duration);

            //if (NetworkServer.active)
            //{
            //    characterBody.AddBuff(VileBuffs.CyclopsBuff);
            //    characterBody.AddBuff(VileBuffs.RideArmorEnabledBuff);
            //}

            //rideFinished = false;

            //PlayAnimation("Body", "IdleLock", "ShootGun.playbackRate", 1.8f);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            rideArmorInstance.transform.position = Vector3.MoveTowards(rideArmorInstance.transform.position, characterBody.transform.position, 60f * Time.fixedDeltaTime);

            if (!rideFinished && Vector3.Distance(rideArmorInstance.transform.position, characterBody.corePosition) < 0.1f)
            {
                RideFinished();
            }

            if (!rideFinished)
            {
                RideFinished();
            }

        }

        private void RideFinished()
        {
            if (rideFinished) return;

            // Impacto
            //Util.PlaySound("Play_missile_impact", gameObject);


            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CharacterLandImpact"), new EffectData
            {
                origin = characterBody.corePosition,
                scale = 4f
            }, true);

            if (rideArmorInstance)
            {
                GameObject.Destroy(rideArmorInstance);
                rideArmorInstance = null;
            }

            //VC.EnterCyclops(true);

            rideFinished = true;

            // Pule direto pro estado principal
            //outer.SetNextStateToMain();

            if (isAuthority) outer.SetNextState(new EnterCyclopsEnd2());

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}