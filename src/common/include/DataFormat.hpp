#pragma once

#include <rpc/msgpack.hpp>

#include <string>
#include <vector>

struct ObjectState
{
    std::string objectName;
    float x, y, z;
    float qw, qx, qy, qz;
    ObjectState() {}
    ObjectState(std::string objectName, float x, float y, float z, float qw, float qx, float qy, float qz) 
        :objectName(objectName), x(x), y(y), z(z), qw(qw), qx(qx), qy(qy), qz(qz) {}
    ObjectState(std::string objectName) 
        :objectName(objectName), x(0), y(0), z(0), qw(1), qx(0), qy(0), qz(0) {}
    MSGPACK_DEFINE_ARRAY(objectName, x, y, z, qw, qx, qy, qz)
};

struct GroupState
{
    std::string groupName;
    std::vector<ObjectState> objectStates;
    GroupState() {}
    GroupState(std::string groupName) :groupName(groupName) {}
    GroupState(std::string groupName, int nObj) :groupName(groupName), objectStates(nObj) {}
    MSGPACK_DEFINE_ARRAY(groupName, objectStates)
};

struct FrameState
{
    double duration;
    std::vector<GroupState> groups;
    MSGPACK_DEFINE_ARRAY(duration, groups)
};
