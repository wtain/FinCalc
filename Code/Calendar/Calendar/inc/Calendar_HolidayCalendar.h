
#pragma once

#include "Calendar_Common.h"

#include <set>
#include <string>
#include <list>
#include <boost/locale/date_time.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/shared_ptr.hpp>

namespace Calendar
{
    enum BumpType
    {
        eBump_NoBump      = 0,
        eBump_Forward     = 1,
        eBump_ModForward  = 2,
        eBump_Backward    = 3,
        eBump_ModBackward = 4
    };

    // todo: daycounts

    class CALENDAR_API HolidayCalendarBase
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

    class CALENDAR_API HolidayCalendar 
        : public HolidayCalendarBase
    {
        std::set<Date> m_holidays;
    public:
    
        explicit HolidayCalendar(const std::string& path);
        explicit HolidayCalendar(std::istream& stream);

        virtual bool IsHoliday(Date date) const override;

        static boost::shared_ptr<HolidayCalendar> Empty;

        HolidayCalendar(); // creates an empty; it's public only for make_shared

    private:
        void init(const boost::property_tree::ptree& pt);
    };

    class CALENDAR_API WeekendCalendar
        : public HolidayCalendarBase
    {
    public:
        virtual bool IsHoliday(Date date) const override;

        static boost::shared_ptr<WeekendCalendar> Inst;
    };

    // todo: to separate files
    // todo: migrate shared to unique

    class CALENDAR_API CompositeCalendar
        : public HolidayCalendarBase
    {
        std::list<boost::shared_ptr<HolidayCalendarBase> > m_pCalendars;
    public:
        CompositeCalendar();

        void AddCalendar(boost::shared_ptr<HolidayCalendarBase> pCalendar);

        virtual bool IsHoliday(Date date) const override;
    };
}