using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class VirtualCameraShakeTarget : Entity
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.state.RegisterShakeTarget(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.state.UnregisterShakeTarget(this);
        }
    }
}
