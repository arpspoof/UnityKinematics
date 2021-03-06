#pragma once

#include "DataFormat.hpp"

class AbstractDataProvider
{
public:
    virtual FrameState GetCurrentState() const = 0;
    virtual ~AbstractDataProvider() {}
};
