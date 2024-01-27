using System;
using System.Linq;
using Amenonegames.CSVHelperParser;
using UnityEngine;
using Assets.DataClassGenerator.Samples.UsecaseSample.Scripts;

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
            var data = TextParser.ToModels<Samples.UsecaseSample.Scripts.Sample>(Resources.Load<TextAsset>("Sample"));
            Debug.Log("Damage:" + data[0].Damage.ToString());
        }
    }
}