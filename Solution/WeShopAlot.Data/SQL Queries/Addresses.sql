-------------------------------------------------------------------------------------------
-- Addresses
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	address.Id
	,(CASE WHEN address.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,address.AddressLine1
	,address.AddressLine2
	,address.City
INTO #T1
FROM WSA.Address address

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	City
