using VileMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using EntityStates;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class GDashPunch : BaseMeleeAttackNeko2
    {

        private float dashSpeed;
        private Vector3 dashDirection;
        private Vector3 idealDirection;
        public static float speedMultiplier = 5f;

        private float afterImageTimer = 0f;

        private Transform modelTransform;
        private CharacterModel characterModel;

        public override void OnEnter()
        {
            hitboxGroupName = "GoliathHitbox";

            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = VileStaticValues.GDashPunchDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;
            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.8f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.2f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = VileStaticValues.Play_Vile_GPunch_SFX;
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = VileAssets.swordSwingEffect;
            hitEffectPrefab = VileAssets.swordHitImpactEffect;

            impactSound = VileAssets.swordHitSoundEvent.index;

            customAnimator = childLocator.FindChildGameObject("VEH").GetComponents<Animator>()[0];
            SetCustomAnimator(customAnimator);
            //playCustomExitAnim = true;

            SetHitReset(true, 8);

            //Debug.Log("GP2");

            //GPunch1 GP1 = new GPunch1();
            //SetNextEntityState(GP1);

            if (base.isAuthority)
            {
                if (base.inputBank)
                {
                    idealDirection = base.inputBank.aimDirection;
                    idealDirection.y = 0f;
                }
                UpdateDirection();
            }

            if (base.modelLocator)
            {
                base.modelLocator.normalizeToFloor = true;
            }

            if (base.characterDirection)
            {
                base.characterDirection.forward = idealDirection;
            }

            modelTransform = base.GetModelTransform();
            characterModel = characterBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();

            CreateAfterImage();

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Ride_Armor_Boost, this.gameObject);

            base.OnEnter();
        }

        private void CreateAfterImage()
        {

            //Debug.LogWarning("ChieldLocator: " + childLocator);
            //Debug.LogWarning("characterModel: " + characterModel);
            //Debug.LogWarning("childLocator.FindChildGameObject(VEH): " + childLocator.FindChildGameObject("VH_Mesh_M"));
            //Debug.LogWarning("childLocator.FindChildGameObject(VEH)SkinnedMeshRenderer: " + childLocator.FindChildGameObject("VH_Mesh_M").GetComponent<SkinnedMeshRenderer>());

            if (modelTransform)
            {
                TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(this.modelTransform.gameObject);
                temporaryOverlayInstance.duration = 0.5f;
                temporaryOverlayInstance.animateShaderAlpha = true;
                temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlayInstance.destroyComponentOnEnd = true;
                temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matGhostEffect");
                temporaryOverlayInstance.inspectorCharacterModel = characterModel;
                temporaryOverlayInstance.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
            }

            //if (!characterBody || !characterBody.modelLocator || !characterBody.modelLocator.modelTransform)
            //{
            //    Debug.LogWarning("CreateAfterImage: characterBody or modelLocator is null");
            //    return;
            //}

            //var skinnedRenderer = childLocator.FindChildGameObject("VH_Mesh_M").GetComponent<SkinnedMeshRenderer>();
            //if (!skinnedRenderer)
            //{
            //    Debug.LogWarning("CreateAfterImage: SkinnedMeshRenderer not found");
            //    return;
            //}

            //var mesh = new Mesh();
            //skinnedRenderer.BakeMesh(mesh);

            //GameObject ghostObject = new GameObject("AfterImageGhost");
            //ghostObject.transform.position = modelTransform.position;
            //ghostObject.transform.rotation = modelTransform.rotation;
            //ghostObject.transform.localScale = modelTransform.lossyScale;

            //var meshFilter = ghostObject.AddComponent<MeshFilter>();
            //meshFilter.sharedMesh = mesh;

            //var meshRenderer = ghostObject.AddComponent<MeshRenderer>();
            //var ghostMat = LegacyResourcesAPI.Load<Material>("Materials/matGhostEffect");
            //if (!ghostMat)
            //{
            //    Debug.LogWarning("CreateAfterImage: matGhostEffect not found");
            //    return;
            //}

            //meshRenderer.material = ghostMat;

            //// Fade e destruição automática
            //ghostObject.AddComponent<DestroyGhost>().Initialize(0.4f);


        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isAuthority)
            {

                afterImageTimer -= Time.fixedDeltaTime;
                if (afterImageTimer <= 0f)
                {
                    CreateAfterImage();
                    afterImageTimer = 0.09f; // intervalo entre fantasmas
                }

                if (isGrounded)
                    UpdateDirection();

                if (base.characterDirection)
                {
                    base.characterDirection.moveVector = this.idealDirection;
                    if (base.characterMotor)
                    {
                        base.characterMotor.rootMotion += this.GetIdealVelocity() * base.GetDeltaTime();
                    }
                }

            }

            //if (characterMotor && dashDirection != Vector3.zero)
            //{
            //    // Mantém o momentum horizontal, respeitando a gravidade
            //    Vector3 newVelocity = dashDirection * dashSpeed;
            //    newVelocity.y = characterMotor.velocity.y;
            //    characterMotor.velocity = newVelocity;
            //}

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
            }
        }

        private Vector3 GetIdealVelocity()
        {
            return base.characterDirection.forward * base.characterBody.moveSpeed * speedMultiplier;
        }

        private void UpdateDirection()
        {
            if (base.inputBank)
            {
                Vector2 vector = Util.Vector3XZToVector2XY(base.inputBank.moveVector);
                if (vector != Vector2.zero)
                {
                    vector.Normalize();
                    idealDirection = new Vector3(vector.x, 0f, vector.y).normalized;
                }
            }
        }

        protected override void PlayCustomExitAnimation()
        {
            //PlayCrossfade("Gesture, Override", "VEH_ATK0_L_END", playbackRateParam, duration, 0.1f * duration);
            //PlayAnimationOnAnimator(customAnimator, "Gesture, Override", "VEH_ATK0_R_END", playbackRateParam, duration * 0.2f, 0.1f * duration);
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "VEH_ATK0_L", playbackRateParam, duration, 0.1f * duration);\
            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "VEHDashS", playbackRateParam, duration * 0.3f, 0.1f * duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {
            if (base.characterMotor)
            {
                base.characterMotor.velocity = Vector3.zero;
            }

            if (base.modelLocator)
            {
                base.modelLocator.normalizeToFloor = false;
            }

            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "VEHDashE", playbackRateParam, duration * 0.3f, 0.1f * duration);



            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(idealDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            idealDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

    }
}