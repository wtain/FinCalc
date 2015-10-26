DROP VIEW IF EXISTS ExpenseByCategoryAndDateMonthAggregation;
CREATE VIEW ExpenseByCategoryAndDateMonthAggregation AS 
SELECT strftime('%m', exp.Date) AS Month, exp.Date, cat.Name, 
	SUM(exp.Amount * 
	      CASE
			WHEN cat.IsIncome=1 THEN 1
			ELSE -1
		  END) as Amount
 FROM expenses exp
 LEFT JOIN categories cat
 ON exp.CategoryId=cat.CategoryId 
 GROUP BY Month, exp.Date, cat.Name;