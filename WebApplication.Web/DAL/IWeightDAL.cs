﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.DAL
{
    public interface IWeightDAL
    {
		IList<int> GetWeights(DateTime startDate, DateTime endDate);
	}
}