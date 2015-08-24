
#include "inc\Calendar_HolidayCalendar.h"
#include "inc\Calendar_Loan.h"

#include <boost/locale/generator.hpp>

#include <iostream>

int main(int argc, char* argv[])
{
    boost::locale::generator gen;
    std::locale::global(gen(""));

    HolidayCalendar rus("d:\\workspace_git\\FinCalc\\Data\\RUS.json");

    Date t = boost::locale::period::year(2015) + 
             boost::locale::period::month(1) + 
             boost::locale::period::day(1);

    Loan loan(3800000.0, 0.12, t, 120, rus);

    const Currency ti = loan.TotalInterest();

    std::cout << "Total interest: " << ti << std::endl;
    std::cout << "Payment: " << loan.Payment() << std::endl;

    Date t1 = boost::locale::period::year(2015) + 
              boost::locale::period::month(6) + 
              boost::locale::period::day(1);
                                       
    loan.AdditionalPayment(200000.0, t1);

    const Currency ti1 = loan.TotalInterest();
    std::cout << "Total interest: " << ti1 << std::endl;
    std::cout << "Payment: " << loan.Payment() << std::endl;

	return 0;
}

