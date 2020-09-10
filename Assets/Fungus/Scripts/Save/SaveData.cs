﻿// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using System;
using System.Collections.Generic;
using System.Linq;

namespace Fungus
{
    /// <summary>
    /// Serializable container for a Save Point's data.
    /// All data is stored as strings, in SaveDataItems. SaveDataSerializers being responsible for encoding and decoding.
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public int version = FungusConstants.CurrentSaveDataVersion;
        public string saveName;
        public List<SaveDataItem> saveDataItems;
        public List<StringPair> stringPairs;
        public string lastWrittenDateTimeString;

        public SaveData(string _saveName)
        {
            Init(_saveName, Array.Empty<StringPair>());
        }

        public SaveData(string _saveName, params StringPair[] pairs)
        {
            Init(_saveName, pairs);
        }

        private void Init(string _saveName, StringPair[] pairs)
        {
            saveName = _saveName;
            saveDataItems = new List<SaveDataItem>();
            stringPairs = new List<StringPair>(pairs);
            lastWrittenDateTimeString = System.DateTime.Now.ToString("O");
        }

        public System.DateTime LastWritten { get { return System.DateTime.Parse(lastWrittenDateTimeString); } }

        public string GetStringPairValue(string key)
        {
            return stringPairs.FirstOrDefault(x => x.key == key)?.val ?? string.Empty;
        }
    }
}