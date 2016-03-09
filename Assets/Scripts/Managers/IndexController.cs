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
using Assets.Scripts.Types;

public class IndexController : MonoBehaviour {

    public Button RenderPathButton = null;
    public Button RenderQueueButton = null;
    public Button ShadowSceneButton = null;
    public Button MultipleCamerasButton = null;
    public Button TransparencyDisableButton = null;
    public Button LightMapProbeButton = null;
    public Button HighResTexButton = null;
    public Button ExtraForwardPassButton = null;
    public Button SimpleShader = null;

    PerfSceneManager PerformanceSceneManager = null;

	void Start ()
    {
        RenderPathButton.onClick.AddListener(() => ClickedRenderPathButton());
        RenderQueueButton.onClick.AddListener(() => ClickedRenderQueueButton());
        ShadowSceneButton.onClick.AddListener(() => ClickedShadowButton());
        MultipleCamerasButton.onClick.AddListener(() => ClickedMultiCamButton());
        TransparencyDisableButton.onClick.AddListener(() => ClickedTransparencyButton());
        LightMapProbeButton.onClick.AddListener(() => ClickedLightMapProbeButton());
        HighResTexButton.onClick.AddListener(() => ClickedHighResTexButton());
        ExtraForwardPassButton.onClick.AddListener(() => ClickedExtraForwardPassButton());
        SimpleShader.onClick.AddListener(() => ClickedSimpleShader());
        PerformanceSceneManager = PerfSceneManager.GetInstance();
    }

    void ClickedRenderPathButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.RENDER_PATHS);
    }

    void ClickedRenderQueueButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.RENDERQUEUE);
    }

    void ClickedShadowButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.SHADOW_SCENE);
    }

    void ClickedMultiCamButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.MULTI_CAM);
    }

    void ClickedTransparencyButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.TRANSPARENCY_SCENE);
    }

    void ClickedLightMapProbeButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.LIGHTMAP_PROBE_SCENE);
    }
    void ClickedHighResTexButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.HIGH_RES_TEX_SCENE);
    }

    void ClickedExtraForwardPassButton()
    {
        PerformanceSceneManager.LoadScene(SCENE.EXTRA_FORWARD_PASS);
    }
    void ClickedSimpleShader()
    {
        PerformanceSceneManager.LoadScene(SCENE.SHADER_VIEWING);
    }
}
