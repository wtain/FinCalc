
#pragma once

#include "Calendar_Common.h"
#include "Calendar_HolidayCalendar.h"

#include <boost/locale/date_time.hpp>
#include <vector>

namespace Calendar
{
    class CALENDAR_API Loan
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
        boost::shared_ptr<const HolidayCalendarBase> m_pCalendar;
        Currency m_payment;

    public:
        Loan(Currency principal, double rate, Date startDate, int duration, boost::shared_ptr<const HolidayCalendarBase> pCalendar = HolidayCalendar::Empty);

        Currency TotalInterest() const;
        Currency TotalPayment() const;
        Currency TotalPrincipalPayment() const;

        void AdditionalPayment(Currency payment, Date date);

        double Rate() const;
        Currency Principal() const;
        Currency Payment() const;

        size_t NumberOfPayments() const;
    };
}