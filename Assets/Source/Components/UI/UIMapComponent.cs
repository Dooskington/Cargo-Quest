﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMapComponent : MonoBehaviour, IPointerClickHandler
{
    public MapDataComponent mapData;
    public float mapScale = 5.0f;

    public GameObject player;
    public RectTransform mapRect;
    public GameObject blipPrefab;
    public GameObject textPrefab;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(mapRect, eventData.position))
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.position, null, out localPos);
        }
    }

    private void Awake()
    {
        mapData = FindObjectOfType<MapDataComponent>();
        if (!mapData)
        {
            Debug.LogError(name + ": No MapData was provided!");
        }
    }

    private void Update()
    {
        UpdateBlips();
    }

    private void UpdateBlips()
    {
        foreach (MapBlipComponent blip in mapData.blipList)
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, blip.transform.position) * mapScale;

            if (blip.BlipUIImage == null)
            {
                CreateUIBlip(blip);
            }

            if ((blip.BlipUIText == null) && (blip.blipName.Length > 0))
            {
                CreateUIBlipText(blip);
            }

            if (blip.BlipUIText)
            {
                blip.BlipUIText.rectTransform.anchoredPosition = screenPoint + new Vector2(0.0f, 6.0f);
            }

            blip.BlipUIImage.rectTransform.anchoredPosition = screenPoint;

            if (blip.rotate)
            {
                blip.BlipUIImage.rectTransform.rotation = blip.transform.rotation;
            }

            if (blip.isPlayer)
            {
                blip.BlipUIImage.transform.SetAsLastSibling();
            }
        }
    }

    private void CreateUIBlip(MapBlipComponent blip)
    {
        GameObject blipObject = Instantiate(blipPrefab, transform) as GameObject;
        blipObject.name = blip.name;
        blip.BlipUIImage = blipObject.GetComponent<Image>();
        blip.BlipUIImage.sprite = blip.blipSprite;
        blip.BlipUIImage.rectTransform.pivot = blip.blipPivot;
        blip.BlipUIImage.rectTransform.localScale = new Vector3(1.0f * blip.blipScale, 1.0f * blip.blipScale , 1.0f);
    }

    private void CreateUIBlipText(MapBlipComponent blip)
    {
        GameObject textObject = Instantiate(textPrefab, transform.position, Quaternion.identity) as GameObject;
        textObject.transform.SetParent(transform, false);
        textObject.name = blip.name + "_text";
        blip.BlipUIText = textObject.GetComponent<Text>();
        blip.BlipUIText.text = blip.blipName;
    }
}

