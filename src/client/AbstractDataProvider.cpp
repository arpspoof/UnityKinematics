#include "AbstractDataProvider.hpp"
#include "RPCClient.hpp"

#include <thread>
#include <cmath>

using namespace std;
using namespace std::chrono;

AbstractDataProvider::AbstractDataProvider() 
    :slotCounter(-1), frequency(100)
{
}

AbstractDataProvider::~AbstractDataProvider()
{
}

void AbstractDataProvider::Tick(int slots)
{
    if (slotCounter >= 0) {
        slotCounter += slots;
    }

    if (slotCounter >= frequency || slotCounter < 0) {
        int bufferedFrames = CreateFrame(GetCurrentState());

        int sleepDuration = (int)((bufferedFrames / 2) * frequency / 6.0);
        if (sleepDuration > 0) {
            this_thread::sleep_for(milliseconds(sleepDuration));
        }

        if (slotCounter >= frequency) {
            slotCounter -= frequency;
            int64_t duration = duration_cast<milliseconds>(high_resolution_clock::now() - startTime).count();
            int64_t requiredDuration = (int64_t)floor(frequency / 6.0);
            if (duration < requiredDuration) {
                this_thread::sleep_for(milliseconds(requiredDuration - duration));
            }
        }
        else {
            slotCounter = 0;
        }

        startTime = high_resolution_clock::now();
    }
}
