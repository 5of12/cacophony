using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace Cacophony
{
    public enum FingerName { THUMB, INDEX, MIDDLE, RING, PINKY }

    [System.Serializable]
    public class SimpleFinger
    {
        [Range(0,1)]
        public float curl;
        [Range(0,1)]
        public float bend;
        [Range(0,1)]
        public float splay;

        public SimpleFinger ()
        {
            curl = 0;
            bend = 0;
            splay = 0;
        }
    }

    [System.Serializable]
    public class SimpleHandPose
    {
        public SimpleFinger thumb;
        public SimpleFinger index;
        public SimpleFinger middle;
        public SimpleFinger ring;
        public SimpleFinger pinky;
        
        [Tooltip("Provide a zero vector to ignore direction")]
        public Vector3 palmDirection;
        [Tooltip("Provide a zero vector to ignore normal")]
        public Vector3 palmNormal;

        private SimpleFinger[] _fingers;
        public SimpleFinger[] fingers { get { return _fingers; } }

        public SimpleHandPose()
        {
            _fingers = new SimpleFinger[5];

            _fingers[0] = thumb = new SimpleFinger { };
            _fingers[1] = index = new SimpleFinger { };
            _fingers[2] = middle = new SimpleFinger { };
            _fingers[3] = ring = new SimpleFinger { };
            _fingers[4] = pinky = new SimpleFinger { };

            palmDirection = Vector3.forward;
            palmNormal = Vector3.down;
        }
    }
    
}