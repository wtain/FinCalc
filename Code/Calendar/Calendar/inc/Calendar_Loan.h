
#pragma once

#include "Calendar_Common.h"
#include "Calendar_HolidayCalendar.h"

#include <boost/locale/date_time.hpp>
#include <vector>

class Loan
{
public:
    struct Cashflow
    {
        Date date;
        Currency principalPayment;
        Currency interestPayment;

        Currency Payment() const { return principalPayment + interestPayment; }

        Cashflow() {}

        Cashflow(Date date_, Currency principalPayment_, Currency interestPayment_)
            : date(date_)
            , principalPayment(principalPayment_)
            , interestPayment(interestPayment_)
        {

        }
    };

private:
    std::vector<Cashflow> m_payments;
    Currency m_principal;
    double m_rate;
    const HolidayCalendarBase& m_rCalendar;
    Currency m_payment;

public:
    Loan(Currency principal, double rate, Date startDate, int duration, const HolidayCalendarBase& calendar = HolidayCalendar::Empty);

    Currency TotalInterest() const;

    void AdditionalPayment(Currency payment, Date date);

    double Rate() const;
    Currency Principal() const;
    Currency Payment() const;
};