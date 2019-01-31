﻿using EnumCollect;
using ManualTable.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace DB
{
    public sealed class DBReference : MonoBehaviour
    {
        [System.Serializable]
        public class DBKeyValuePair
        {
            public ListUpgrade Key;
            public ScriptableObject Value;
        }

        [System.Serializable]
        public class DBKeyValuePairOther
        {
            public DBType Key;
            public ScriptableObject Value;
        }

        public static DBReference Instance { get; private set; }

        private Dictionary<ListUpgrade, ITable> dbs;
        private Dictionary<DBType, ITable> dbos;

        public ITable this[ListUpgrade dbType]
        {
            get
            {
                try { return dbs[dbType]; }
                catch { return null; }
            }
        }
        public ITable this[DBType dbType]
        {
            get
            {
                try { return dbos[dbType]; }
                catch { return null; }
            }
        }

        public DBKeyValuePair[] InitalizeDB;
        public DBKeyValuePairOther[] InitalizeDBO;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);

            dbs = new Dictionary<ListUpgrade, ITable>();
            for (int i = 0; i < InitalizeDB?.Length; i++)
            {
                dbs[InitalizeDB[i].Key] = InitalizeDB[i].Value as ITable;
            }

            dbos = new Dictionary<DBType, ITable>();
            for (int i = 0; i < InitalizeDBO?.Length; i++)
            {
                dbos[InitalizeDBO[i].Key] = InitalizeDBO[i].Value as ITable;
            }
        }

    }
}