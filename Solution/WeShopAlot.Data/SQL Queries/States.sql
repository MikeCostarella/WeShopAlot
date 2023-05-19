-------------------------------------------------------------------------------------------
-- States/Provinces
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	aState.Id
	,(CASE WHEN aState.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	--,country.Id AS CountryId
	,country.Name AS CountryName
	,aState.Name AS StateName
INTO #T1
FROM WSA.[State] aState
	LEFT JOIN WSA.Country country ON aState.CountryId = country.Id

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	CountryName, StateName