
#pragma once

#include <boost/date_time/gregorian/gregorian.hpp>

typedef double Currency;
typedef boost::gregorian::date Date;

#ifdef CALENDAR_EXPORTS
#define CALENDAR_API __declspec(dllexport)
#else
#define CALENDAR_API __declspec(dllimport)
#endif

// class needs to have dll-interface to be used by clients of class 'AClass'
#pragma warning (disable: 4251)