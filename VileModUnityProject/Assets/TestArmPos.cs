using UnityEngine;
using RoR2;

public class TestArmPos : MonoBehaviour
{
    public Transform clavicleBone; // Bip R Clavicle
    public float smoothSpeed = 15f; // quanto maior, mais rápido se ajusta
    public Vector3 localEulerOffset; // compensação se o bone estiver girado por padrão
    public Vector3 targetPoint;

    private Ray aimRay;

    private Quaternion initialLocalRotation;

    void Awake()
    {

        if (clavicleBone)
        {
            initialLocalRotation = clavicleBone.localRotation;
        }
    }

    void LateUpdate()
    {
        if (clavicleBone == null)return;

        // Só aplica durante a skill primária ativa
        // Você precisa adaptar esse check pro seu estado/skill. Exemplo genérico:
        //bool primaryActive = /* sua condição: ex: currentSkill == primarySkill && está sendo usada */ true;
        //if (!primaryActive)
        //{
        //    // opcional: voltar suavemente à rotação original
        //    clavicleBone.localRotation = Quaternion.Slerp(clavicleBone.localRotation, initialLocalRotation, Time.deltaTime * smoothSpeed);
        //    return;
        //}

        // Pega a direção da mira
        //Vector3 aimOrigin = body.aimOrigin; // ponto de origem da mira
        //Vector3 aimDirection = inputBank.aimDirection; // direção da mira em world space

        //if (aimDirection.sqrMagnitude < 0.0001f) return;

        //aimRay = new Ray(aimOrigin, aimDirection);

        // Queremos que a clavícula "olhe" para frente da mira, projetando um alvo à frente
        //Vector3 targetPoint = aimRay.GetPoint(10f); // 10 unidades à frente da mira, ajuste se quiser

        // Calcula rotação desejada: fazer o eixo da clavícula apontar na direção da mira.
        // Dependendo da orientação do seu rig, pode precisar ajustar o eixo.
        Vector3 toTarget = targetPoint - clavicleBone.position;
        if (toTarget.sqrMagnitude < 0.0001f) return;

        Quaternion desiredWorldRot = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
        // aplica offset local se necessário
        desiredWorldRot *= Quaternion.Euler(localEulerOffset);

        // Suaviza e aplica
        clavicleBone.rotation = Quaternion.Slerp(clavicleBone.rotation, desiredWorldRot, Time.deltaTime * smoothSpeed);
    }
}
