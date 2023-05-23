-------------------------------------------------------------------------------------------
-- MunicipalLawDirectors
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	municipalLawDirector.Id
	,(CASE WHEN municipalLawDirector.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,country.Name AS Country_Name
	,stateProvince.Name AS StateProvince_Name
    --,municipalCouncilPresident.MunicipalityId
	,municipality.Name AS Municipality_Name
	,municipalityType.Name AS Municipality_Type
    ,municipalLawDirector.FirstName
    ,municipalLawDirector.LastName
    ,municipalLawDirector.MiddleName
	,(SELECT FORMAT (municipalLawDirector.TermStartDate, 'yyyy-MM-dd')) AS TermStartDate
	,(SELECT FORMAT (municipalLawDirector.TermEndDate, 'yyyy-MM-dd')) AS TermEndDate
	,municipality.Telephone
	,municipality.MailingAddressLine1
	,municipality.MailingAddressLine2
	,municipality.ZIPCode
	,municipality.Website
INTO #T1
FROM
	WSA.MunicipalLawDirector municipalLawDirector
	LEFT JOIN WSA.Municipality municipality ON municipalLawDirector.MunicipalityId = municipality.Id
	LEFT JOIN WSA.StateProvince stateProvince ON municipality.StateId = stateProvince.Id
	LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id
	LEFT JOIN WSA.MunicipalityType municipalityType ON municipality.MunicipalityTypeId = municipalityType.Id

SELECT * FROM #T1
WHERE
	1 = 1
	--AND LastName = ''
ORDER BY
	Country_Name, StateProvince_Name, Municipality_Name, LastName, FirstName, MiddleName