using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Otumn.Juggling
{
    public class TouchManager : MonoBehaviour
    {
        [SerializeField] private bool editor = true;
        [SerializeField] private LayerMask touchMask;
        private ITouchable lastTouchedTouchable;
        private int lastTouchedIndex = 0;
        private Ray touchRay;
        private RaycastHit[] hits;

        private void Update()
        {
            TouchStateManager();
        }

        private void TouchStateManager()
        {
            if (editor) // mouse
            {
                if (Input.GetMouseButtonDown(0)) // mouse clicked
                {
                    SendRayCast(Input.mousePosition);
                    lastTouchedTouchable = GetClosestHit(hits);
                    if (lastTouchedTouchable != null) lastTouchedTouchable.OnTouchBegin(GetGameSpaceScreenPos(Input.mousePosition));
                }
                if (Input.GetMouseButton(0) && lastTouchedTouchable != null) // mouse held
                {
                    if (lastTouchedTouchable != null) lastTouchedTouchable.OnTouchHeld(GetGameSpaceScreenPos(Input.mousePosition));
                }
                if (Input.GetMouseButtonUp(0) && lastTouchedTouchable != null) // mouse released
                {
                    if (lastTouchedTouchable != null) lastTouchedTouchable.OnTouchReleased(GetGameSpaceScreenPos(Input.mousePosition));
                    lastTouchedTouchable = null;
                }
            }
            else // touch
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began) // touch began
                    {
                        SendRayCast(Input.GetTouch(0).position);
                        lastTouchedTouchable = GetClosestHit(hits);
                        if(lastTouchedTouchable != null) lastTouchedTouchable.OnTouchBegin(GetGameSpaceScreenPos(Input.GetTouch(0).position));
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary) // touch held
                    {
                        if (lastTouchedTouchable != null) lastTouchedTouchable.OnTouchHeld(GetGameSpaceScreenPos(Input.GetTouch(0).position));
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended) // touch released
                    {
                        if (lastTouchedTouchable != null) lastTouchedTouchable.OnTouchReleased(GetGameSpaceScreenPos(Input.GetTouch(0).position));
                    }
                }
            }
        }

        private ITouchable GetClosestHit(RaycastHit[] hits)
        {
            ITouchable chosenTouchable = null;
            float minimumLenght = Mathf.Infinity;

            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].collider.GetComponent<ITouchable>() != null)
                {
                    float testedDist = Vector3.Distance(hits[i].point, Camera.main.transform.position);
                    if (testedDist < minimumLenght)
                    {
                        minimumLenght = testedDist;
                        lastTouchedIndex = i;
                        chosenTouchable = hits[i].collider.GetComponent<ITouchable>();
                    }
                }
                else
                {
                    continue;
                }
            }

            return chosenTouchable;
        }

        private void SendRayCast(Vector3 screenPos)
        {
            touchRay = Camera.main.ScreenPointToRay(screenPos);
            hits = Physics.RaycastAll(touchRay.origin, touchRay.direction, 200, touchMask);
        }

        private Vector3 GetGameSpaceScreenPos(Vector3 screenPos)
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10));
            result = new Vector3(result.x, result.y, 0);
            return result;
        }
    }

    public interface ITouchable
    {
        void OnTouchBegin(Vector3 touchPos);
        void OnTouchHeld(Vector3 touchPos);
        void OnTouchReleased(Vector3 touchPos);
    }
}
