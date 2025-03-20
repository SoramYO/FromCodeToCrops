using System;
using System.Collections.Generic;
using UnityEngine;
namespace GameSave
{
    [Serializable]
    public class StringIntPair
    {
        public string key;
        public int value;
    }

    [Serializable]
    public class SerializableDictionary
    {
        public List<StringIntPair> pairs = new List<StringIntPair>();

        public void AddPair(string key, int value)
        {
            pairs.Add(new StringIntPair { key = key, value = value });
        }

        public Dictionary<string, int> ToDictionary()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (var pair in pairs)
            {
                result[pair.key] = pair.value;
            }
            return result;
        }
    }

    [Serializable]
    public class SaveData
    {
        // Player info
        public float playerPosX;
        public float playerPosY;
        public float playerStamina;

        // Farm tiles
        public List<FarmTileData> farmTiles = new List<FarmTileData>();

        // Currency and other inventory items 
        public int money;
        public SerializableDictionary seedInventory;
        public SerializableDictionary cropInventory;

        // Animal Inventory (Fish, Pig, etc.)
        public SerializableDictionary animalInventory;

        // Pet info
        public string petName;
        public int petAffection;
        public int petFoodCount;

        // Time and Season
        public int currentDay;
        public int currentSeason;
        public float currentTime;
    }

    [Serializable]
    public class FarmTileData
    {
        public int x;
        public int y;
        public bool isPlowed;
        public bool isWatered;
        public string cropType;
        public int growthStage;
        public float growthTime;
    }
}