#include "FrameBuffer.hpp"

using namespace std;

int FrameBuffer::GetNumOfAvailableElements()
{
    const lock_guard<mutex> _guard(lock);
    return buffer.size();
}

FrameState FrameBuffer::Read(int index)
{
    const lock_guard<mutex> _guard(lock);
    return buffer[index];
}

FrameState FrameBuffer::ReadAndErase(int index)
{
    const lock_guard<mutex> _guard(lock);
    FrameState result = buffer[index];
    while(index-- >= 0) buffer.pop_front();
    return result;
}

void FrameBuffer::Write(const FrameState& frameState)
{
    const lock_guard<mutex> _guard(lock);
    buffer.push_back(frameState);
}
