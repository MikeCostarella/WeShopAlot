-------------------------------------------------------------------------------------------
-- ProductBrands
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	productBrand.Id
	,(CASE WHEN productBrand.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,productBrand.InternalId
    ,productBrand.Name
INTO #T1
FROM
	WSA.ProductBrand productBrand


SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	Name