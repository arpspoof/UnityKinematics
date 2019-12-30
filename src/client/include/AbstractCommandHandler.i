%{
    #include "AbstractCommandHandler.hpp"
%}

%feature("director") AbstractCommandHandler;

class AbstractCommandHandler
{
public:
    virtual void HandleCommand(const Command& cmd) const = 0;
    virtual ~AbstractCommandHandler() {}
};
