-------------------------------------------------------------------------------------------
-- ProductTypes
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	productType.Id
	,(CASE WHEN productType.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,productType.InternalId
    ,productType.Name
	,(SELECT COUNT(DISTINCT product.Id)
		FROM WSA.Product product
		WHERE
			product.ProductTypeId = productType.Id
	) AS ProductCount
	,(STUFF((
		SELECT ',' + product.Name
		FROM
			WSA.Product product
		WHERE
			product.ProductTypeId = productType.Id
		GROUP BY product.Name
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
		) AS Products			
INTO #T1
FROM
	WSA.ProductType productType


SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	Name