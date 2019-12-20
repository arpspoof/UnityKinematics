#pragma once

#include "DataFormat.hpp"
#include <chrono>

class AbstractDataProvider
{
    std::chrono::time_point<std::chrono::high_resolution_clock> startTime;
    int slotCounter;

public:
    // 1 slot = 1/6 milliseconds
    // frequency = slots / update 
    // e.g. 60Hz = 100 slots / update 
    int frequency;
    
    void Tick(int slots);

    AbstractDataProvider();
    virtual ~AbstractDataProvider();

public: // Abstract
    virtual FrameState GetCurrentState() const = 0;
};
