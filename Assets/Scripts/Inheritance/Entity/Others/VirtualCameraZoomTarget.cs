using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class VirtualCameraZoomTarget : Entity
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.state.RegisterZoomTarget(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.state.UnregisterZoomTarget(this);
        }
    }
}
