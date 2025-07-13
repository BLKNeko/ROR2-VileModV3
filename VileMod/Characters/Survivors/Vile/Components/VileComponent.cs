using UnityEngine;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Wamp;

namespace VileMod.Survivors.Vile.Components
{
    internal class VileComponent : NetworkBehaviour
    {

        private Transform modelTransform;

        private Animator Anim;
        private Animator AnimVeh;

        private HealthComponent HealthComp;

        private CharacterBody Body;

        private FootstepHandler footstepHandler;

        private CharacterModel model;
        private ChildLocator childLocator;
        private CameraTargetParams cameraTargetParams;

        private float minHpWeak = 0.45f;

        private bool isWeak;
        private Vector3 cameraDefaultPos;
        private Vector3 cameraMechaPos = new Vector3(0f, 2f, -12f);

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

            cameraTargetParams = Body.GetComponent<CameraTargetParams>();

            Debug.Log(AnimVeh);
            Debug.Log("Camera: " + cameraTargetParams);

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

            if (Body.HasBuff(VileBuffs.GoliathBuff))
                UpdateGoliathAnimator();

        }

        private void IsWeak()
        {
            isWeak = HealthComp.combinedHealthFraction < minHpWeak;

            Anim.SetBool("isWeak", isWeak);

        }

        public void EnterGoliath()
        {

            childLocator.FindChildGameObject("VEH").SetActive(false);
            childLocator.FindChildGameObject("VBodyMesh").SetActive(false);
            childLocator.FindChildGameObject("VH_VLC_Mesh").SetActive(false);
            childLocator.FindChildGameObject("VH_VLMKC_Mesh").SetActive(false);

            childLocator.FindChildGameObject("VEH").SetActive(true);

            if(Body.skinIndex == 0)
            {
                childLocator.FindChildGameObject("VH_VLC_Mesh").SetActive(true);
            }
            else
            {
                childLocator.FindChildGameObject("VH_VLMKC_Mesh").SetActive(true);
            }


            //cameraTargetParams.fovOverride = 60f;
            cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);


        }

        public void ExitGoliath()
        {
            childLocator.FindChildGameObject("VEH").SetActive(false);
            childLocator.FindChildGameObject("VH_VLC_Mesh").SetActive(false);
            childLocator.FindChildGameObject("VH_VLMKC_Mesh").SetActive(false);
            childLocator.FindChildGameObject("VBodyMesh").SetActive(true);

            cameraTargetParams.fovOverride = 0f;

        }

        private void UpdateGoliathAnimator()
        {
            AnimVeh.SetBool("isMoving", Anim.GetBool("isMoving"));
            AnimVeh.SetBool("isSprinting", Anim.GetBool("isSprinting"));
            AnimVeh.SetBool("isGrounded", Anim.GetBool("isGrounded"));
            AnimVeh.SetBool("inCombat", Anim.GetBool("inCombat"));


        }

    }
}