%{
    #include "ActorBuffer.hpp"
%}

template <typename T> class ActorBuffer
{
public:
    int GetNumOfAvailableElements();

    T Read(int index);
    T ReadAndErase(int index);
    
    void Write(const T& frameState);
};
