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
using System.Collections;

namespace Assets.Scripts.Managers
{
    public class CameraMovement : MonoBehaviour
    {

        public Transform CameraLookAtTarget;
        public float MovementSpeed = 1.0f;

        public enum MOVEMENT
        {
            STATIC,
            RAILS,
            STATE_COUNT
        }

        public MOVEMENT CurrentMovement = MOVEMENT.STATIC;

        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
        {
            return angle * (point - pivot) + pivot;
        }

        void Update()
        {
            if (CurrentMovement == MOVEMENT.RAILS)
            {
                transform.position = RotatePointAroundPivot(transform.position,
                                   CameraLookAtTarget.position,
                                   Quaternion.Euler(0, MovementSpeed * Time.deltaTime, 0));
            }
            transform.LookAt(CameraLookAtTarget);
        }
    }
}
