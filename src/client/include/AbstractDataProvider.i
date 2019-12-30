%{
    #include "AbstractDataProvider.hpp"
%}

%feature("director") AbstractDataProvider;

class AbstractDataProvider
{
public:
    virtual FrameState GetCurrentState() const = 0;
    virtual ~AbstractDataProvider() {}
};
