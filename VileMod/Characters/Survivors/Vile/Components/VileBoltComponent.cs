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
    internal class VileBoltComponent : MonoBehaviour
    {
        private Transform modelTransform;

        private HealthComponent HealthComp;

        private CharacterBody Body;


        private CharacterModel model;
        private ChildLocator childLocator;


        private int boltValue;
        private uint lastMoney = 0;


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

            //Hook();

        }

        private void FixedUpdate()
        {
            if (Body && Body.hasAuthority && Body.master)
            {
                uint currentMoney = Body.master.money;
                if (currentMoney != lastMoney)
                {
                    if (currentMoney > lastMoney)
                    {
                        uint gained = currentMoney - lastMoney;

                        float reduction = GetBoltReduction(Body.level);
                        gained = (uint)(gained * (1f - reduction));

                        boltValue += (int)gained;
                        boltValue = Mathf.Clamp(boltValue, 0, 1000);
                    }

                    lastMoney = currentMoney;
                }
            }
        }

        private float GetBoltReduction(float level)
        {
            float reduction;

            if (level <= 10f)
            {
                // 0% -> 33% (nível 1 a 10)
                reduction = Mathf.Lerp(0f, 0.5f, level / 10f);
            }
            else if (level <= 20f)
            {
                // 33% -> 50% (nível 10 a 20)
                reduction = Mathf.Lerp(0.5f, 0.8f, (level - 10f) / 10f);
            }
            else
            {
                // 50% -> 70% (nível 20+)
                // A cada 30 níveis, sobe proporcionalmente até chegar no limite
                reduction = Mathf.Lerp(0.8f, 0.95f, (level - 20f) / 30f);
            }

            return Mathf.Min(reduction, 0.95f); // Garante máximo de 95%
        }

        //private void Hook()
        //{
        //    //On.RoR2.CameraRigController.Update += CameraRigController_Update;
        //    //On.RoR2.UI.HUD.Update += HUD_Update;
        //    On.RoR2.CharacterMaster.GiveMoney += CharacterMaster_GiveMoney;

        //}

        public int GetBoltValue()
        {
            return boltValue;
        }

        public float GetInverseLerpBoltValue()
        {
            // Inverse lerp the bolt value to a range of 0 to 1
            return Mathf.InverseLerp(0, 1000, boltValue);
        }

        public void ChangeBoltValue(int value)
        {
            boltValue += value;
            boltValue = Mathf.Clamp(boltValue, 0, 1000); // Limita o valor entre 0 e 9999
            //Debug.Log("Bolt Value Changed: " + boltValue);
        }

        public void SetBoltValue(int value)
        {
            boltValue = value;
            boltValue = Mathf.Clamp(boltValue, 0, 1000); // Limita o valor entre 0 e 9999
            //Debug.Log("Bolt Value Changed: " + boltValue);
        }

        //private void CharacterMaster_GiveMoney(On.RoR2.CharacterMaster.orig_GiveMoney orig, CharacterMaster self, uint amount)
        //{
        //    // Chama o original para não quebrar o jogo
        //    orig(self, amount);

        //    //Debug.Log("Self: " + self);
        //    //Debug.Log("Self Body: " + self.GetBody());
        //    //Debug.Log("Body: " + Body);
        //    //Debug.Log("Body.hasAuthority: " + Body.hasAuthority);
        //    // Verifica se esse master tem um corpo e é o nosso personagem
        //    if (self.GetBody() && self.GetBody() == Body && Body.hasAuthority) 
        //    {
        //        boltValue += (int)amount;

        //        //Debug.Log("amount: " + amount);
        //        //Debug.Log("boltValue: " + boltValue);

        //        boltValue = Mathf.Clamp(boltValue, 0, 1000); // Limita o valor entre 0 e 9999

        //    }
        //}

        //public void Unhook()
        //{
        //    //On.RoR2.CameraRigController.Update -= CameraRigController_Update;
        //    //On.RoR2.UI.HUD.Update -= HUD_Update;
        //    On.RoR2.CharacterMaster.GiveMoney -= CharacterMaster_GiveMoney;

        //}

        //public void OnDestroy()
        //{

        //    Unhook();
        //}

    }
}