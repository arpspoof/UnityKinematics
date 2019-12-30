%{
    #include "DataFormat.hpp"
%}

%include "std_vector.i"
%include "std_string.i"

struct ObjectState
{
    std::string objectName;
    float x, y, z;
    float qw, qx, qy, qz;
    ObjectState();
    ObjectState(std::string objectName, float x, float y, float z, float qw, float qx, float qy, float qz);
    ObjectState(std::string objectName);
};

namespace std {
   %template(vectorstate) vector<ObjectState>;
};

struct GroupState
{
    std::string groupName;
    std::vector<ObjectState> objectStates;
    GroupState() {}
    GroupState(std::string groupName) :groupName(groupName) {}
    GroupState(std::string groupName, int nObj) :groupName(groupName), objectStates(nObj) {}
};

namespace std {
   %template(vectorgroupstate) vector<GroupState>;
};

struct FrameState
{
    std::vector<GroupState> groups;
};
