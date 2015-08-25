
#include <boost/test/unit_test.hpp>
#include <boost/make_shared.hpp>
#include <boost/locale/date_time.hpp>
#include <boost/locale/generator.hpp>
#include <string>
#include <sstream>

#include "..\Calendar\inc\Calendar_HolidayCalendar.h"
#include "Calendar_Fixture.h"


BOOST_FIXTURE_TEST_SUITE( HolidayCalendarTests, Fixture )

BOOST_AUTO_TEST_CASE( JsonCalendarParseTest )
{
    std::istringstream stream(JsonCalendarRus());
    Calendar::HolidayCalendar rus(stream);
    Calendar::WeekendCalendar xxx;

    Date t(2014, 1, 1);

    int nHolidays = 0;
    for (; t.year() == 2014; t += boost::gregorian::date_duration(1))
    {
        if (rus.IsHoliday(t) || xxx.IsHoliday(t))
            ++nHolidays;
    }

    BOOST_REQUIRE_EQUAL(nHolidays, 118);
}

BOOST_AUTO_TEST_CASE( CompositeCalendarTest )
{
    std::istringstream stream(JsonCalendarRus());
    boost::shared_ptr<Calendar::HolidayCalendar> pRus = boost::make_shared<Calendar::HolidayCalendar>(stream);
    boost::shared_ptr<Calendar::WeekendCalendar> pXxx  = boost::make_shared<Calendar::WeekendCalendar>();
    boost::shared_ptr<Calendar::CompositeCalendar> pComposite  = boost::make_shared<Calendar::CompositeCalendar>();

    pComposite->AddCalendar(pRus);
    pComposite->AddCalendar(pXxx);

    Date t(2014, 1, 1);

    int nHolidays = 0;
    for (; t.year() == 2014; t += boost::gregorian::date_duration(1))
    {
        BOOST_REQUIRE_EQUAL(pRus->IsHoliday(t) || pXxx->IsHoliday(t), pComposite->IsHoliday(t));
    }
}

BOOST_AUTO_TEST_SUITE_END()