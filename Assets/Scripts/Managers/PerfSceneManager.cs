﻿/******************************************************************************
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
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts.Types;

public class PerfSceneManager
{
    private static PerfSceneManager ManagerSingleton = null;
    private Dictionary<SCENE, string> SceneToNameDict = new Dictionary<SCENE, string>();

    public static PerfSceneManager GetInstance()
    {
        if (ManagerSingleton == null)
        {
            ManagerSingleton = new PerfSceneManager();
        }
        return ManagerSingleton;
    }

    public PerfSceneManager()
    {
        SceneToNameDict[SCENE.UI] = "__UI_Cam_Scene";
        SceneToNameDict[SCENE.TOWER] = "__TowerScene";
        SceneToNameDict[SCENE.TOWER_LOW_COUNT] = "__TowerScene_LowModelsCount";
        SceneToNameDict[SCENE.RENDER_PATHS] = "__RenderPathScene";
        SceneToNameDict[SCENE.RENDERQUEUE] = "__RenderQueueScene";
        SceneToNameDict[SCENE.SHADOW_SCENE] = "__ShadowScene";
        SceneToNameDict[SCENE.TRANSPARENCY_SCENE] = "__TransparentTextureScene";
        SceneToNameDict[SCENE.MULTI_CAM] = "__MultiCamScene";
        SceneToNameDict[SCENE.LIGHTMAP_PROBE_SCENE] = "__LightMapProbeScene";
        SceneToNameDict[SCENE.SCENE_INDEX] = "__SceneIndex";
        SceneToNameDict[SCENE.HIGH_RES_TEX_SCENE] = "__HighResTextureScene";
        SceneToNameDict[SCENE.EXTRA_FORWARD_PASS] = "__ForwardDraws";
        SceneToNameDict[SCENE.SHADER_VIEWING] = "__ShaderViewing";
    }

    public void LoadScene(SCENE sceneID)
    {
        SceneManager.LoadScene(SceneToNameDict[sceneID], LoadSceneMode.Single);
    }

    public void LoadSceneAdditive(SCENE sceneID)
    {
        SceneManager.LoadScene(SceneToNameDict[sceneID], LoadSceneMode.Additive);
    }
}
