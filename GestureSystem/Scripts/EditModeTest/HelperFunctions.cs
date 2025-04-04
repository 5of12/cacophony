using UnityEngine;

namespace Cacophony 
{
namespace Testing
{
    public static class Helper
    {
        public static SimpleHandPose CreateFistPose()
        {
            SimpleHandPose pose = new SimpleHandPose();
            foreach(var finger in pose.fingers)
            {
                finger.curl = 1;
                finger.bend = 1;
                finger.splay = 0;
            }
            pose.palmNormal = new Vector3(0 -1, 0);
            pose.palmDirection = new Vector3(0, 0, 1);
            return pose;
        }

        public static SimpleHandPose CreatePointingPose()
        {
            SimpleHandPose pose = CreateFistPose();
            pose.index.curl = 0;
            pose.index.bend = 0;

            return pose;
        }

        public static SimpleHandPose CreateExtendedPose()
        {
            SimpleHandPose pose = new SimpleHandPose();
            foreach(var finger in pose.fingers)
            {
                finger.splay = 1f;
            }
            pose.palmNormal = new Vector3(0 -1, 0);
            pose.palmDirection = new Vector3(0, 0, 1);
            return pose;
        }

        public static SimpleHandPose CreateSpatulaPose()
        {
            SimpleHandPose pose = new SimpleHandPose();
            foreach(var finger in pose.fingers)
            {
                pose.index.curl = 0;
                pose.index.bend = 0;
                finger.splay = 0f;
            }
            pose.palmNormal = new Vector3(0 -1, 0);
            pose.palmDirection = new Vector3(0, 0, 1);
            return pose;
        }

        public static SimpleHandPose CreateFlatBendPose()
        {
            SimpleHandPose pose = new SimpleHandPose();
            foreach(var finger in pose.fingers)
            {
                pose.index.curl = 0;
                pose.index.bend = 1;
                finger.splay = 1f;
            }
            pose.palmNormal = new Vector3(0 -1, 0);
            pose.palmDirection = new Vector3(0, 0, 1);
            return pose;
        }

        public static SimpleHandPose CreateStraightCurlPose()
        {
            SimpleHandPose pose = new SimpleHandPose();
            foreach(var finger in pose.fingers)
            {
                pose.index.curl = 1;
                pose.index.bend = 0;
                finger.splay = 1f;
            }
            pose.palmNormal = new Vector3(0 -1, 0);
            pose.palmDirection = new Vector3(0, 0, 1);
            return pose;
        }

    }
}
}