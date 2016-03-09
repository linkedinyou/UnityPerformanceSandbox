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
using UnityEngine.Assertions;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.Types;

public class RenderQueueController : MonoBehaviour
{

    public MeshRenderer FloorDownRenderer = null;
    public MeshRenderer FloorUpRenderer = null;
    public MeshRenderer LeftCubeRenderer = null;
    public MeshRenderer RightCubeRenderer = null;

    [HideInInspector]
    public GameObject DescriptionBox = null;
    Text DescriptionText = null;

    enum OBJECT
    {
        FLOOR_UP,
        FLOOR_DOWN,
        CUBE_LEFT,
        CUBE_RIGHT
    }

    Dictionary<OBJECT, int> OriginalRenderQueueNumbers = new Dictionary<OBJECT, int>();

    enum RENDER_QUEUE_STATE
    {
        DEFAULT_ORDERING,
        SMART_ORDERING,
        MAX_STATES
    }

    RENDER_QUEUE_STATE CurrentRenderQueueState = RENDER_QUEUE_STATE.DEFAULT_ORDERING;

    class RenderQueueData
    {
        public string ButtonDisplayText;
        public static string Description;
        public static readonly string RenderQueueLabel = "Render Sorting: ";
        public static UIController.ButtonData ButtonData = null;

        public RenderQueueData(string displayText)
        {
            ButtonDisplayText = displayText;
        }
    };

    Dictionary<RENDER_QUEUE_STATE, RenderQueueData> renderDict = new Dictionary<RENDER_QUEUE_STATE, RenderQueueData>();

    void Awake()
    {
        var perfMgr = PerfSceneManager.GetInstance();
        //perfMgr.LoadScene(PerfSceneManager.SCENE.BUILDINGS);
        perfMgr.LoadSceneAdditive(SCENE.UI);
    }

	// Use this for initialization
    void Start()
    {
        var canvasRoot = GameObject.FindGameObjectWithTag("UICanvasRoot");
        Assert.AreNotEqual(null, canvasRoot);
        var uiControllerScript = canvasRoot.GetComponent<UIController>();
        Assert.AreNotEqual(null, uiControllerScript);

        var renderQueueButton = uiControllerScript.GetAvailableButton();
        Assert.AreNotEqual(null, renderQueueButton);
        renderQueueButton.PointerEnteredButton += PointerEnterCascadeButton;
        renderQueueButton.PointerExitedButton += PointerExitCascadeButton;

        // Maybe make a script for description box that contains Text field + accessor to string so I can search on canvas root.
        DescriptionBox = GameObject.FindGameObjectWithTag("DescriptionBox");
        DescriptionText = DescriptionBox.GetComponentInChildren<Text>();
        DescriptionText.text = "";
        DescriptionBox.SetActive(false);

        renderQueueButton.ButtonComponent.onClick.AddListener(() => RenderQueueClicked());

        // Build Dictionaries
        renderDict[RENDER_QUEUE_STATE.DEFAULT_ORDERING] = new RenderQueueData("Default");
        renderDict[RENDER_QUEUE_STATE.SMART_ORDERING] = new RenderQueueData("Smart Ordering");

        RenderQueueData.ButtonData = renderQueueButton;
        RenderQueueData.Description = "Toggles between the default ordering mode and the manual ordering mode.  Default will do Z ordering based on the central point of a mesh.  Sometimes developers will use a plane as their floor and scale it up.  This can lead to a lot of overdrawn pixels.";
        renderQueueButton.SetText(RenderQueueData.RenderQueueLabel + renderDict[RENDER_QUEUE_STATE.DEFAULT_ORDERING].ButtonDisplayText);

        OriginalRenderQueueNumbers[OBJECT.FLOOR_UP] = FloorUpRenderer.material.renderQueue;
        OriginalRenderQueueNumbers[OBJECT.FLOOR_DOWN] = FloorDownRenderer.material.renderQueue;
        OriginalRenderQueueNumbers[OBJECT.CUBE_LEFT] = RightCubeRenderer.material.renderQueue;
        OriginalRenderQueueNumbers[OBJECT.CUBE_RIGHT] = LeftCubeRenderer.material.renderQueue;
    }

    void RenderQueueClicked()
    {
        CurrentRenderQueueState = (RENDER_QUEUE_STATE)(((((int)CurrentRenderQueueState) + 1)) % ((int)RENDER_QUEUE_STATE.MAX_STATES));
        if(CurrentRenderQueueState == RENDER_QUEUE_STATE.DEFAULT_ORDERING)
        {
            // default ordering algo
            // 1) On load, get the renderqueue place of each renderer in scene.
            // 2) On switch back, set the renderqueue of everything to be what initial value was

            FloorUpRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.FLOOR_UP];
            FloorDownRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.FLOOR_DOWN];
            RightCubeRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.CUBE_LEFT];
            LeftCubeRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.CUBE_RIGHT];
        }
        else if(CurrentRenderQueueState == RENDER_QUEUE_STATE.SMART_ORDERING)
        {
            // smart ordering algo
            RightCubeRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.CUBE_LEFT] + 0;
            LeftCubeRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.CUBE_RIGHT] + 1;
            FloorUpRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.FLOOR_UP] + 2;
            FloorDownRenderer.material.renderQueue = OriginalRenderQueueNumbers[OBJECT.FLOOR_DOWN] + 3;
        }
        RenderQueueData.ButtonData.SetText(RenderQueueData.RenderQueueLabel + renderDict[CurrentRenderQueueState].ButtonDisplayText);
    }

    public void PointerEnterCascadeButton(object source, EventArgs e)
    {
        DescriptionBox.SetActive(true);
        DescriptionText.text = RenderQueueData.Description;
    }

    public void PointerExitCascadeButton(object source, EventArgs e)
    {
        DescriptionBox.SetActive(false);
    }
}