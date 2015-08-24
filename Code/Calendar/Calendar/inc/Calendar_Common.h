
#pragma once

#include <boost/locale/date_time.hpp>

typedef double Currency;
typedef boost::locale::date_time Date;

#ifdef CALENDAR_EXPORTS
#define CALENDAR_API __declspec(dllexport)
#else
#define CALENDAR_API __declspec(dllimport)
#endif

// class needs to have dll-interface to be used by clients of class 'AClass'
#pragma warning (disable: 4251)