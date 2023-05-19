-------------------------------------------------------------------------------------------
-- States/Provinces
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	stateProvince.Id
	,(CASE WHEN stateProvince.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	--,country.Id AS CountryId
	,country.Name AS CountryName
	,stateProvince.Name AS StateName
INTO #T1
FROM WSA.StateProvince stateProvince
	LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	CountryName, StateName