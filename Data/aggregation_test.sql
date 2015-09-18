SELECT exp.Date, cat.Name, exp.Amount
 FROM expenses exp
 LEFT JOIN categories cat
 ON exp.CategoryId=cat.CategoryId;
 
 
 SELECT exp.Date, cat.Name, SUM(exp.Amount)
 FROM expenses exp
 LEFT JOIN categories cat
 ON exp.CategoryId=cat.CategoryId 
 GROUP BY exp.Date, cat.Name;