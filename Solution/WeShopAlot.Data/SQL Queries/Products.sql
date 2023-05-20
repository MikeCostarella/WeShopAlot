-------------------------------------------------------------------------------------------
-- Products
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	product.Id
	,product.InternalId
	,(CASE WHEN product.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,product.Name AS ProductName
    ,product.Description
    ,product.PictureUrl
    ,product.Price
    ,product.ProductBrandId
	,productBrand.Name AS ProductBrandName
    ,product.ProductTypeId
	,productType.Name AS ProductTypeName
FROM
	WSA.Product product
	LEFT JOIN WSA.ProductBrand productBrand ON product.ProductBrandId = productBrand.Id
	LEFT JOIN WSA.ProductType productType ON product.ProductTypeId = productType.Id