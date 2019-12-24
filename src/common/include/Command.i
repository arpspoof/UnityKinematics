%{
    #include "Command.hpp"
%}

%include "std_vector.i"
%include "std_string.i"

namespace std {
   %template(vi) vector<int>;
   %template(vf) vector<float>;
   %template(vs) vector<string>;
};

struct Command
{
    std::string name;
    std::vector<int> pi;
    std::vector<float> pf;
    std::vector<std::string> ps;
    Command();
    Command(std::string name);
};

%feature("director") AbstractCommandHandler;

class AbstractCommandHandler
{
public:
    virtual ~AbstractCommandHandler();
    virtual int HandleCommand(Command cmd) = 0;
};

AbstractCommandHandler* GetCommandHandler();
void SetCommandHandler(AbstractCommandHandler* handler);
