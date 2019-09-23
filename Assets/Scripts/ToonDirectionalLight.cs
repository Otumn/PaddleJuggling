using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToonDirectionalLight : MonoBehaviour
{
    [SerializeField][Range(-1, 1)] private float edge = 0.5f;
    [SerializeField][Range(0, 1)] private float smoothness = 0.5f;
    [SerializeField] private Color shadowColor;

    [SerializeField] private Material[] materials;

    private void Update()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_Edge", edge);
            materials[i].SetFloat("_Smoothness", smoothness);
            materials[i].SetVector("_WorldLight", -transform.forward);
            materials[i].SetColor("_ShadowColor", shadowColor);
        }
    }
}
