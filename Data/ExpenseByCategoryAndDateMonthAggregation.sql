DROP VIEW IF EXISTS ExpenseByCategoryAndDateMonthAggregation;
CREATE VIEW ExpenseByCategoryAndDateMonthAggregation AS 
SELECT strftime('%m', exp.Date) AS Month, exp.Date, cat.Name, SUM(exp.Amount)
 FROM expenses exp
 LEFT JOIN categories cat
 ON exp.CategoryId=cat.CategoryId 
 GROUP BY Month, exp.Date, cat.Name;