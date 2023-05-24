-------------------------------------------------------------------------------------------
-- Municipalities
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	municipality.Id
	,(CASE WHEN municipality.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    --,municipality.StateId
	,stateProvince.Name AS State_Name
	,municipalityType.Name AS Type
    ,municipality.Name
    --,municipality.MunicipalityTypeId
	,municipality.Telephone
	,municipality.MailingAddressLine1
	,municipality.MailingAddressLine2
	,municipality.ZIPCode
	,municipality.Website
	,municipality.FormOfGovernment
	,FORMAT(municipality.Census2000, N'N0') AS Census2000
	,FORMAT(municipality.Census2010, N'N0') AS Census2010
	,FORMAT(municipality.Census2020, N'N0') AS Census2020
	,municipality.YearIncorporated
	,(STUFF((
		SELECT ',' + str(municipalIncomeTaxRate.Rate,8,3)
		FROM WSA.MunicipalIncomeTaxRate municipalIncomeTaxRate
		WHERE
			municipalIncomeTaxRate.MunicipalityId = municipality.Id
			AND municipalIncomeTaxRate.EndDate IS NULL
		GROUP BY str(municipalIncomeTaxRate.Rate,8,3)
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS IncomeTaxRate			
	,(STUFF((
		SELECT ',' + mayor.FirstName + ' ' + mayor.LastName 
		FROM WSA.Mayor mayor
		WHERE
			mayor.MunicipalityId = municipality.Id
		GROUP BY mayor.FirstName, mayor.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS Mayor			
	,(STUFF((
		SELECT ',' + municipalCouncilPresident.FirstName + ' ' + municipalCouncilPresident.LastName 
		FROM WSA.MunicipalCouncilPresident municipalCouncilPresident
		WHERE
			municipalCouncilPresident.MunicipalityId = municipality.Id
		GROUP BY municipalCouncilPresident.FirstName, municipalCouncilPresident.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS CouncilPresident			
	,(STUFF((
		SELECT ',' + municipalAuditor.FirstName + ' ' + municipalAuditor.LastName 
		FROM WSA.MunicipalAuditor municipalAuditor
		WHERE
			municipalAuditor.MunicipalityId = municipality.Id
		GROUP BY municipalAuditor.FirstName, municipalAuditor.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS Auditor			
	,(STUFF((
		SELECT ',' + municipalLawDirector.FirstName + ' ' + municipalLawDirector.LastName 
		FROM WSA.MunicipalLawDirector municipalLawDirector
		WHERE
			municipalLawDirector.MunicipalityId = municipality.Id
		GROUP BY municipalLawDirector.FirstName, municipalLawDirector.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS LawDirector			
	,(STUFF((
		SELECT ',' + municipalTreasurer.FirstName + ' ' + municipalTreasurer.LastName 
		FROM WSA.MunicipalTreasurer municipalTreasurer
		WHERE
			municipalTreasurer.MunicipalityId = municipality.Id
		GROUP BY municipalTreasurer.FirstName, municipalTreasurer.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS Treasurer
	,(SELECT COUNT(DISTINCT municipalCouncilMember.Id)
		FROM WSA.MunicipalCouncilMember municipalCouncilMember
		WHERE
			municipalCouncilMember.MunicipalityId = municipality.Id
	) AS CouncilMemberCount
	,(STUFF((
		SELECT ',' + municipalCouncilMember.FirstName + ' ' + municipalCouncilMember.LastName 
		FROM WSA.MunicipalCouncilMember municipalCouncilMember
		WHERE
			municipalCouncilMember.MunicipalityId = municipality.Id
		GROUP BY municipalCouncilMember.FirstName, municipalCouncilMember.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS CouncilMembers			
INTO #T1
FROM
	WSA.Municipality municipality
	LEFT JOIN WSA.StateProvince stateProvince ON municipality.StateId = stateProvince.Id
	LEFT JOIN WSA.MunicipalityType municipalityType ON municipality.MunicipalityTypeId = municipalityType.Id

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	State_Name, Name