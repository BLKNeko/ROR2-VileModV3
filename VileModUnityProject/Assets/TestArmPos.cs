using UnityEngine;
using RoR2;

public class TestArmPos : MonoBehaviour
{
    public Transform clavicleBone; // Bip R Clavicle
    public float smoothSpeed = 15f; // quanto maior, mais r�pido se ajusta
    public Vector3 localEulerOffset; // compensa��o se o bone estiver girado por padr�o
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

        // S� aplica durante a skill prim�ria ativa
        // Voc� precisa adaptar esse check pro seu estado/skill. Exemplo gen�rico:
        //bool primaryActive = /* sua condi��o: ex: currentSkill == primarySkill && est� sendo usada */ true;
        //if (!primaryActive)
        //{
        //    // opcional: voltar suavemente � rota��o original
        //    clavicleBone.localRotation = Quaternion.Slerp(clavicleBone.localRotation, initialLocalRotation, Time.deltaTime * smoothSpeed);
        //    return;
        //}

        // Pega a dire��o da mira
        //Vector3 aimOrigin = body.aimOrigin; // ponto de origem da mira
        //Vector3 aimDirection = inputBank.aimDirection; // dire��o da mira em world space

        //if (aimDirection.sqrMagnitude < 0.0001f) return;

        //aimRay = new Ray(aimOrigin, aimDirection);

        // Queremos que a clav�cula "olhe" para frente da mira, projetando um alvo � frente
        //Vector3 targetPoint = aimRay.GetPoint(10f); // 10 unidades � frente da mira, ajuste se quiser

        // Calcula rota��o desejada: fazer o eixo da clav�cula apontar na dire��o da mira.
        // Dependendo da orienta��o do seu rig, pode precisar ajustar o eixo.
        Vector3 toTarget = targetPoint - clavicleBone.position;
        if (toTarget.sqrMagnitude < 0.0001f) return;

        Quaternion desiredWorldRot = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
        // aplica offset local se necess�rio
        desiredWorldRot *= Quaternion.Euler(localEulerOffset);

        // Suaviza e aplica
        clavicleBone.rotation = Quaternion.Slerp(clavicleBone.rotation, desiredWorldRot, Time.deltaTime * smoothSpeed);
    }
}
