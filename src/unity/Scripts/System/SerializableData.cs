using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityKinematics
{
    public static class SerializableObjects
    {
        [Serializable]
        public class SerializableData
        {
        }

        [Serializable]
        public class SerializableCommand : SerializableData
        {
            public int frameId;
            public string name;
            public List<int> pi;
            public List<float> pf;
            public List<string> ps;
            
            public SerializableCommand(Command cmd, int frameId)
            {
                this.frameId = frameId;
                this.name = cmd.name;
                this.pi = (from i in cmd.pi select i).ToList();
                this.pf = (from f in cmd.pf select f).ToList();
                this.ps = (from s in cmd.ps select s).ToList();
            }

            public static implicit operator Command(SerializableCommand scmd)
            {
                Command cmd = new Command(scmd.name);
                cmd.pi = new vi(scmd.pi);
                cmd.pf = new vf(scmd.pf);
                cmd.ps = new vs(scmd.ps);
                return cmd;
            }
        }

        [Serializable]
        public class SerializableObjectState : SerializableData
        {
            public string objectName;
            public float x, y, z;
            public float qw, qx, qy, qz;

            public SerializableObjectState(ObjectState obj)
            {
                this.objectName = obj.objectName;
                this.x = obj.x;
                this.y = obj.y;
                this.z = obj.z;
                this.qx = obj.qx;
                this.qy = obj.qy;
                this.qz = obj.qz;
                this.qw = obj.qw;
            }

            public static implicit operator ObjectState(SerializableObjectState sobj)
            {
                return new ObjectState(sobj.objectName, sobj.x, sobj.y, sobj.z, 
                    sobj.qw, sobj.qx, sobj.qy, sobj.qz);
            }
        }

        [Serializable]
        public class SerializableGroupState : SerializableData
        {
            public string groupName;
            public List<SerializableObjectState> objectStates;

            public SerializableGroupState(GroupState g)
            {
                groupName = g.groupName;
                objectStates = (from obj in g.objectStates select new SerializableObjectState(obj)).ToList();
            }

            public static implicit operator GroupState(SerializableGroupState sg)
            {
                GroupState g = new GroupState(sg.groupName);
                g.objectStates = new vectorstate(from sobj in sg.objectStates select (ObjectState)sobj);
                return g;
            }
        }

        [Serializable]
        public class SerializableFrameState : SerializableData
        {
            public double duration;
            public List<SerializableGroupState> groups;

            public SerializableFrameState(FrameState f)
            {
                duration = f.duration;
                groups = (from g in f.groups select new SerializableGroupState(g)).ToList();
            }

            public static implicit operator FrameState(SerializableFrameState sf)
            {
                FrameState f = new FrameState();
                f.duration = sf.duration;
                f.groups = new vectorgroupstate(from sg in sf.groups select (GroupState)sg);
                return f;
            }
        }
    }
}
