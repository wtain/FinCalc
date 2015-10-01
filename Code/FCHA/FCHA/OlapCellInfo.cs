using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
    public class OlapCellInfo
    {
        public OlapDimensionsTree LeftFilters;
        public OlapDimensionsTree TopFilters;

        public OlapCellInfo(OlapDimensionsTree leftFilters, OlapDimensionsTree topFilters)
        {
            LeftFilters = leftFilters;
            TopFilters = topFilters;
        }

        public override string ToString()
        {
            if (null != LeftFilters && null != TopFilters)
                return string.Format("{0}; {1}", LeftFilters.ToString(), TopFilters.ToString());
            else if (null != LeftFilters)
                return string.Format("{0}", LeftFilters.ToString());
            else if (null != TopFilters)
                return string.Format("{0}", TopFilters.ToString());
            else
                return string.Empty;
        }
    }
}
