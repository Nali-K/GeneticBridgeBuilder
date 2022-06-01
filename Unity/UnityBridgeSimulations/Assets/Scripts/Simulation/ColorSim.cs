using System;
using UnityEngine;
/*
namespace Simulation
{
    [Serializable]
    public class ColorSim:Simulation
    {
        public GameObject prefab;
        public Material red;
        public Material green;
        public Material blue;
        public Material orange;
        public Material purple;
        public Material noir;
        public Material white;
        public Material cyan;
        public Material yellow;
        public Material pink;
        
        
        

        public override void Simulate(Chromosome c, Vector3 space)
        {
            var dict = c.GetValuesAndPositions(new[] {-1, -1, -1});
            foreach (var VARIABLE in dict)
            {
                Debug.Log(VARIABLE.Value);
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
                if (VARIABLE.Value == 4)
                {
                    obj.GetComponent<MeshRenderer>().material = orange;
                }
                if (VARIABLE.Value == 5)
                {
                    obj.GetComponent<MeshRenderer>().material = purple;
                }
                if (VARIABLE.Value == 6)
                {
                    obj.GetComponent<MeshRenderer>().material = noir;
                }
                if (VARIABLE.Value == 7)
                {
                    obj.GetComponent<MeshRenderer>().material = white;
                }
                if (VARIABLE.Value == 8)
                {
                    obj.GetComponent<MeshRenderer>().material = cyan;
                }
                if (VARIABLE.Value == 9)
                {
                    obj.GetComponent<MeshRenderer>().material = yellow;
                }
                if (VARIABLE.Value == 10)
                {
                    obj.GetComponent<MeshRenderer>().material = pink;
                }
            }
        }

        public override void EndSimulation()
        {
            throw new NotImplementedException();
        }
    }
    
}*/