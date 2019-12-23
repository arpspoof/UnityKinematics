%{
    #include "DataFormat.hpp"
%}

%include "std_vector.i"
%include "std_string.i"

struct PackVec3 
{
    float x, y, z;
    PackVec3() :x(0), y(0), z(0) {}
    PackVec3(float x, float y, float z) :x(x), y(y), z(z) {}
};

struct PackQuat
{
    float w, x, y, z;
    PackQuat() :w(1), x(0), y(0), z(0) {}
    PackQuat(float w, float x, float y, float z) :w(w), x(x), y(y), z(z) {}
};

struct PackTransform
{
    PackVec3 p;
    PackQuat q;
    PackTransform() {}
    PackTransform(PackVec3 p, PackQuat q) :p(p), q(q) {}
    PackTransform(float x, float y, float z, float qw, float qx, float qy, float qz) 
        :p(PackVec3(x, y, z)), q(PackQuat(qw, qx, qy, qz)) {}    
};

struct ObjectState
{
    std::string objectName;
    PackTransform transform;
    ObjectState() {}
    ObjectState(std::string objectName, PackTransform transform) :objectName(objectName), transform(transform) {}
};

namespace std {
   %template(vectorstate) vector<ObjectState>;
};

struct FrameState
{
    std::string sessionName;
    std::vector<ObjectState> objectStates;
    FrameState() {}
    FrameState(std::string sessionName) :sessionName(sessionName) {}
    FrameState(std::string sessionName, int nObj) :sessionName(sessionName), objectStates(nObj) {}
};
