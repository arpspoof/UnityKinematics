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
