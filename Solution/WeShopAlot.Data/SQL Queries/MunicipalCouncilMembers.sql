-------------------------------------------------------------------------------------------
-- MunicipalCouncilMembers
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1
SELECT
	municipalCouncilMember.Id
	,(CASE WHEN municipalCouncilMember.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,country.Name AS Country_Name
	,stateProvince.Name AS StateProvince_Name
    --,mayor.MunicipalityId
	,municipality.Name AS Municipality_Name
	,municipalityType.Name AS Municipality_Type
	,municipalCouncilMember.Title
    ,municipalCouncilMember.FirstName
    ,municipalCouncilMember.LastName
    ,municipalCouncilMember.MiddleName
	,(SELECT FORMAT (municipalCouncilMember.TermStartDate, 'yyyy-MM-dd')) AS TermStartDate
	,(SELECT FORMAT (municipalCouncilMember.TermEndDate, 'yyyy-MM-dd')) AS TermEndDate
	,municipality.Telephone
	,municipality.MailingAddressLine1
	,municipality.MailingAddressLine2
	,municipality.ZIPCode
	,municipality.Website
INTO #T1
FROM
	WSA.MunicipalCouncilMember municipalCouncilMember
	LEFT JOIN WSA.Municipality municipality ON municipalCouncilMember.MunicipalityId = municipality.Id
	LEFT JOIN WSA.StateProvince stateProvince ON municipality.StateId = stateProvince.Id
	LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id
	LEFT JOIN WSA.MunicipalityType municipalityType ON municipality.MunicipalityTypeId = municipalityType.Id

SELECT * FROM #T1
WHERE
	1 = 1
	--AND LastName = ''
ORDER BY
	Country_Name, StateProvince_Name, Municipality_Name, LastName, FirstName, MiddleName