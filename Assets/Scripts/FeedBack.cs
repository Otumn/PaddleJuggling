using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class FeedBack : MonoBehaviour
    {
        [SerializeField] private bool playOnStart = false;
        [SerializeField] private bool looping = false;
        [SerializeField] private bool hardFollowingTransform;
        [SerializeField] private Transform transformToHardFollow;
        [SerializeField] private bool tpOnTransformOnPlay;
        [SerializeField] private Transform transformToTPToOnPlay;
        //Particles
        [SerializeField] private bool particles = false;
        [SerializeField] private bool particleRotation = false;
        [SerializeField] private ParticleSystem[] particlesSystems;
        [SerializeField] private ParticleSystem[] particleSystemsToRotate;
        //Trail
        [SerializeField] private bool trails = false;
        [SerializeField] private TrailRenderer[] trailRenderers;
        //shake
        [SerializeField] private bool shake = false;
        [SerializeField] private bool useSpecificTransform = false;
        [SerializeField] private Transform[] specificTransformToShake;
        [SerializeField] private float shakeAmplitude = 0.2f;
        [SerializeField] private float shakeTime = 0.1f;
        // slow mo
        [SerializeField] private bool slowMo = false;
        [SerializeField] private float targetTimeScale = 0.05f;
        [SerializeField] private float slowMoSpeed = 0.1f;
        [SerializeField] private AnimationCurve slowMoInCurve;
        [SerializeField] private AnimationCurve slowMoOutCurve;
        //zoom
        [Tooltip("The zoom will be applied on the position of this gameobject, therefore it's often better to use this option with 'tpOnTranformOnPlay' or 'hardFollowingTranform'.")]
        [SerializeField] private bool zoom = false;
        [SerializeField] private AnimationCurve zoomInCurve;
        [SerializeField] private AnimationCurve zoomOutCurve;
        [SerializeField] private float zoomedDist = 4f;
        [SerializeField] private float zoomSpeed = 0.1f;
        //rumble
        [SerializeField] private bool rumble = false;
        [SerializeField] private long rumbleTime = 5;

        private Transform[] usedShakeTransform;
        private Vector3[] initialZoomTargetsPos;
        private Vector3 shakePosTarget;
        private bool isShaking = false;
        private bool isRumbling = false;
        private bool isZoomingIn = false;
        private bool isZoomingOut = false;
        private bool isSlowMoIn = false;
        private bool isSlowMoOut = false;
        private float shakeTimerDecrementer = 1f;
        private float shakePosDecrementer = 0.90f;
        private float zoomIncrementer = 0f;
        private float currentTimeScale = 1f;
        private float slowMoIncrementer = 0f;

        private void Start()
        {
            if (playOnStart) Play();
            else Stop();
        }

        private void Update()
        {
            ShakeManagement();
            ZoomManagement();
            PositionManagement();
            SlowMoManagement();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            if(tpOnTransformOnPlay)
            {
                transform.position = transformToTPToOnPlay.position;
            }

            if(particles)
            {
                for (int i = 0; i < particlesSystems.Length; i++)
                {
                    particlesSystems[i].Play();
                }
            }

            if(trails)
            {
                for (int i = 0; i < trailRenderers.Length; i++)
                {
                    trailRenderers[i].enabled = true;
                }
            }

            if(shake)
            {
                if(useSpecificTransform)
                {
                    usedShakeTransform = specificTransformToShake;
                }
                else
                {
                    usedShakeTransform = new Transform[GameManager.state.ShakeTargets.Count];
                    for (int i = 0; i < usedShakeTransform.Length; i++)
                    {
                        usedShakeTransform[i] = GameManager.state.ShakeTargets[i].transform;
                        Debug.Log(usedShakeTransform[i].gameObject.name);
                    }
                }
                shakePosTarget = Vector3.zero;
                shakeTimerDecrementer = shakeTime;
                isShaking = true;
            }

            if(slowMo)
            {
                isSlowMoIn = true;
            }

            if(zoom)
            {
                initialZoomTargetsPos = new Vector3[GameManager.state.ZoomTargets.Count];
                for (int i = 0; i < initialZoomTargetsPos.Length; i++)
                {
                    initialZoomTargetsPos[i] = GameManager.state.ZoomTargets[i].transform.position;
                }
                isZoomingIn = true;
            }

            if(rumble)
            {
                if(looping)
                {
                    isRumbling = true;
                }
                else
                {
                    Vibration.Vibrate(rumbleTime);
                }
            }
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            if(looping)
            {
                if (particles)
                {
                    for (int i = 0; i < particlesSystems.Length; i++)
                    {
                        particlesSystems[i].Stop();
                    }
                }
                if (trails)
                {
                    for (int i = 0; i < trailRenderers.Length; i++)
                    {
                        trailRenderers[i].enabled = false;
                    }
                }
                if (shake)
                {
                    shakeTimerDecrementer = shakeTime;
                    isShaking = false;
                }
                if(slowMo)
                {
                    isSlowMoIn = false;
                    isSlowMoOut = true;
                }
                if(zoom)
                {
                    isZoomingOut = true;
                    isZoomingIn = false;
                }
                if(rumble)
                {
                    isRumbling = false;
                }
            }
        }

        #region Effects management

        private void ShakeManagement()
        {
            if (isShaking)
            {
                if(shakeTimerDecrementer > 0)
                {
                    if(!looping)
                    {
                        shakeTimerDecrementer -= Time.deltaTime;
                    }

                    shakePosDecrementer -= Time.timeScale;

                    if (shakePosDecrementer < 0)
                    {
                        shakePosDecrementer = 0.90f;
                        shakePosTarget = (Random.insideUnitCircle * shakeAmplitude * Mathf.InverseLerp(0, shakeTime, shakeTimerDecrementer) * ((Time.timeScale * 0.9f) + 0.1f));
                    }
                    for (int i = 0; i < usedShakeTransform.Length; i++)
                    {
                        usedShakeTransform[i].localPosition = Vector3.Lerp(usedShakeTransform[i].localPosition, new Vector3(shakePosTarget.x, shakePosTarget.y, usedShakeTransform[i].localPosition.z), 1 - shakePosDecrementer);
                    }
                }
                else
                {
                    shakeTimerDecrementer = shakeTime;
                    isShaking = false;
                }
            }
        }

        private void ZoomManagement()
        {
            if(isZoomingIn)
            {
                if(zoomIncrementer < 1f)
                {
                    if (slowMo)
                    {
                        zoomIncrementer += Time.deltaTime * (zoomSpeed / currentTimeScale);
                    }
                    else
                    {
                        zoomIncrementer += Time.deltaTime * zoomSpeed;
                    }

                    float newZ = Mathf.Lerp(0, zoomedDist, zoomInCurve.Evaluate(zoomIncrementer));
                    for (int i = 0; i < GameManager.state.ZoomTargets.Count; i++)
                    {
                        float newX = Mathf.Lerp(initialZoomTargetsPos[i].x, transform.position.x, zoomInCurve.Evaluate(zoomIncrementer));
                        float newY = Mathf.Lerp(initialZoomTargetsPos[i].y, transform.position.y, zoomInCurve.Evaluate(zoomIncrementer));
                        GameManager.state.ZoomTargets[i].transform.position = new Vector3(newX, newY, newZ);
                    }
                }
                else
                {
                    if(!looping)
                    {
                        zoomIncrementer = 0f;
                        isZoomingOut = true;
                        isZoomingIn = false;
                    }
                    else
                    {
                        isZoomingIn = false;
                        zoomIncrementer = 0f;
                    }
                }
            }
            else if(isZoomingOut)
            {
                if (zoomIncrementer < 1)
                {
                    if(slowMo)
                    {
                        zoomIncrementer += Time.deltaTime * (zoomSpeed / currentTimeScale);
                    }
                    else
                    {
                        zoomIncrementer += Time.deltaTime * zoomSpeed;
                    }

                    float newZ = Mathf.Lerp(0, zoomedDist, zoomOutCurve.Evaluate(zoomIncrementer));
                    for (int i = 0; i < GameManager.state.ZoomTargets.Count; i++)
                    {
                        float newX = Mathf.Lerp(initialZoomTargetsPos[i].x, transform.position.x, zoomOutCurve.Evaluate(zoomIncrementer));
                        float newY = Mathf.Lerp(initialZoomTargetsPos[i].y, transform.position.y, zoomOutCurve.Evaluate(zoomIncrementer));
                        GameManager.state.ZoomTargets[i].transform.position = new Vector3(newX, newY, newZ);
                    }
                }
                else
                {
                    zoomIncrementer = 0f;
                    isZoomingOut = false;
                    isZoomingIn = false;
                }
            }
        }
        
        private void VibrationManager()
        {
            if(isRumbling && looping)
            {
                Vibration.Vibrate(50);
            }
        }

        private void PositionManagement()
        {
            if(hardFollowingTransform)
            {
                transform.position = transformToHardFollow.position;
            }
        }

        private void SlowMoManagement()
        {
            if(isSlowMoIn)
            {
                if(slowMoIncrementer < 1)
                {
                    slowMoIncrementer += Time.deltaTime * slowMoSpeed;
                    // 1 = target
                    // 0 = normal scale
                    currentTimeScale = Mathf.Lerp(1, targetTimeScale, slowMoInCurve.Evaluate(slowMoIncrementer));
                    Time.timeScale = currentTimeScale;
                    Time.fixedDeltaTime = 0.02f * currentTimeScale;
                }
                else
                {
                    if(!looping)
                    {
                        slowMoIncrementer = 0f;
                        isSlowMoIn = false;
                        isSlowMoOut = true;
                    }
                    else
                    {
                        slowMoIncrementer = 0f;
                        isSlowMoIn = false;
                    }
                }
                Debug.Log(currentTimeScale);
            }
            if(isSlowMoOut)
            {
                if (slowMoIncrementer < 1)
                {
                    slowMoIncrementer += Time.deltaTime * slowMoSpeed;
                    currentTimeScale = Mathf.Lerp(1, targetTimeScale, slowMoOutCurve.Evaluate(slowMoIncrementer));
                    Time.timeScale = currentTimeScale;
                    Time.fixedDeltaTime = 0.02f * currentTimeScale;
                }
                else
                {
                    slowMoIncrementer = 0f;
                    isSlowMoOut = false;
                    isSlowMoIn = false;
                }
            }
        }

        #endregion

        #region Rotation

        public void RotateShapeEmitter(float newRotX)
        {
            for (int i = 0; i < particleSystemsToRotate.Length ; i++)
            {
                var shapeModule = particleSystemsToRotate[i].shape;
                shapeModule.rotation = new Vector3(newRotX,-90f,0);
            }
        }

        public void Rotate3DStartRotationZ(float newRotZ)
        {
            for (int i = 0; i < particleSystemsToRotate.Length; i++)
            {
                ParticleSystem.MainModule module = particleSystemsToRotate[i].main;
                module.startRotationZ = newRotZ * Mathf.Deg2Rad;
            }
        }

        public void Rotate3DStartRotationX(float newRotX)
        {
            for (int i = 0; i < particleSystemsToRotate.Length; i++)
            {
                ParticleSystem.MainModule module = particleSystemsToRotate[i].main;
                module.startRotationX = newRotX * Mathf.Deg2Rad;
            }
        }

        #endregion

        public bool Particles { get => particles; }
        public bool ParticleRotation { get => particleRotation; }
        public bool Trails { get => trails; }
        public bool Shake { get => shake; }
        public bool UseSpecificTransform { get => useSpecificTransform; }
        public bool HardFollowingTransform { get => hardFollowingTransform; }
        public bool TpOnTransformOnPlay { get => tpOnTransformOnPlay; }
        public ParticleSystem[] ParticlesSystems { get => particlesSystems; set => particlesSystems = value; }
        public ParticleSystem[] ParticleSystemsToRotate { get => particleSystemsToRotate; set => particleSystemsToRotate = value; }
        public TrailRenderer[] TrailRenderers { get => trailRenderers; set => trailRenderers = value; }
        public bool Rumble { get => rumble; }
        public bool Zoom { get => zoom; }
        public bool SlowMo { get => slowMo; set => slowMo = value; }
    }
}
