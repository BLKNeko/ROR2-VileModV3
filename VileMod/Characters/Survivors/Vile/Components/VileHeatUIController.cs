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
    public RawImage segmentImage;
    private VileComponent heatComp;
    private int maxSegments = 10;
    private HUD rorHUD;
    private Transform RoRHUDSpringCanvasTransform;
    private GameObject RoRHUDObject;
    private GameObject heatBarGO;
    private CharacterBody characterBody;

    public void Start()
    {
        characterBody = GetComponent<CharacterBody>();
        heatComp = GetComponent<VileComponent>();
        heatBarGO = VileAssets.BarPanel;

        Hook();


        InitializeRoRHUD();

    }

    //private void InitializeRoRHUD()
    //{
    //    if (RoRHUDObject)
    //    {
    //        // Using Lee as a reference for HUD
    //        RoRHUDSpringCanvasTransform = RoRHUDObject.transform.Find("MainContainer").Find("MainUIArea").Find("SpringCanvas");

    //        rorHUD = RoRHUDObject.GetComponent<HUD>();

    //        return;
    //    }
    //    throw new NullReferenceException();
    //}

    private void InitializeRoRHUD()
    {
        if (!heatBarGO || !RoRHUDObject) return;

        Transform springCanvas = RoRHUDObject.transform.Find("MainContainer/MainUIArea/SpringCanvas");
        if (!springCanvas)
        {
            Debug.LogWarning("SpringCanvas not found in HUD.");
            return;
        }

        GameObject instance = GameObject.Instantiate(heatBarGO, springCanvas);
        segmentImage = instance.GetComponentInChildren<RawImage>();

        if (!segmentImage)
        {
            Debug.LogWarning("segmentImage (RawImage) not found in HeatBar prefab.");
        }

        Debug.Log("VileHeatUI: Bar added to HUD.");
    }

    void Update()
    {
        if (!heatComp || !segmentImage) return;

        float heat = heatComp.GetBaseHeatValue();
        int segments = Mathf.RoundToInt(heat * maxSegments);
        segmentImage.uvRect = new Rect(0f, 0f, segments, 1f);
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
