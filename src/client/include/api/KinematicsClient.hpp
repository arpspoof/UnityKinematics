#pragma once

#include <string>

#include "DataFormat.hpp"
#include "AbstractDataProvider.hpp"

void RPCStartClient(const std::string& addr, unsigned short port);
void RPCStopClient();
