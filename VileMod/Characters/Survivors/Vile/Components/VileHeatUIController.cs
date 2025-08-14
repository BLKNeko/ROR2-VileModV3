using UnityEngine;
using UnityEngine.UI;
using RoR2;
using RoR2.UI;
using VileMod.Survivors.Vile.Components;
using System.Reflection;
using VileMod.Survivors.Vile;
using System;
using TMPro;

public class VileHeatUIController : MonoBehaviour
{
    public Image barFill;
    public Image barFillOH;
    public Image barFillEI;
    public Image barFillES;
    public Image barFillEF;
    public Image barFillB;
    public Image barFillR;
    public Image barFillRSH;
    public TextMeshProUGUI barTTInput;
    private VileComponent heatComp;
    private VileBoltComponent boltComp;
    private VileRideArmorComponent rideComp;
    private int maxSegments = 10;
    private HUD rorHUD;
    private Transform RoRHUDSpringCanvasTransform;
    private GameObject RoRHUDObject;
    private GameObject heatBarGO;
    public GameObject barR;
    private CharacterBody characterBody;

    public bool failedToInitialize;
    private bool isInitialized = false;
    public const int MAX_FAIL_ATTEMPTS = 50;
    public int failAttempts = 0;

    public void Start()
    {
        characterBody = GetComponent<CharacterBody>();
        heatComp = GetComponent<VileComponent>();
        boltComp = GetComponent<VileBoltComponent>();
        rideComp = GetComponent<VileRideArmorComponent>();

        Hook();

        try
        {
            InitializeUI();
        }
        catch (NullReferenceException e)
        {
            Debug.Log($"Vile - NRE on UI Initialization, trying again: {e}");
        }

       

    }

    public void InitializeUI()
    {
        if (!isInitialized)
        {
            InitializeRoRHUD();
            HeatBarUI();

            isInitialized = true;
        }
    }

    private void InitializeRoRHUD()
    {
        if (RoRHUDObject)
        {
            // Using Lee as a reference for HUD
            RoRHUDSpringCanvasTransform = RoRHUDObject.transform.Find("MainContainer").Find("MainUIArea").Find("SpringCanvas");

            rorHUD = RoRHUDObject.GetComponent<HUD>();

            return;
        }
        throw new NullReferenceException();
    }

    void HeatBarUI()
    {
        if (!RoRHUDObject) return;

        // Instancia a barra dentro do SpringCanvas (pode ser ajustado para outro local depois)
        heatBarGO = Instantiate(VileAssets.BarPanel, RoRHUDSpringCanvasTransform);
        heatBarGO.name = "VileHeatBar";

        // Resetar transform
        heatBarGO.transform.localRotation = Quaternion.identity;
        heatBarGO.transform.localScale = Vector3.one;

        // Ajustar posição usando RectTransform
        RectTransform rectTransform = heatBarGO.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 1f);  // Canto superior esquerdo
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);      // Pivot no canto superior esquerdo

        // Offset relativo ao canto superior esquerdo (ajuste fino)
        rectTransform.anchoredPosition = new Vector2(1300f, -680f); // X mais para direita, Y mais para baixo

        // Ajuste de tamanho, se necessário
        rectTransform.sizeDelta = new Vector2(160f, 20f); // largura x altura

        // Referência ao Image do preenchimento
        barFill = heatBarGO.transform.Find("Bar/Bar_Fill").GetComponent<Image>();
        barFillOH = heatBarGO.transform.Find("Bar/Bar_Fill_OH").GetComponent<Image>();
        barFillEI = heatBarGO.transform.Find("EBar/Bar_Fill_I").GetComponent<Image>();
        barFillES = heatBarGO.transform.Find("EBar/Bar_Fill_S").GetComponent<Image>();
        barFillEF = heatBarGO.transform.Find("EBar/Bar_Fill_F").GetComponent<Image>();
        barFillB = heatBarGO.transform.Find("BBar/Bar_Fill_B").GetComponent<Image>();
        barFillR = heatBarGO.transform.Find("RBar/Bar_Fill_R").GetComponent<Image>();
        barFillRSH = heatBarGO.transform.Find("RBar/Bar_Fill_RSH").GetComponent<Image>();
        barTTInput = heatBarGO.transform.Find("BBText/BBTInput").GetComponent<TextMeshProUGUI>();
        barTTInput.fontSize = 20;
        barTTInput.alignment = TextAlignmentOptions.Center;
        barTTInput.color = Color.white;


        barR = heatBarGO.transform.Find("RBar").gameObject;

        //Debug.Log("barFill: " + barFill);

    }

    void Update()
    {

        // Using Lee as a reference for HUD
        if (characterBody.hasEffectiveAuthority)
        {
            if (!isInitialized && !failedToInitialize)
            {
                try
                {
                    InitializeUI();
                }
                catch (NullReferenceException e)
                {
                    Debug.Log($"Vile - NRE on UI Initialization, trying again: {e}");
                    failAttempts++;

                    if (failAttempts >= MAX_FAIL_ATTEMPTS)
                    {
                        failedToInitialize = true;
                        Debug.Log($"Vile UI failed to initialize after more than {MAX_FAIL_ATTEMPTS} attempts. Stopping attempts to initialize.");
                    }
                }

                return;
            }

            //After that its supposed to be initialized buuut...

            if (isInitialized)
            {
                //Debug.Log("RoRHUDOBJ: " + RoRHUDObject);
                //Debug.Log("heatBarGO: " + heatBarGO);

                barFill.fillAmount = heatComp.GetBaseHeatValue();
                barFillOH.fillAmount = heatComp.GetBaseOverHeatValue();
                barFillEI.fillAmount = heatComp.GeticeElementValue();
                barFillES.fillAmount = heatComp.GetShockElementValue();
                barFillEF.fillAmount = heatComp.GetFlameElementValue();
                barFillB.fillAmount = boltComp.GetInverseLerpBoltValue();

                
                barTTInput.text = boltComp.GetBoltValue().ToString();

                if (characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff))
                {
                    barR.SetActive(true);
                    barFillR.fillAmount = rideComp.GetInverseLerpRHealthValue();
                    barFillRSH.fillAmount = rideComp.GetInverseLerpRShieldValue();
                }
                else
                {
                    barR.SetActive(false);
                }

                //Debug.Log("barFill.fillAmount: " + barFill.fillAmount);

            }

        }
    }


    public void Hook()
    {
        //On.RoR2.CameraRigController.Update += CameraRigController_Update;
        On.RoR2.UI.HUD.Update += HUD_Update;

    }

    public void Unhook()
    {
        //On.RoR2.CameraRigController.Update -= CameraRigController_Update;
        On.RoR2.UI.HUD.Update -= HUD_Update;

    }

    private void HUD_Update(On.RoR2.UI.HUD.orig_Update orig, HUD self)
    {
        orig(self);

        if (!RoRHUDObject)
        {
            RoRHUDObject = self.gameObject;
        }
    }

    public void OnDestroy()
    {
        //Destroy(canvasObject);
        Destroy(heatBarGO);
        Unhook();
    }

}
