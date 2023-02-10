using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Controller;
using Controllers;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simulation
{
    public class BridgePictureSimulation:BridgeSimulation
    {

        private Vector3 simulationSpace;
        public bool simulating;
        public GameObject blockPrevab;
        private List<GameObject> blocks= new List<GameObject>();
        private Transform droppedBlock;
        public Material mat1;
        public Material mat2;
        public bool dropBlockInBounds;
        public Camera camera;
        public int resWidth;
        public int resHeight;
        private RenderTexture renderTexture;
        private IImageSubmitter submitter;
        

        public override async Task Simulate(Chromosome c, Vector3 space,CancellationToken token)
        {
            await Task.Delay(50);
            submitter = new SubmitImageToWebApi(GameObject.FindObjectOfType<CoroutineRunner>());
            var score = 0;
            renderTexture = new RenderTexture(resWidth, resHeight, 24);
            camera.targetTexture = renderTexture;
            BuildBridge(c, space,blockPrevab,blocks);
            byte[] s = TakeScreenshot();
            await Task.Delay(10000);
            byte[] s2 = TakeScreenshot();
            var result = false;
            var tasks = new List<Task<string>>();
            
            /*tasks.Add(submitter.SubmitImageAsync(token,s));
            tasks.Add(submitter.SubmitImageAsync(token,s2));
            await Task.WhenAll(tasks);*/
            EndSimulation();
            c.imageResults.Add("start",await submitter.SubmitImageAsync(token,s));
            c.imageResults.Add("end",await submitter.SubmitImageAsync(token,s2));
            simulating = true;

        }

        private byte[] TakeScreenshot()
        {
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            RenderTexture.active = null;
            byte[] bytes = screenShot.EncodeToPNG();
            return bytes;
        }
        public void OnDestroy()
        {
            camera.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(renderTexture);
        }

        private void printdict(Dictionary<int[],float> a)
        {
            var s = "";
            foreach (var VARIABLE in a)
            {
                s += "[";
                foreach (var keyint in VARIABLE.Key)
                {
                    s += keyint;
                    s += ",";
                }
                s += "] ";
                s += VARIABLE.Value;
                s += ",";
            }
            
            Debug.Log(s);
        }
        public override void EndSimulation()
        {
            foreach (var block in blocks)
            {
                Destroy(block);
            }
            if (droppedBlock!=null)
                Destroy(droppedBlock.gameObject);

        }

        public async Task<bool> DropBlock(int weight, Vector3 cubeSpawnPos,CancellationToken token)
        {
            
            droppedBlock = Instantiate(blockPrevab,cubeSpawnPos,Quaternion.identity).transform;
            droppedBlock.GetComponent<Rigidbody>().mass = 0.1f + weight;
            droppedBlock.GetComponent<MeshRenderer>().material = mat1;
            droppedBlock.gameObject.layer = 7;
            await Task.Delay(8300,token);
            var result = dropBlockInBounds;
            return result;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (droppedBlock == null) return;
            if (other.gameObject != droppedBlock.gameObject) return;
            droppedBlock.GetComponent<MeshRenderer>().material = mat1;
            dropBlockInBounds = true;

        }

        private void OnTriggerExit(Collider other)
        {
            if (droppedBlock == null) return;
            if (other.gameObject != droppedBlock.gameObject) return;
            droppedBlock.GetComponent<MeshRenderer>().material = mat2;
            dropBlockInBounds = false;
        }




    }
}