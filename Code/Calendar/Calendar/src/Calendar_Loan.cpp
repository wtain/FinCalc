
#include "..\inc\Calendar_Loan.h"

#include <boost/foreach.hpp>
#include <cmath>
#include <algorithm>
#include <iterator>
#include <assert.h>

namespace Calendar
{
    // todo: pointer for calendar
    Loan::Loan(Currency principal, double rate, Date startDate, int duration, boost::shared_ptr<const HolidayCalendarBase> pCalendar)
        : m_principal(principal)
        , m_rate(rate)
        , m_pCalendar(pCalendar)
    {
        const int nPaymentsPerYear = 12; // Monthly
        const double flowRate = rate / nPaymentsPerYear;
        m_payment = (principal * flowRate) / (1 - 1 / std::pow(1+flowRate, duration));
        Currency amount = principal;
        Date date = startDate; // paying immediately, percents for the month forward; in case of full deal the principal payment would be 1 month off
        m_payments.reserve(duration);
        for (int i = 0; i < duration; ++i)
        {
            const Currency interestPayment = amount * flowRate;
            const Currency principalPayment = m_payment - interestPayment;
            amount -= principalPayment;
            m_payments.push_back(Cashflow(date, principalPayment, interestPayment));
            date += boost::gregorian::months(1); // don't respect calendar
        }
        assert(std::abs(amount) < 0.01);
    }

    Currency Loan::TotalInterest() const
    {
        Currency rv = Currency();
        BOOST_FOREACH(const Cashflow& cf, m_payments)
        {
            rv += cf.interestPayment;
        }
        return rv;
    }

    Currency Loan::TotalPayment() const
    {
        Currency rv = Currency();
        BOOST_FOREACH(const Cashflow& cf, m_payments)
        {
            rv += cf.Payment();
        }
        return rv;
    }

    Currency Loan::TotalPrincipalPayment() const
    {
        Currency rv = Currency();
        BOOST_FOREACH(const Cashflow& cf, m_payments)
        {
            rv += cf.principalPayment;
        }
        return rv;
    }

    void Loan::AdditionalPayment(Currency payment, Date date)
    {
        // todo: + percents for partial month
        size_t i = 0;
        for (; i < m_payments.size(); ++i)
            if (m_payments[i].date > date)
                break;
        assert(i > 0 && i < m_payments.size());
        Currency totalPaid = 0.0;
        for (size_t j = 0; j < i; ++j)
            totalPaid += m_payments[j].principalPayment;
        totalPaid += payment;
        Currency newAmount = m_principal - totalPaid;
        const bool bHasFuturePayments = (i < m_payments.size());
        Date newStartDate = bHasFuturePayments ? m_payments[i+1].date : date;
        int newDuration = m_payments.size() - i;
        m_payments.reserve(m_payments.size() + 1);
        m_payments.resize(i);
        m_payments.push_back(Cashflow(date, payment, 0.0)); // todo: interest payments here
        if (bHasFuturePayments)
        {
            Loan newLoan(newAmount, m_rate, newStartDate, newDuration, m_pCalendar);
            std::copy(newLoan.m_payments.begin(), newLoan.m_payments.end(), std::back_inserter(m_payments));
            m_payment = newLoan.Payment();
        }
    }

    double Loan::Rate() const
    {
        return m_rate;
    }

    Currency Loan::Principal() const
    {
        return m_principal;
    }

    Currency Loan::Payment() const
    {
        return m_payment;
    }

    size_t Loan::NumberOfPayments() const
    {
        return m_payments.size();
    }
}