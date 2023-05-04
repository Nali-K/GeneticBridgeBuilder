using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{
    public struct PoolableBlock
    {
        public static Stack<PoolableBlock> blockPool= new Stack<PoolableBlock>();
        public GameObject BlockObject;
        public Rigidbody BlockRigidbody;
        public BlockData BlockData;
        public Renderer BlockRenderer;

    }
}