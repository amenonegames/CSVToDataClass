
using UnityEngine;

namespace Assets.DataClassGenerator.Samples.UsecaseSample.Scripts
{

    [System.Serializable]
    public class Sample
    {

        [SerializeField]
        private int	 _id;
        public int	 Id 
        { 
            get => _id; 
            set => _id = value;
        }

        [SerializeField]
        private int _health;
        public int Health 
        { 
            get => _health; 
            set => _health = value;
        }

        [SerializeField]
        private float _damage;
        public float Damage 
        { 
            get => _damage; 
            set => _damage = value;
        }

        [SerializeField]
        private string _description;
        public string Description 
        { 
            get => _description; 
            set => _description = value;
        }
    }
}