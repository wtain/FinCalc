
#include "inc\Calendar_HolidayCalendar.h"
#include "inc\Calendar_Loan.h"

#include <boost/locale/generator.hpp>

#include <iostream>
#include <windows.h>

BOOL APIENTRY DllMain(HINSTANCE hinstDLL,
                      DWORD fdwReason, LPVOID lpvReserved)
{
    switch (fdwReason)
    {
    case DLL_PROCESS_ATTACH:
        {
            boost::locale::generator gen;
            std::locale::global(gen(""));
        }
      
        return 1; 

    case DLL_PROCESS_DETACH:
        break;

    case DLL_THREAD_ATTACH:
        break;

    case DLL_THREAD_DETACH:
        break;
    }
    return TRUE;
}

int main(int argc, char* argv[])
{
    boost::locale::generator gen;
    std::locale::global(gen(""));

    HolidayCalendar rus("d:\\workspace_git\\FinCalc\\Data\\RUS.json");

    Date t(2015, 1, 1);

    Loan loan(3800000.0, 0.12, t, 120, rus);

    const Currency ti = loan.TotalInterest();

    std::cout << "Total interest: " << ti << std::endl;
    std::cout << "Payment: " << loan.Payment() << std::endl;

    Date t1(2015, 6, 1);
                                       
    loan.AdditionalPayment(200000.0, t1);

    const Currency ti1 = loan.TotalInterest();
    std::cout << "Total interest: " << ti1 << std::endl;
    std::cout << "Payment: " << loan.Payment() << std::endl;

	return 0;
}

