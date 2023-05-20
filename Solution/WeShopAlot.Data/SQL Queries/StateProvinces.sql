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
	,stateProvince.Abbreviation AS StateAbbreviation
	,(SELECT COUNT(DISTINCT county.Id)
		FROM WSA.County county
		WHERE
			county.StateProvinceId = stateProvince.Id
	) AS CountyCount
	,(STUFF((
		SELECT ',' + county.Name
		FROM
			WSA.County county
		WHERE
			county.StateProvinceId = stateProvince.Id
		GROUP BY county.Name
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
		) AS Counties			
INTO #T1
FROM WSA.StateProvince stateProvince
	LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	CountryName, StateName