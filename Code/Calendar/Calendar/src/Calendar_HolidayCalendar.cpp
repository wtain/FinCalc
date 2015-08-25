
#include "..\inc\Calendar_HolidayCalendar.h"

#include <boost/property_tree/json_parser.hpp>
#include <boost/foreach.hpp>
#include <boost/lexical_cast.hpp>
#include <exception>

HolidayCalendarBase::~HolidayCalendarBase()
{

}

Date HolidayCalendarBase::BumpForward(Date date)
{
    while (IsHoliday(date))
        date += boost::gregorian::date_duration(1);
    return date;
}

Date HolidayCalendarBase::BumpModForward(Date date)
{
    Date bumpedDate = BumpForward(date);
    
    int m1 = date.month();
    int m2 = bumpedDate.month();
    if (m1 == m2)
        return bumpedDate;
    return BumpBackward(date);
}

Date HolidayCalendarBase::BumpBackward(Date date)
{
    while (IsHoliday(date))
        date -= boost::gregorian::date_duration(1);
    return date;
}

Date HolidayCalendarBase::BumpModBackward(Date date)
{
    Date bumpedDate = BumpBackward(date);
       
    int m1 = date.month();
    int m2 = bumpedDate.month();
    if (m1 == m2)
        return bumpedDate;
    return BumpForward(date);
}

Date HolidayCalendarBase::Bump(Date date, BumpType type)
{
    switch (type)
    {
        case eBump_NoBump:
            return date;
        case eBump_Forward:
            return BumpForward(date);
        case eBump_ModForward:
            return BumpForward(date);
        case eBump_Backward:
            return BumpBackward(date);
        case eBump_ModBackward:
            return BumpModBackward(date);
    }
    throw std::exception("Invalid bump type");
}

HolidayCalendar::HolidayCalendar(const std::string& path)
{
    boost::property_tree::ptree pt;
    boost::property_tree::read_json(path, pt);

    init(pt);
}

HolidayCalendar::HolidayCalendar(std::istream& stream)
{
    boost::property_tree::ptree pt;
    boost::property_tree::read_json(stream, pt);

    init(pt);
}

void HolidayCalendar::init(const boost::property_tree::ptree& pt)
{
    BOOST_FOREACH(const boost::property_tree::ptree::value_type &v, pt.get_child("data"))
    {
        const int year = boost::lexical_cast<int>(v.first);
        BOOST_FOREACH(const boost::property_tree::ptree::value_type &v1, v.second)
        {
            const int month = boost::lexical_cast<int>(v1.first);
            BOOST_FOREACH(const boost::property_tree::ptree::value_type &v2, v1.second)
            {
                bool bHoliday = true;
                BOOST_FOREACH(const boost::property_tree::ptree::value_type &v3, v2.second)
                {
                    if (v3.first != "isWorking")
                        continue;
                    if (v3.second.get_value<int>() == 3)
                    {
                        bHoliday = false;
                        break;
                    }
                }
                if (!bHoliday)
                    continue;
                const int day = boost::lexical_cast<int>(v2.first);
                Date t(year, month, day);
                assert(m_holidays.end() == m_holidays.find(t));
                m_holidays.insert(t);
            }
        }
    }
}

HolidayCalendar HolidayCalendar::Empty;

HolidayCalendar::HolidayCalendar()
{

}

bool HolidayCalendar::IsHoliday(Date date) const
{
    return m_holidays.end() != m_holidays.find(date);
}

bool WeekendCalendar::IsHoliday(Date date) const
{
    return date.day_of_week() == boost::date_time::Saturday || 
           date.day_of_week() == boost::date_time::Sunday;
}

WeekendCalendar WeekendCalendar::Inst;