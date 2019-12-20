%{
    #include "DataFormat.hpp"
%}

%include "std_vector.i"
%include "std_string.i"

struct Vec3 
{
    float x, y, z;
    Vec3() :x(0), y(0), z(0) {}
    Vec3(float x, float y, float z) :x(x), y(y), z(z) {}
};

struct Quat
{
    float w, x, y, z;
    Quat() :w(1), x(0), y(0), z(0) {}
    Quat(float w, float x, float y, float z) :w(w), x(x), y(y), z(z) {}
};

struct Transform
{
    Vec3 p;
    Quat q;
    Transform() {}
    Transform(Vec3 p, Quat q) :p(p), q(q) {}
    Transform(float x, float y, float z, float qw, float qx, float qy, float qz) 
        :p(Vec3(x, y, z)), q(Quat(qw, qx, qy, qz)) {}    
};

struct ObjectState
{
    std::string objectName;
    Transform transform;
    ObjectState() {}
    ObjectState(std::string objectName, Transform transform) :objectName(objectName), transform(transform) {}
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
