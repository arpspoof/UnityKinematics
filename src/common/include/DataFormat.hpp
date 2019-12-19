#pragma once

#include <rpc/msgpack.hpp>

#include <string>
#include <vector>

struct Vec3 
{
    float x, y, z;
    Vec3() :x(0), y(0), z(0) {}
    Vec3(float x, float y, float z) :x(x), y(y), z(z) {}
    MSGPACK_DEFINE_ARRAY(x, y, z)
};

struct Quat
{
    float w, x, y, z;
    Quat() :w(1), x(0), y(0), z(0) {}
    Quat(float w, float x, float y, float z) :w(w), x(x), y(y), z(z) {}
    MSGPACK_DEFINE_ARRAY(w, x, y, z)
};

struct Transform
{
    Vec3 p;
    Quat q;
    Transform() {}
    Transform(Vec3 p, Quat q) :p(p), q(q) {}
    Transform(float x, float y, float z, float qw, float qx, float qy, float qz) 
        :p(Vec3(x, y, z)), q(Quat(qw, qx, qy, qz)) {}    
    MSGPACK_DEFINE_ARRAY(p, q)
};

struct ObjectState
{
    std::string objectName;
    Transform transform;
    ObjectState() {}
    ObjectState(std::string objectName, Transform transform) :objectName(objectName), transform(transform) {}
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
