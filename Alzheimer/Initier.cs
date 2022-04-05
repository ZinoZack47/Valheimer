using UnityEngine;

namespace Alzheimer
{
    public class Initier
    {
        public static void Load()
        {
            _Load = new GameObject();
            _Load.AddComponent<AlCore>();
            GameObject.DontDestroyOnLoad(_Load);
        }
        public static void Unload()
        {
            Extract();
        }
        private static void Extract()
        {
            GameObject.Destroy(_Load);
        }

        private static GameObject _Load;
    }
}
