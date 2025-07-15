using UnityEngine;
using UnityEngine.UI;
using RoR2;
using RoR2.UI;
using VileMod.Survivors.Vile.Components;
using System.Reflection;
using VileMod.Survivors.Vile;
using System;

public class VileHeatUIController : MonoBehaviour
{
    public Image barFill;
    private VileComponent heatComp;
    private int maxSegments = 10;
    private HUD rorHUD;
    private Transform RoRHUDSpringCanvasTransform;
    private GameObject RoRHUDObject;
    private GameObject heatBarGO;
    private CharacterBody characterBody;

    public bool failedToInitialize;
    private bool isInitialized = false;
    public const int MAX_FAIL_ATTEMPTS = 50;
    public int failAttempts = 0;

    public void Start()
    {
        characterBody = GetComponent<CharacterBody>();
        heatComp = GetComponent<VileComponent>();

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

        heatBarGO = Instantiate(VileAssets.BarPanel, RoRHUDSpringCanvasTransform.Find("BottomLeftCluster/BarRoots/Healthbar"));
        heatBarGO.transform.rotation = Quaternion.identity;
        heatBarGO.transform.localScale = new Vector3(0.7891f, 0.4f, 1f);
        heatBarGO.transform.position = new Vector3(-9.7021f, -4.8843f, 12.6537f);
        heatBarGO.transform.localPosition = new Vector3(2.3027f, 0.7998f, 0.0003f);

        barFill = heatBarGO.transform.Find("Bar/Bar_Fill").GetComponent<Image>();

        Debug.Log("barFill: " + barFill);

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

                Debug.Log("barFill.fillAmount: " + barFill.fillAmount);

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
