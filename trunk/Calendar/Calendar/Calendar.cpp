
#include <boost/property_tree/json_parser.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/foreach.hpp>
#include <boost/lexical_cast.hpp>
#include <string>
#include <iostream>
#include <set>
#include <boost/locale/date_time.hpp>
#include <boost/locale/generator.hpp>
#include <sstream>

class HolidayCalendar
{
    std::set<boost::locale::date_time> m_holidays;
public:
    
    static const std::string Format_Date_ISO;

    HolidayCalendar(const std::string& path)
    {
        boost::property_tree::ptree pt;
        boost::property_tree::read_json(path, pt);

        BOOST_FOREACH(boost::property_tree::ptree::value_type &v, pt.get_child("data"))
        {
            const int year = boost::lexical_cast<int>(v.first);
            BOOST_FOREACH(boost::property_tree::ptree::value_type &v1, v.second)
            {
                const int month = boost::lexical_cast<int>(v1.first);
                BOOST_FOREACH(boost::property_tree::ptree::value_type &v2, v1.second)
                {
                    const int day = boost::lexical_cast<int>(v1.first);
                    boost::locale::date_time t = boost::locale::period::year(year) + 
                                                 boost::locale::period::month(month) + 
                                                 boost::locale::period::day(day);
                    
                    m_holidays.insert(t);
                }
            }
        }
    }
};

int main(int argc, char* argv[])
{
    boost::locale::generator gen;
    std::locale::global(gen(""));

    HolidayCalendar rus("d:\\workspace_git\\FinCalc\\Data\\RUS.json");

	return 0;
}

