/******************************************************************************
Copyright (C) 2016 Intel Corporation All Rights Reserved.
The source code, information and material ("Material") contained herein is
owned by Intel Corporation or its suppliers or licensors, and title to such
Material remains with Intel Corporation or its suppliers or licensors. The
Material contains proprietary information of Intel or its suppliers and
licensors. The Material is protected by worldwide copyright laws and treaty
provisions. No part of the Material may be used, copied, reproduced, modified,
published, uploaded, posted, transmitted, distributed or disclosed in any way
without Intel's prior express written permission. No license under any patent,
copyright or other intellectual property rights in the Material is granted to
or conferred upon you, either expressly, by implication, inducement, estoppel
or otherwise. Any license under such intellectual property rights must be
express and approved by Intel in writing.
Unless otherwise agreed by Intel in writing, you may not remove or alter this
notice or any other notice embedded in Materials by Intel or Intel's suppliers
or licensors in any way.
******************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.Types;

public class RenderingPathUI : MonoBehaviour {

    private Camera LoadedMainCamera = null;

    [HideInInspector]
    public GameObject DescriptionBox = null;
    Text DescriptionText = null;

    private const string s_RenderingBaseLabel = "Mode: ";
    enum RENDERING_TYPE {
        LEGACY_DEFERRED,
        DEFERRED,
        FORWARD,
        VERTEX_LIT,
        NUM_TYPES
    };

    class RenderingData {
        public string ButtonDisplayText;

        public static readonly string RenderPathLabel = "Mode: ";
        public static UIController.ButtonData ButtonData = null;
        public RenderingPath UnityRenderingPath;
        public string Description;
        public RenderingData(string displayText, RenderingPath unityRenderingPath)
        {
            ButtonDisplayText = displayText;
            UnityRenderingPath = unityRenderingPath;
        }
    };

    private Dictionary<RENDERING_TYPE, RenderingData> renderDict = new Dictionary<RENDERING_TYPE, RenderingData>();

    private RENDERING_TYPE CurrentRenderingMode = RENDERING_TYPE.FORWARD;
    

    // Necessary as tagged MainCamera exists in another scene
    void Awake()
    {
        var perfMgr = PerfSceneManager.GetInstance();
        perfMgr.LoadSceneAdditive(SCENE.TOWER_LOW_COUNT);
        perfMgr.LoadSceneAdditive(SCENE.UI);

        RenderingData deferredLightingInf = new RenderingData("Legacy Deferred", RenderingPath.DeferredLighting);
        deferredLightingInf.Description = "Legacy version of the deferred renderer.";
        renderDict[RENDERING_TYPE.LEGACY_DEFERRED] = deferredLightingInf;
        RenderingData deferredShadingRef = new RenderingData("Deferred", RenderingPath.DeferredShading);
        deferredShadingRef.Description = "Lighting performance unrelated to scene geometry complexity.  Trade heavy lighting computation for more memory usage leading to a higher chance of being memory bound.  Real-time shadows and per-pixel effects supported.  Semi-transparent rendering done in additional forwarded pass.  No support for anti-aliasing and Mesh Renderer's Receive Shadows flag.";
        renderDict[RENDERING_TYPE.DEFERRED] = deferredShadingRef;
        RenderingData forwardInf = new RenderingData("Forward", RenderingPath.Forward);
        forwardInf.Description = "Lighting done with a combination of per-pixel, per-vertex, and spherical harmonic techniques.  Real-time shadows and other per-pixel effects supported.  Does not incur memory cost required to build the g-buffer as in deferred.  Can lead to many draw calls covering the same pixel(s) if care isn't taken.";
        renderDict[RENDERING_TYPE.FORWARD] = forwardInf;
        RenderingData vertexLitInf = new RenderingData("Vertex Lit", RenderingPath.VertexLit);
        vertexLitInf.Description = "Does calculations per-vertex, and not per-pixel.  Can improve mobile performance.  Real-time shadows and other per-pixel effects not supported.  Low quality lighting.";
        renderDict[RENDERING_TYPE.VERTEX_LIT] = vertexLitInf;
    }

    void Start()
    {
        var CamObj = GameObject.FindGameObjectWithTag("MainCamera");
        Assert.AreNotEqual(null, CamObj);
        LoadedMainCamera = CamObj.GetComponent<Camera>();
        Assert.AreNotEqual(null, LoadedMainCamera);

        var canvasRoot = GameObject.FindGameObjectWithTag("UICanvasRoot");
        Assert.AreNotEqual(null, canvasRoot);
        var uiControllerScript = canvasRoot.GetComponent<UIController>();
        Assert.AreNotEqual(null, uiControllerScript);

        UIController.ButtonData renderQueueButton = uiControllerScript.GetAvailableButton();
        Assert.AreNotEqual(null, renderQueueButton);
        renderQueueButton.PointerEnteredButton += PointerEnterPathButton;
        renderQueueButton.PointerExitedButton += PointerExitPathButton;

        DescriptionBox = GameObject.FindGameObjectWithTag("DescriptionBox");
        DescriptionText = DescriptionBox.GetComponentInChildren<Text>();
        DescriptionText.text = "";
        DescriptionBox.SetActive(false);

        renderQueueButton.ButtonComponent.onClick.AddListener(() => SwitchRenderMode());


        RenderingData.ButtonData = renderQueueButton;
        renderQueueButton.SetText(RenderingData.RenderPathLabel + ": " + renderDict[CurrentRenderingMode].ButtonDisplayText);

        // Always start with forward rendering mode
        SetRenderingMode(CurrentRenderingMode);
    }

    void SetRenderingMode(RENDERING_TYPE type)
    {
        var renderInf = renderDict[type];
        Assert.AreNotEqual(null, renderInf);
        LoadedMainCamera.renderingPath = renderInf.UnityRenderingPath;
        RenderingData.ButtonData.SetText(RenderingData.RenderPathLabel + " " + renderDict[type].ButtonDisplayText);
        SetDescriptionBoxToCurrentRender();
    }

    void SwitchRenderMode()
    {
        CurrentRenderingMode = (RENDERING_TYPE) (((((int) CurrentRenderingMode) + 1)) % ((int)RENDERING_TYPE.NUM_TYPES));
        SetRenderingMode(CurrentRenderingMode);
    }

    void SetDescriptionBoxToCurrentRender()
    {
        if(!DescriptionBox.activeInHierarchy)
        {
            DescriptionBox.SetActive(true);
        }
        var renderInf = renderDict[CurrentRenderingMode];
        DescriptionText.text = renderInf.Description;
    }

    public void PointerEnterPathButton(object source, EventArgs e)
    {
        SetDescriptionBoxToCurrentRender();
    }

    public void PointerExitPathButton(object source, EventArgs e)
    {
        DescriptionBox.SetActive(false);
    }
}
