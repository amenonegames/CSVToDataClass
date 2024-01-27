using System;
using System.Linq;
using Amenonegames.CSVDataParser;
using UnityEngine;

namespace Assets.DataClassGenerator.Sample.Scripts
{
    public class User : MonoBehaviour
    {
        private void Start()
        {
            GetSampleData();
        }

        private void GetSampleData()
        {
            var data = TextParser.ToModels<Sample>(Resources.Load<TextAsset>("Sample"));
            Debug.Log("Damage:" + data[0].Damage.ToString());
        }
    }
}