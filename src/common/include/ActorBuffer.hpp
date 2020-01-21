#pragma once

#include <queue>
#include <mutex>

template <typename T> class ActorBuffer
{
    std::mutex lock;
    std::deque<T> buffer;

public:
    int GetNumOfAvailableElements()
    {
        const std::lock_guard<std::mutex> _guard(lock);
        return buffer.size();
    }

    T Read(int index)
    {
        const std::lock_guard<std::mutex> _guard(lock);
        return buffer[index];
    }

    T ReadAndErase(int index)
    {
        const std::lock_guard<std::mutex> _guard(lock);
        T result = buffer[index];
        while(index-- >= 0) buffer.pop_front();
        return result;
    }

    void Write(const T& data)
    {
        const std::lock_guard<std::mutex> _guard(lock);
        buffer.push_back(data);
    }
};
