#pragma once

#include "DataFormat.hpp"

class AbstractDataProvider
{
public:
    virtual void GetCurrentState(FrameState& objectStateList) const = 0;
};
