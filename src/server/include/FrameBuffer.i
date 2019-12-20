%{
    #include "FrameBuffer.hpp"
%}

class FrameBuffer
{
public:
    int GetNumOfAvailableElements();

    FrameState Read(int index);
    FrameState ReadAndErase(int index);

    void Write(const FrameState& frameState);
};
