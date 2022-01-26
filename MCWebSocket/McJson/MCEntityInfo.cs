
using System;
using System.Numerics;

namespace ShortDev.Minecraft.Bedrock.McWebSocket.McJson
{
    public struct MCEntityInfo
    {
        public MCDimension dimension;
        public MCVector3 position;
        public string uniqueId;
        public double yRot;

        public Guid GetID()
        {
            return Guid.Parse(uniqueId);
        }
        public Vector3 GetPosition()
        {
            return new Vector3((float)position.x, (float)position.y, (float)position.z);
        }
        public Vector2 GetPosition2D()
        {
            return new Vector2((float)position.x, (float)position.z);
        }
    }
}
