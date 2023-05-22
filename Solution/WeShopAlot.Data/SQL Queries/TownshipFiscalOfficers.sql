-------------------------------------------------------------------------------------------
-- TownshipFiscalOfficers
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	townshipFiscalOfficer.Id
	,(CASE WHEN townshipFiscalOfficer.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,country.Name AS Country_Name
	,stateProvince.Name AS StateProvince_Name
    --,township.CountyId AS County_Id
	,county.Name AS County_Name
    --,townshipFiscalOfficer.TownshipId
    ,township.Name AS Township_Name
    ,townshipFiscalOfficer.FirstName
    ,townshipFiscalOfficer.LastName
    ,townshipFiscalOfficer.MiddleName
	,(SELECT FORMAT (townshipFiscalOfficer.TermStartDate, 'yyyy-MM-dd')) AS TermStartDate
	,(SELECT FORMAT (townshipFiscalOfficer.TermEndDate, 'yyyy-MM-dd')) AS TermEndDate
INTO #T1
FROM
	WSA.TownshipFiscalOfficer townshipFiscalOfficer
	LEFT JOIN WSA.Township township ON townshipFiscalOfficer.TownshipId = township.Id
	LEFT JOIN WSA.County county ON township.CountyId = county.Id
	LEFT JOIN WSA.StateProvince stateProvince ON county.StateProvinceId = stateProvince.Id
	LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id

SELECT * FROM #T1
WHERE
	1 = 1
	--AND Township_Name = 'NewCastle'
	--AND County_Name = 'Hardin County'
ORDER BY
	Country_Name, StateProvince_Name, County_Name, Township_Name, LastName, FirstName, MiddleName
