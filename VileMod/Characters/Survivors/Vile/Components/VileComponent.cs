using UnityEngine;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Wamp;
using ExtraSkillSlots;

namespace VileMod.Survivors.Vile.Components
{
    internal class VileComponent : NetworkBehaviour
    {

        private Transform modelTransform;

        private Animator Anim;
        private Animator AnimVeh;
        private Animator AnimHK;

        private HealthComponent HealthComp;

        private CharacterBody Body;

        private FootstepHandler footstepHandler;

        private CharacterModel model;
        private ChildLocator childLocator;
        private CameraTargetParams cameraTargetParams;

        private ExtraSkillLocator extraskillLocator;

        private VileRideArmorComponent rideArmorComponent;

        private HuntressTracker tracker;

        private float minHpWeak = 0.45f;

        private bool isWeak;
        private Vector3 cameraDefaultPos;
        private Vector3 cameraMechaPos = new Vector3(0f, 2f, -12f);

        private float baseHeatValue;
        private float baseOverHeatValue;
        private float shockElementValue;
        private float flameElementValue;
        private float iceElementValue;

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

            footstepHandler = Body.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().GetComponent<FootstepHandler>();

            model = Body.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();

            childLocator = Body.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().GetComponent<ChildLocator>();

            Anim = Body.characterDirection.modelAnimator;

            AnimVeh = childLocator.FindChildGameObject("VEH").GetComponents<Animator>()[0];

            AnimHK = childLocator.FindChildGameObject("HAWK").GetComponents<Animator>()[0];

            cameraTargetParams = Body.GetComponent<CameraTargetParams>();

            extraskillLocator = GetComponent<ExtraSkillLocator>();

            rideArmorComponent = GetComponent<VileRideArmorComponent>();

            tracker = GetComponent<HuntressTracker>();

            tracker.enabled = false;

            //Debug.Log(AnimVeh);
            //Debug.Log("Camera: " + cameraTargetParams);

            //Debug.Log("EXEmodel: " + EXEmodel);
            //Debug.Log("EXEchildLocator: " + EXEchildLocator);

            //Debug.Log("footstepHandler: " + footstepHandler);

            //switch (XConfig.enableXFootstep.Value)
            //{
            //    case 0:
            //        footstepHandler.baseFootstepString = "";
            //        footstepHandler.sprintFootstepOverrideString = "";
            //        break;
            //    case 1:
            //        footstepHandler.baseFootstepString = "Play_X_Footstep_SFX";
            //        footstepHandler.sprintFootstepOverrideString = "Play_X_Footstep_SFX";
            //        break;
            //    case 2:
            //        footstepHandler.baseFootstepString = "Play_X_Footstep_X8_SFX";
            //        footstepHandler.sprintFootstepOverrideString = "Play_X_Footstep_X8_SFX";
            //        break;
            //    default:
            //        footstepHandler.baseFootstepString = "";
            //        footstepHandler.sprintFootstepOverrideString = "";
            //        break;
            //}


        }

        private void FixedUpdate()
        {
            if(!Body.hasAuthority) return;

            IsWeak();
            VHeatUpdate();
            VElementUpdate();
            VElementBuffUpdate();
            SetOverHeat();

            if (Body.HasBuff(VileBuffs.OverHeatDebuff))
            {
                OverHeatBehavior();
            }

            if (Body.HasBuff(VileBuffs.GoliathBuff))
                UpdateGoliathAnimator();

            if (Body.HasBuff(VileBuffs.HawkBuff))
                UpdateHawkAnimator();

        }

        private void IsWeak()
        {
            isWeak = HealthComp.combinedHealthFraction < minHpWeak;

            Anim.SetBool("isWeak", isWeak);

        }

        public CharacterBody GetVileBody()
        {
            return Body;
        }

        

        private void VHeatUpdate()
        {
            float delta = Time.fixedDeltaTime * 0.1f;

            if (!Body.HasBuff(VileBuffs.PrimaryHeatBuff))
            {
                baseHeatValue -= delta;
                baseOverHeatValue -= delta;
            } 
            else
                baseHeatValue += delta;

            if (Body.HasBuff(VileBuffs.PrimaryHeatBuff) && baseHeatValue >= 0.95f)
                baseOverHeatValue += delta;
            

            baseHeatValue = Mathf.Clamp01(baseHeatValue);
            baseOverHeatValue = Mathf.Clamp01(baseOverHeatValue);

            //Debug.Log("Base Heat Value: " + baseHeatValue);
        }

        private void VElementUpdate()
        {
            float delta = Time.fixedDeltaTime * 0.05f;

            if (iceElementValue >= 0)
                iceElementValue -= delta;

            if (flameElementValue >= 0)
                flameElementValue -= delta;

            if (shockElementValue >= 0)
                shockElementValue -= delta;


            iceElementValue = Mathf.Clamp01(iceElementValue);
            flameElementValue = Mathf.Clamp01(flameElementValue);
            shockElementValue = Mathf.Clamp01(shockElementValue);

            //Debug.Log("iceElementValue Value: " + iceElementValue);
            //Debug.Log("flameElementValue Value: " + flameElementValue);
            //Debug.Log("shockElementValue Value: " + shockElementValue);
        }

        private void VElementBuffUpdate()
        {
            UpdateBuff(VileBuffs.PrimaryIceBuff, iceElementValue);
            UpdateBuff(VileBuffs.PrimaryFlameBuff, flameElementValue);
            UpdateBuff(VileBuffs.PrimaryShockBuff, shockElementValue);
        }

        public void SetExtraHeatValues(float heat)
        {
            if (!Body.hasAuthority) return;

            if (baseOverHeatValue >= 0.1f)
            {
                baseOverHeatValue += heat;
            }
            else
            {
                baseHeatValue += heat;
            }

        }

        public void SetHeatValue(float heat)
        {
            baseHeatValue = heat;
        }

        public void SetOverHeatValue(float heat)
        {
            baseOverHeatValue = heat;
        }

        private void SetOverHeat()
        {
            if (!Body.hasAuthority) return;

            if (baseOverHeatValue >= 0.99f && !Body.HasBuff(VileBuffs.OverHeatDebuff)) 
            {
                Body.AddTimedBuff(VileBuffs.OverHeatDebuff, 7f);
            }

        }

        private void OverHeatBehavior()
        {
            Body.skillLocator.primary.temporaryCooldownPenalty = 2f;
            Body.skillLocator.secondary.temporaryCooldownPenalty = 3f;
            Body.skillLocator.utility.temporaryCooldownPenalty = 4f;
            Body.skillLocator.special.temporaryCooldownPenalty = 5f;
        }

        public void SetElementValues(float iceValue, float shockValue, float flameValue, bool iceReset, bool shockReset, bool flameReset)
        {
            if (!Body.hasAuthority) return;

            iceElementValue += iceValue;
            shockElementValue += shockValue;
            flameElementValue += flameValue;

            iceElementValue = iceReset ? 0f : iceElementValue;
            shockElementValue = shockReset ? 0f : shockElementValue;
            flameElementValue = flameReset ? 0f : flameElementValue;

            iceElementValue = Mathf.Clamp01(iceElementValue);
            flameElementValue = Mathf.Clamp01(flameElementValue);
            shockElementValue = Mathf.Clamp01(shockElementValue);

            Debug.Log("Set Element Values: " + iceElementValue + ", " + shockElementValue + ", " + flameElementValue);
        }

        private void UpdateBuff(BuffDef buff, float value)
        {
            bool hasBuff = Body.HasBuff(buff);

            if (value > 0f && !hasBuff)
                Body.AddBuff(buff);
            else if (value <= 0f && hasBuff)
                Body.RemoveBuff(buff);
        }

        public float GetBaseHeatValue()
        {
            return baseHeatValue;
        }

        public float GetBaseOverHeatValue()
        {
            return baseOverHeatValue;
        }

        public float GeticeElementValue()
        {
            return iceElementValue;
        }

        public float GetShockElementValue()
        {
            return shockElementValue;
        }

        public float GetFlameElementValue()
        {
            return flameElementValue;
        }

        public void EnterGoliath()
        {

            DeactivateChilds();

            childLocator.FindChildGameObject("VEH").SetActive(true);

            if (Body.skinIndex == 0)
            {
                childLocator.FindChildGameObject("VH_VLC_Mesh").SetActive(true);
            }
            else
            {
                childLocator.FindChildGameObject("VH_VLMKC_Mesh").SetActive(true);
            }


            //cameraTargetParams.fovOverride = 60f;
            cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);

            RemoveSkills();

            Body.skillLocator.primary.SetSkillOverride(Body.skillLocator.primary, VileSurvivor.goliathPunchComboSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            Body.skillLocator.secondary.SetSkillOverride(Body.skillLocator.secondary, VileSurvivor.goliathShootSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            Body.skillLocator.utility.SetSkillOverride(Body.skillLocator.utility, VileSurvivor.goliathDashPunchSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            Body.skillLocator.special.SetSkillOverride(Body.skillLocator.special, VileSurvivor.rideRapairSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, VileSurvivor.exitGoliathSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            Body.skillLocator.primary.Reset();
            Body.skillLocator.secondary.Reset();
            Body.skillLocator.utility.Reset();
            Body.skillLocator.special.Reset();
            extraskillLocator.extraFourth.Reset();

            if(!Body.HasBuff(VileBuffs.RideArmorEnabledBuff))
                rideArmorComponent.InitializeRideArmor();

        }

        public void ExitGoliath()
        {
            DeactivateChilds();

            childLocator.FindChildGameObject("VBodyMesh").SetActive(true);

            cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);

            RemoveSkills();

            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, VileSurvivor.resumeGoliathSkillDef, GenericSkill.SkillOverridePriority.Contextual);

        }

        private void UpdateGoliathAnimator()
        {
            AnimVeh.SetBool("isMoving", Anim.GetBool("isMoving"));
            AnimVeh.SetBool("isSprinting", Anim.GetBool("isSprinting"));
            AnimVeh.SetBool("isGrounded", Anim.GetBool("isGrounded"));
            AnimVeh.SetBool("inCombat", Anim.GetBool("inCombat"));


        }

        // HAWK

        public void EnterHawk()
        {

            DeactivateChilds();

            tracker.enabled = true;

            childLocator.FindChildGameObject("HAWK").SetActive(true);

            if (Body.skinIndex == 0)
            {
                childLocator.FindChildGameObject("HK_VLC_Mesh").SetActive(true);
            }
            else
            {
                childLocator.FindChildGameObject("HK_VLMKC_Mesh").SetActive(true);
            }


            //cameraTargetParams.fovOverride = 60f;
            cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);

            RemoveSkills();

            Body.skillLocator.primary.SetSkillOverride(Body.skillLocator.primary, VileSurvivor.hawkGunSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            Body.skillLocator.secondary.SetSkillOverride(Body.skillLocator.secondary, VileSurvivor.goliathShootSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            Body.skillLocator.utility.SetSkillOverride(Body.skillLocator.utility, VileSurvivor.goliathDashPunchSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            Body.skillLocator.special.SetSkillOverride(Body.skillLocator.special, VileSurvivor.rideRapairSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, VileSurvivor.exitHawkSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            Body.skillLocator.primary.Reset();
            Body.skillLocator.secondary.Reset();
            Body.skillLocator.utility.Reset();
            Body.skillLocator.special.Reset();
            extraskillLocator.extraFourth.Reset();

            if (!Body.HasBuff(VileBuffs.RideArmorEnabledBuff))
                rideArmorComponent.InitializeRideArmor();

        }

        public void ExitHawk()
        {
            DeactivateChilds();

            tracker.enabled = false;

            childLocator.FindChildGameObject("VBodyMesh").SetActive(true);

            cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);

            RemoveSkills();

            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, VileSurvivor.resumeHawkSkillDef, GenericSkill.SkillOverridePriority.Contextual);

        }

        private void UpdateHawkAnimator()
        {
            AnimHK.SetBool("isMoving", Anim.GetBool("isMoving"));
            AnimHK.SetBool("isSprinting", Anim.GetBool("isSprinting"));
            AnimHK.SetBool("isGrounded", Anim.GetBool("isGrounded"));
            AnimHK.SetBool("inCombat", Anim.GetBool("inCombat"));


        }

        public void DestroyRideArmor()
        {
            DeactivateChilds();

            tracker.enabled = false;

            childLocator.FindChildGameObject("VBodyMesh").SetActive(true);

            cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);

            RemoveSkills();
        }

        private void DeactivateChilds()
        {
            childLocator.FindChildGameObject("VBodyMesh").SetActive(false);

            childLocator.FindChildGameObject("VEH").SetActive(false);
            childLocator.FindChildGameObject("VH_VLC_Mesh").SetActive(false);
            childLocator.FindChildGameObject("VH_VLMKC_Mesh").SetActive(false);

            childLocator.FindChildGameObject("HAWK").SetActive(false);
            childLocator.FindChildGameObject("HK_VLC_Mesh").SetActive(false);
            childLocator.FindChildGameObject("HK_VLMKC_Mesh").SetActive(false);

        }

        private void RemoveSkills()
        {
            foreach (var skill in SkillDefs)
            {
                Body.skillLocator.primary.UnsetSkillOverride(Body.skillLocator.primary, skill, GenericSkill.SkillOverridePriority.Contextual);
                Body.skillLocator.secondary.UnsetSkillOverride(Body.skillLocator.secondary, skill, GenericSkill.SkillOverridePriority.Contextual);
                Body.skillLocator.utility.UnsetSkillOverride(Body.skillLocator.utility, skill, GenericSkill.SkillOverridePriority.Contextual);
                Body.skillLocator.special.UnsetSkillOverride(Body.skillLocator.special, skill, GenericSkill.SkillOverridePriority.Contextual);


                extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, skill, GenericSkill.SkillOverridePriority.Contextual);

            }
        }

        private static readonly SkillDef[] SkillDefs =
        {
            VileSurvivor.goliathPunchComboSkillDef,
            VileSurvivor.goliathShootSkillDef,
            VileSurvivor.goliathDashPunchSkillDef,

            VileSurvivor.enterGoliathSkillDef,
            VileSurvivor.exitGoliathSkillDef,
            VileSurvivor.resumeGoliathSkillDef,

            VileSurvivor.rideRapairSkillDef,


            VileSurvivor.cherryBlastSkillDef,
            VileSurvivor.vileBurningDriveSkillDef
            
        };

    }
}