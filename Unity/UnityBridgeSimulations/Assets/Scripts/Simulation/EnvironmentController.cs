using UnityEngine;

namespace Simulation
{
    public class EnvironmentController : MonoBehaviour
    {
        public Transform leftFloor;
        public Transform rightFloor;
        public Transform bottomFloor;
        /// <summary>
        /// setup the environment in which the simulation will be run
        /// </summary>
        /// <param name="chromosomeX"> the x size of the chromosome </param>
        /// <param name="chromosomeY"> the y size of the chromosome </param>
        /// <param name="chromosomeZ"> the z size of the chromosome </param>
        public void Setup(int chromosomeX,int chromosomeY,int chromosomeZ)
        {
            leftFloor.transform.localPosition = new Vector3(0, 0, -4.5f);
            rightFloor.transform.localPosition = new Vector3(0, 0, 4.5f + chromosomeZ - 1);
            bottomFloor.transform.localPosition = new Vector3(0, -Mathf.Ceil(chromosomeY/2f), chromosomeZ/2f);
        }
    }
}