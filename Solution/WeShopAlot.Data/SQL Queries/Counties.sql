-------------------------------------------------------------------------------------------
-- Counties
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	county.Id
	,(CASE WHEN county.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,country.Name AS CountryName
	,stateProvince.Name AS StateName
	,county.Name AS CountyName
	--,CONCAT(county.Id + '-' + township.Id) AS SomeNumberAsString
	--,(SELECT FORMAT (county.DateFounded, 'yyyy-MM-dd hh:mm:ss')) AS DateFounded
	,(SELECT COUNT(DISTINCT township.Id)
		FROM WSA.Township
		WHERE
			township.CountyId = county.Id
	) AS TownshipCount
	,(STUFF((
		SELECT ',' + township.Name
		FROM
			WSA.Township township
		WHERE
			township.CountyId = county.Id
		GROUP BY township.Name
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
		) AS Townships			
INTO #T1
FROM WSA.County county
LEFT JOIN WSA.StateProvince stateProvince ON county.StateProvinceId = stateProvince.Id
LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id

SELECT * FROM #T1
WHERE
	1 = 1
	AND StateName = 'Ohio'
ORDER BY
	CountryName, StateName, CountyName
