
#include <boost/test/unit_test.hpp>
#include <boost/locale/date_time.hpp>
#include <boost/locale/generator.hpp>
#include <string>
#include <sstream>

#include "..\Calendar\inc\Calendar_HolidayCalendar.h"

class Fixture
{
    static const std::string s_jsonCalendarRus;
public:
    
    Fixture()
    {
        
    }

    void SetLocale()
    {
        boost::locale::generator gen;
        std::locale::global(gen(""));
    }

    void WriteDate(const Date& t)
    {
        std::ostringstream stream;
        stream.imbue(std::locale());
        stream << boost::locale::as::date << t;
        std::cout << stream.str() << " ";
    }

    const std::string& JsonCalendarRus() const { return s_jsonCalendarRus; }
};

// todo: to resources
const std::string Fixture::s_jsonCalendarRus = "{\"data\":{\"2003\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":0},\"5\":{\"isWorking\":3},\"6\":{\"isWorking\":2},\"7\":{\"isWorking\":2}},\"2\":{\"24\":{\"isWorking\":2}},\"3\":{\"7\":{\"isWorking\":3},\"10\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"8\":{\"isWorking\":3},\"9\":{\"isWorking\":2}},\"6\":{\"11\":{\"isWorking\":3},\"12\":{\"isWorking\":2},\"13\":{\"isWorking\":2},\"21\":{\"isWorking\":0}},\"11\":{\"6\":{\"isWorking\":3},\"7\":{\"isWorking\":2}},\"12\":{\"11\":{\"isWorking\":3},\"12\":{\"isWorking\":2},\"31\":{\"isWorking\":3}}},\"2004\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"6\":{\"isWorking\":3},\"7\":{\"isWorking\":2}},\"2\":{\"23\":{\"isWorking\":2}},\"3\":{\"8\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"10\":{\"isWorking\":2}},\"6\":{\"11\":{\"isWorking\":3},\"14\":{\"isWorking\":2}},\"11\":{\"8\":{\"isWorking\":2}},\"12\":{\"13\":{\"isWorking\":2},\"31\":{\"isWorking\":3}}},\"2005\":{\"1\":{\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"10\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":3},\"23\":{\"isWorking\":2}},\"3\":{\"5\":{\"isWorking\":3},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2}},\"5\":{\"2\":{\"isWorking\":2},\"9\":{\"isWorking\":2}},\"6\":{\"13\":{\"isWorking\":2}},\"11\":{\"3\":{\"isWorking\":3},\"4\":{\"isWorking\":2}},\"12\":{\"12\":{\"isWorking\":2}}},\"2006\":{\"1\":{\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"9\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":3},\"23\":{\"isWorking\":2},\"24\":{\"isWorking\":2},\"26\":{\"isWorking\":0}},\"3\":{\"7\":{\"isWorking\":3},\"8\":{\"isWorking\":2}},\"5\":{\"1\":{\"isWorking\":2},\"6\":{\"isWorking\":3},\"8\":{\"isWorking\":2},\"9\":{\"isWorking\":2}},\"6\":{\"12\":{\"isWorking\":2}},\"11\":{\"3\":{\"isWorking\":3},\"6\":{\"isWorking\":2}}},\"2007\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"8\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":3},\"23\":{\"isWorking\":2}},\"3\":{\"7\":{\"isWorking\":3},\"8\":{\"isWorking\":2}},\"4\":{\"28\":{\"isWorking\":3},\"30\":{\"isWorking\":2}},\"5\":{\"1\":{\"isWorking\":2},\"8\":{\"isWorking\":3},\"9\":{\"isWorking\":2}},\"6\":{\"9\":{\"isWorking\":3},\"11\":{\"isWorking\":2},\"12\":{\"isWorking\":2}},\"11\":{\"5\":{\"isWorking\":2}},\"12\":{\"29\":{\"isWorking\":3},\"31\":{\"isWorking\":2}}},\"2008\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":3},\"25\":{\"isWorking\":2}},\"3\":{\"7\":{\"isWorking\":3},\"10\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"4\":{\"isWorking\":0},\"8\":{\"isWorking\":3},\"9\":{\"isWorking\":2}},\"6\":{\"7\":{\"isWorking\":0},\"11\":{\"isWorking\":3},\"12\":{\"isWorking\":2},\"13\":{\"isWorking\":2}},\"11\":{\"1\":{\"isWorking\":3},\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2}},\"12\":{\"31\":{\"isWorking\":3}}},\"2009\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2},\"9\":{\"isWorking\":2},\"11\":{\"isWorking\":0}},\"2\":{\"23\":{\"isWorking\":2}},\"3\":{\"9\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"1\":{\"isWorking\":2},\"8\":{\"isWorking\":3},\"11\":{\"isWorking\":2}},\"6\":{\"11\":{\"isWorking\":3},\"12\":{\"isWorking\":2}},\"11\":{\"3\":{\"isWorking\":3},\"4\":{\"isWorking\":2}},\"12\":{\"31\":{\"isWorking\":3}}},\"2010\":{\"1\":{\"1\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":2},\"23\":{\"isWorking\":2},\"27\":{\"isWorking\":3}},\"3\":{\"8\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"3\":{\"isWorking\":2},\"10\":{\"isWorking\":2}},\"6\":{\"11\":{\"isWorking\":3},\"14\":{\"isWorking\":2}},\"11\":{\"3\":{\"isWorking\":3},\"4\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"13\":{\"isWorking\":0}},\"12\":{\"31\":{\"isWorking\":3}}},\"2011\":{\"1\":{\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"10\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":3},\"23\":{\"isWorking\":2}},\"3\":{\"5\":{\"isWorking\":3},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2}},\"5\":{\"2\":{\"isWorking\":2},\"9\":{\"isWorking\":2}},\"6\":{\"13\":{\"isWorking\":2}},\"11\":{\"3\":{\"isWorking\":3},\"4\":{\"isWorking\":2}}},\"2012\":{\"1\":{\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"9\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":3},\"23\":{\"isWorking\":2}},\"3\":{\"7\":{\"isWorking\":3},\"8\":{\"isWorking\":2},\"9\":{\"isWorking\":2},\"11\":{\"isWorking\":0}},\"4\":{\"28\":{\"isWorking\":3},\"30\":{\"isWorking\":2}},\"5\":{\"1\":{\"isWorking\":2},\"5\":{\"isWorking\":0},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2},\"9\":{\"isWorking\":2},\"12\":{\"isWorking\":3}},\"6\":{\"9\":{\"isWorking\":3},\"11\":{\"isWorking\":2},\"12\":{\"isWorking\":2}},\"11\":{\"5\":{\"isWorking\":2}},\"12\":{\"29\":{\"isWorking\":3},\"31\":{\"isWorking\":2}}},\"2013\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2}},\"2\":{\"22\":{\"isWorking\":3}},\"3\":{\"7\":{\"isWorking\":3},\"8\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"8\":{\"isWorking\":3},\"9\":{\"isWorking\":2},\"10\":{\"isWorking\":2}},\"6\":{\"11\":{\"isWorking\":3},\"12\":{\"isWorking\":2}},\"11\":{\"4\":{\"isWorking\":2}},\"12\":{\"31\":{\"isWorking\":3}}},\"2014\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"3\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2}},\"2\":{\"24\":{\"isWorking\":3}},\"3\":{\"7\":{\"isWorking\":3},\"10\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"8\":{\"isWorking\":3},\"9\":{\"isWorking\":2}},\"6\":{\"11\":{\"isWorking\":3},\"12\":{\"isWorking\":2},\"13\":{\"isWorking\":2}},\"11\":{\"3\":{\"isWorking\":2},\"4\":{\"isWorking\":2}},\"12\":{\"31\":{\"isWorking\":3}}},\"2015\":{\"1\":{\"1\":{\"isWorking\":2},\"2\":{\"isWorking\":2},\"5\":{\"isWorking\":2},\"6\":{\"isWorking\":2},\"7\":{\"isWorking\":2},\"8\":{\"isWorking\":2},\"9\":{\"isWorking\":2}},\"2\":{\"20\":{\"isWorking\":3},\"23\":{\"isWorking\":2}},\"3\":{\"6\":{\"isWorking\":3},\"9\":{\"isWorking\":2}},\"4\":{\"30\":{\"isWorking\":3}},\"5\":{\"1\":{\"isWorking\":2},\"4\":{\"isWorking\":2},\"8\":{\"isWorking\":3},\"11\":{\"isWorking\":2}},\"6\":{\"11\":{\"isWorking\":3},\"12\":{\"isWorking\":2}},\"11\":{\"3\":{\"isWorking\":3},\"4\":{\"isWorking\":2}},\"12\":{\"31\":{\"isWorking\":3}}}}}";

BOOST_FIXTURE_TEST_SUITE( HolidayCalendarTests, Fixture )

BOOST_AUTO_TEST_CASE( JsonCalendarParseTest )
{
    std::istringstream stream(JsonCalendarRus());
    // todo: composite calendar
    HolidayCalendar rus(stream);
    WeekendCalendar xxx;

    SetLocale();

    Date t(2014, 1, 1);

    int nHolidays = 0;
    for (; t.year() == 2014; t += boost::gregorian::date_duration(1))
    {
        if (rus.IsHoliday(t) || xxx.IsHoliday(t))
            ++nHolidays;
    }

    BOOST_REQUIRE_EQUAL(nHolidays, 118);
}
//
//BOOST_AUTO_TEST_CASE( test_case2 )
//{
//    BOOST_REQUIRE_EQUAL( 1, 2 );
//    BOOST_FAIL( "Should never reach this line" );
//}
//
//BOOST_AUTO_TEST_SUITE_END()
//BOOST_AUTO_TEST_SUITE( test_suite2 )
//
//BOOST_AUTO_TEST_CASE( test_case3 )
//{
//    BOOST_CHECK( true );
//}
//
//BOOST_AUTO_TEST_CASE( test_case4 )
//{
//    BOOST_CHECK( false );
//}

BOOST_AUTO_TEST_SUITE_END()