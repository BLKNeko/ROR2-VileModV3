using RoR2;
using RoR2.Projectile;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace VileMod.Survivors.Vile.Components
{
    public class OrbitAroundOwner : MonoBehaviour
    {
        public float radius = 5f;
        public float speed = 150f;
        public float duration = 40f;
        public float returnSpeed = 1f;
        public float destroyDistance = 0.5f;

        private float angle;
        private float age;
        private bool returning = false;
        private Transform ownerTransform;
        private ProjectileController projectileController;
        public CharacterBody ownerBody;

        void Start()
        {
            projectileController = GetComponent<ProjectileController>();

            ownerBody = projectileController.owner.GetComponent<CharacterBody>();

            speed += ownerBody.attackSpeed * 10f;

            //Debug.Log("ownerBody: " + ownerBody);

            if (projectileController && projectileController.owner)
            {
                ownerTransform = ownerBody.transform;
            }

            if (ownerTransform)
            {
                angle = 0f;
                Vector3 offset = new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;
                transform.position = ownerTransform.position + offset;
            }

        }

        void Update()
        {
            if (!ownerTransform) return;

            if (!returning)
            {
                // Fase de órbita
                angle += speed * Time.deltaTime;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
                transform.position = ownerTransform.position + offset;

                // Rotação fixa
                //transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                Vector3 direction = (ownerTransform.position - transform.position).normalized;
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
                if (flatDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 3f);
                }

                age += Time.deltaTime;
                if (age >= duration)
                {
                    returning = true;
                }
            }
            else
            {
                // Fase de retorno
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    ownerTransform.position,
                    returnSpeed * Time.deltaTime
                );

                // Rotação fixa
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                // Quando chegar perto o suficiente, destruir
                //if (Vector3.Distance(transform.position, ownerTransform.position) < destroyDistance)
                //{
                //    Destroy(gameObject);
                //}
            }
        }
    }

}