-------------------------------------------------------------------------------------------
-- TownshipTrustees   -- hh:mm:ss
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	townshipTrustee.Id
	,(CASE WHEN townshipTrustee.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,country.Name AS Country_Name
	,stateProvince.Name AS StateProvince_Name
    --,township.CountyId AS County_Id
	,county.Name AS County_Name
    ,townshipTrustee.TownshipId
    ,township.Name AS Township_Name
    ,townshipTrustee.FirstName
    ,townshipTrustee.LastName
    ,townshipTrustee.MiddleName
	,(SELECT FORMAT (townshipTrustee.TermEndDate, 'yyyy-MM-dd')) AS TermEndDate
	,(SELECT FORMAT (townshipTrustee.TermStartDate, 'yyyy-MM-dd')) AS TermStartDate
INTO #T1
FROM
	WSA.TownshipTrustee townshipTrustee
	LEFT JOIN WSA.Township township ON townshipTrustee.TownshipId = township.Id
	LEFT JOIN WSA.County county ON township.CountyId = county.Id
	LEFT JOIN WSA.StateProvince stateProvince ON county.StateProvinceId = stateProvince.Id
	LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id

SELECT * FROM #T1
WHERE
	1 = 1
	--AND Township_Name = 'NewCastle'
	--AND County_Name = 'Hardin County'
ORDER BY
	Country_Name, StateProvince_Name, County_Name, Township_Name
