
#pragma once

#include "Calendar_Common.h"

#include <set>
#include <string>
#include <boost/locale/date_time.hpp>

class HolidayCalendar
{
    std::set<boost::locale::date_time> m_holidays;
public:
    
    explicit HolidayCalendar(const std::string& path);
};