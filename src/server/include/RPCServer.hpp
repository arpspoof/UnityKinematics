#pragma once

#include "FrameBuffer.hpp"

void RPCStart(unsigned short port);
void RPCStop();

FrameBuffer* RPCGetBuffer();
