-------------------------------------------------------------------------------------------
-- ProductBrands
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	productBrand.Id
	,(CASE WHEN productBrand.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,productBrand.InternalId
    ,productBrand.Name
	,(SELECT COUNT(DISTINCT product.Id)
		FROM WSA.Product product
		WHERE
			product.ProductBrandId = productBrand.Id
	) AS ProductCount
	,(STUFF((
		SELECT ',' + product.Name
		FROM
			WSA.Product product
		WHERE
			product.ProductBrandId = productBrand.Id
		GROUP BY product.Name
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
		) AS Products			
INTO #T1
FROM
	WSA.ProductBrand productBrand


SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	Name