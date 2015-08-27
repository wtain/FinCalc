
#include <boost/test/unit_test.hpp>
#include <boost/make_shared.hpp>

#include "../Calendar/inc/Calendar_HolidayCalendar.h"
#include "../Calendar/inc/Calendar_Loan.h"

#include "Calendar_Fixture.h"

BOOST_FIXTURE_TEST_SUITE( LoanTests, Fixture )

BOOST_AUTO_TEST_CASE( LoanTest_AdditionalPayment )
{
    std::istringstream stream(JsonCalendarRus());
    
    Date t(2015, 1, 1);

    Calendar::Loan loan(3800000.0, 0.12, t, 120, Calendar::HolidayCalendar::Empty);
    BOOST_REQUIRE_EQUAL(loan.NumberOfPayments(), 120);
    BOOST_REQUIRE_CLOSE(loan.TotalPrincipalPayment(), 3800000.0, Eps);
    BOOST_REQUIRE_CLOSE(loan.TotalPayment(), loan.TotalPrincipalPayment() + loan.TotalInterest(), Eps);

    Date t1(2015, 6, 1);
                                       
    loan.AdditionalPayment(200000.0, t1);
    BOOST_REQUIRE_EQUAL(loan.NumberOfPayments(), 121);
    BOOST_REQUIRE_CLOSE(loan.TotalPrincipalPayment(), 3800000.0, Eps);
    BOOST_REQUIRE_CLOSE(loan.TotalPayment(), loan.TotalPrincipalPayment() + loan.TotalInterest(), Eps);
}

BOOST_AUTO_TEST_CASE( LoanTest )
{
    std::istringstream stream(JsonCalendarRus());
    boost::shared_ptr<Calendar::HolidayCalendar> pRus = boost::make_shared<Calendar::HolidayCalendar>(stream);

    Date t(2015, 1, 25);

    Calendar::Loan loan(521450.0, 0.28, t, 26, pRus);
    BOOST_REQUIRE_EQUAL(loan.NumberOfPayments(), 26);
    BOOST_REQUIRE_CLOSE(loan.TotalPrincipalPayment(), 521450.0, Eps);
    BOOST_REQUIRE_CLOSE(loan.TotalPayment(), loan.TotalPrincipalPayment() + loan.TotalInterest(), Eps);

    std::ostringstream stream2;
    stream2 << loan;
    BOOST_TEST_MESSAGE(stream2.str());
}

BOOST_AUTO_TEST_SUITE_END()