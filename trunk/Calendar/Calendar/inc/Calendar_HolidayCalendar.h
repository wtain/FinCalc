
#pragma once

#include "Calendar_Common.h"

#include <set>
#include <string>
#include <boost/locale/date_time.hpp>

enum BumpType
{
    eBump_NoBump      = 0,
    eBump_Forward     = 1,
    eBump_ModForward  = 2,
    eBump_Backward    = 3,
    eBump_ModBackward = 4
};

class HolidayCalendarBase
{
public:
    virtual ~HolidayCalendarBase();

    Date BumpForward(Date date);
    Date BumpModForward(Date date);
    Date BumpBackward(Date date);
    Date BumpModBackward(Date date);
    Date Bump(Date date, BumpType type);

    virtual bool IsHoliday(Date date) const = 0;
};

class HolidayCalendar 
    : public HolidayCalendarBase
{
    std::set<Date> m_holidays;
public:
    
    explicit HolidayCalendar(const std::string& path);

    virtual bool IsHoliday(Date date) const override;

    static HolidayCalendar Empty;

private:
    HolidayCalendar();
};

class WeekendCalendar
    : public HolidayCalendarBase
{
public:
    virtual bool IsHoliday(Date date) const override;

    static WeekendCalendar Inst;
};