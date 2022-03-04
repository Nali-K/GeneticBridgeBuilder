using System;
using UnityEngine;

namespace Simulation
{
    [Serializable]
    public class ColorSim:Simulation
    {
        public GameObject prefab;
        public Material red;
        public Material green;
        public Material blue;

        public override void Simulate(Chromosome c, Vector3 space)
        {
            var dict = c.GetValuesAndPositions(new[] {-1, -1, -1});
            foreach (var VARIABLE in dict)
            {
                var position = new Vector3(space.x,space.y,space.z);
                position.x += VARIABLE.Key[0];
                position.y += VARIABLE.Key[1];
                position.z += VARIABLE.Key[2];
                

                var obj = GameObject.Instantiate(prefab, position, Quaternion.identity);
                if (VARIABLE.Value == 1)
                {
                    obj.GetComponent<MeshRenderer>().material = blue;
                }

                if (VARIABLE.Value == 2)
                {
                    obj.GetComponent<MeshRenderer>().material = green;
                }

                if (VARIABLE.Value == 3)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
            }
        }

        public override void EndSimulation()
        {
            throw new NotImplementedException();
        }
    }
    
}