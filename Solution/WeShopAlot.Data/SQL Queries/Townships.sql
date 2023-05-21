-------------------------------------------------------------------------------------------
-- Townships
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	township.Id
	,(CASE WHEN township.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	--,country.Id As Country_Id
	,country.Name AS Country_Name
	,stateProvince.Name AS StateProvince_Name
    --,township.CountyId AS County_Id
	,county.Name AS County_Name
    ,township.Name AS Township_Name
INTO #T1
FROM WSA.Township township
LEFT JOIN WSA.County county ON township.CountyId = county.Id
LEFT JOIN WSA.StateProvince stateProvince ON county.StateProvinceId = stateProvince.Id
LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id

SELECT * FROM #T1
WHERE
	1 = 1
	--AND Township_Name = 'NewCastle'
	--AND County_Name = 'Greene County'
ORDER BY
	Country_Name, StateProvince_Name, County_Name, Township_Name
