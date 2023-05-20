-------------------------------------------------------------------------------------------
-- ProductTypes
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	productType.Id
	,(CASE WHEN productType.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,productType.InternalId
    ,productType.Name
INTO #T1
FROM
	WSA.ProductType productType


SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	Name