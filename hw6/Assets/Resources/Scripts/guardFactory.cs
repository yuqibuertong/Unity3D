using UnityEngine;

namespace myspace {
    class guardFactory: System.Object {
        private static guardFactory instance;

        public static guardFactory getInstance() {
            if (instance == null) instance = new guardFactory();
            return instance;
        }

        public GameObject getNewGuard(float x, float y, float z) {
            return Object.Instantiate(Resources.Load("Prefabs/Guard"), new Vector3(x, y, z), Quaternion.Euler(0, -90, 0)) as GameObject;
        }
    }
}