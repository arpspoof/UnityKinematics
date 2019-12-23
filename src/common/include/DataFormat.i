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

struct FrameState
{
    std::string sessionName;
    std::vector<ObjectState> objectStates;
    FrameState();
    FrameState(std::string sessionName);
    FrameState(std::string sessionName, int nObj);
};
