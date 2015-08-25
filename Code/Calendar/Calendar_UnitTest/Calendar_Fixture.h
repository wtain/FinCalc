
#pragma once

#include <string>

class Fixture
{
    static const std::string s_jsonCalendarRus;
public:
    
    Fixture()
    {
        
    }

    const std::string& JsonCalendarRus() const { return s_jsonCalendarRus; }
};
