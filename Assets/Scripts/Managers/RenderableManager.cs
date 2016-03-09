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
using System.Collections;

public class RenderableManager : MonoBehaviour {

    public float MaxItems;
    public float MaxItemsLowCount = 1;
    public float HorizontalBounds;
    public float VerticalBounds;

    public GameObject[] PrefabsToSpawn;
    float NumItemsPerRow = 0;

    public float Spacing;
    ArrayList Renderables = new ArrayList();

    public bool IsLowCountScene = false;

	void Start () {
        if(IsLowCountScene)
            NumItemsPerRow = MaxItemsLowCount;
        else
            NumItemsPerRow = Mathf.Sqrt(MaxItems);
        Assert.AreNotEqual(0, NumItemsPerRow);

        int currentPrefabToSpawn = 0;
        Assert.AreNotEqual(0, PrefabsToSpawn.Length);

        Vector3 currentSpawnPos = Vector3.zero;

        for(int i = 0; i < NumItemsPerRow; i++)
        {
            for(int j = 0; j < NumItemsPerRow; j++)
            {
                currentSpawnPos.x = i * Spacing +
                    -(NumItemsPerRow / 2) * Spacing;
                currentSpawnPos.z = j * Spacing +
                    -(NumItemsPerRow / 2) * Spacing;
                var newObject = Instantiate(PrefabsToSpawn[currentPrefabToSpawn], currentSpawnPos, PrefabsToSpawn[currentPrefabToSpawn].transform.rotation) as GameObject;
                newObject.transform.SetParent(gameObject.transform);
                Renderables.Add(newObject);

                currentPrefabToSpawn++;
                if(currentPrefabToSpawn >= PrefabsToSpawn.Length)
                {
                    currentPrefabToSpawn = 0;
                }
            }
        }
	}
}
