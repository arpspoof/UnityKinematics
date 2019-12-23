#pragma once

#include <rpc/msgpack.hpp>

#include <string>
#include <vector>

struct PackVec3 
{
    float x, y, z;
    PackVec3() :x(0), y(0), z(0) {}
    PackVec3(float x, float y, float z) :x(x), y(y), z(z) {}
    MSGPACK_DEFINE_ARRAY(x, y, z)
};

struct PackQuat
{
    float w, x, y, z;
    PackQuat() :w(1), x(0), y(0), z(0) {}
    PackQuat(float w, float x, float y, float z) :w(w), x(x), y(y), z(z) {}
    MSGPACK_DEFINE_ARRAY(w, x, y, z)
};

struct PackTransform
{
    PackVec3 p;
    PackQuat q;
    PackTransform() {}
    PackTransform(PackVec3 p, PackQuat q) :p(p), q(q) {}
    PackTransform(float x, float y, float z, float qw, float qx, float qy, float qz) 
        :p(PackVec3(x, y, z)), q(PackQuat(qw, qx, qy, qz)) {}    
    MSGPACK_DEFINE_ARRAY(p, q)
};

struct ObjectState
{
    std::string objectName;
    PackTransform transform;
    ObjectState() {}
    ObjectState(std::string objectName, PackTransform transform) :objectName(objectName), transform(transform) {}
    MSGPACK_DEFINE_ARRAY(objectName, transform)
};

struct FrameState
{
    std::string sessionName;
    std::vector<ObjectState> objectStates;
    FrameState() {}
    FrameState(std::string sessionName) :sessionName(sessionName) {}
    FrameState(std::string sessionName, int nObj) :sessionName(sessionName), objectStates(nObj) {}
    MSGPACK_DEFINE_ARRAY(sessionName, objectStates)
};
