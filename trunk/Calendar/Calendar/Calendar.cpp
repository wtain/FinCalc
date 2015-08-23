
#include "inc\Calendar_HolidayCalendar.h"

#include <boost/locale/generator.hpp>

int main(int argc, char* argv[])
{
    boost::locale::generator gen;
    std::locale::global(gen(""));

    HolidayCalendar rus("d:\\workspace_git\\FinCalc\\Data\\RUS.json");

	return 0;
}

