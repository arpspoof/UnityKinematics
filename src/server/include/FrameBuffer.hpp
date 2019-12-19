#pragma once

#include "DataFormat.hpp"

#include <queue>
#include <mutex>

class FrameBuffer
{
    std::mutex lock;
    std::deque<FrameState> buffer;

public:
    int GetNumOfAvailableElements();

    FrameState Read(int index);
    FrameState ReadAndErase(int index);

    void Write(const FrameState& frameState);
};
