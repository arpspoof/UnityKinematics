#pragma once

#include "Command.hpp"
#include "AbstractDataProvider.hpp"
#include "AbstractCommandHandler.hpp"

void InitRenderController(
    const std::string& serverAddr, 
    unsigned short serverPort,
    const std::string& localAddr, 
    unsigned short commandHandlingPort, 
    int maxPhysicalFPS, 
    AbstractDataProvider* dataProvider, 
    AbstractCommandHandler* commandHandler
);

void DisposeRenderController();

void Pause(bool setPauseTo = true);
void SetPhysicalFPS(int fps);

void Tick(float physicalTimeStep);

void SendCustomCommand(const Command& cmd);
void CreatePrimitive(
    const std::string& type, 
    const std::string& groupName,
    const std::string& objectName,
    float param0,
    float param1,
    float param2
);
