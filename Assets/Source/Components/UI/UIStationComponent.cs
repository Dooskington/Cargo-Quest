﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStationComponent : MonoBehaviour
{
    public Text title;
    public Button rechargeButton;
    public GameObject rechargeCostObject;
    public Text rechargeCostText;
    public Button repairButton;
    public GameObject repairCostObject;
    public Text repairCostText;

    public AudioEvent buySuccessAudio;
    public AudioEvent buyFailureAudio;

    private int rechargeCost;
    private int repairCost;

    private StationControllerComponent station;
    private PlayerControllerComponent player;
    private ShipReactorComponent shipReactor;
    private ShipDefenseComponent shipDefense;

    public void Open(StationControllerComponent stationControllerComponent)
    {
        station = stationControllerComponent;

        player = FindObjectOfType<PlayerControllerComponent>();
        shipReactor = player.GetComponent<ShipReactorComponent>();
        shipDefense = player.GetComponent<ShipDefenseComponent>();

        ConstructUI();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnRechargeClick()
    {
        if (!player.TakeCredits(rechargeCost))
        {
            buyFailureAudio.Play();
            return;
        }

        buySuccessAudio.Play();
        player.Recharge();

        ConstructUI();
    }

    public void OnRepairClick()
    {
        if (!player.TakeCredits(repairCost))
        {
            buyFailureAudio.Play();
            return;
        }

        buySuccessAudio.Play();
        shipDefense.Repair();

        ConstructUI();
    }

    private void ConstructUI()
    {
        //title.text = station.StationName;

        rechargeCost = (int)Mathf.Ceil((shipReactor.maxCoreHealth - shipReactor.coreHealth) * 5);
        repairCost = (int)Mathf.Ceil((shipDefense.maxHull - shipDefense.hull) * 5);

        rechargeButton.interactable = player.HasCredits(rechargeCost);
        repairButton.interactable = player.HasCredits(repairCost);

        rechargeCostText.text = rechargeCost.ToString();
        repairCostText.text = repairCost.ToString();

        rechargeCostText.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
        repairCostText.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();

        rechargeButton.enabled = (rechargeCost > 0);
        repairButton.enabled = (repairCost > 0);

        rechargeCostObject.SetActive(rechargeCost > 0);
        repairCostObject.SetActive(repairCost > 0);
    }

}
