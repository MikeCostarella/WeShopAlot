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
	,township.MailingAddressLine1
	,township.MailingAddressLine2
	,township.MailingAddressCity
	,township.MailingAddressZIPCode
	,township.WebSiteUrl
	--,(SELECT COUNT(DISTINCT townshipFiscalOfficer.Id)
	--	FROM WSA.TownshipFiscalOfficer townshipFiscalOfficer
	--	WHERE
	--		townshipFiscalOfficer.TownshipId = township.Id
	--) AS FiscalOfficerCount
	,(STUFF((
		SELECT ',' + townshipFiscalOfficer.FirstName + ' ' + townshipFiscalOfficer.LastName 
		FROM WSA.TownshipFiscalOfficer townshipFiscalOfficer
		WHERE
			townshipFiscalOfficer.TownshipId = township.Id
		GROUP BY townshipFiscalOfficer.FirstName, townshipFiscalOfficer.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS FiscalOfficer			
	,(SELECT COUNT(DISTINCT townshipTrustee.Id)
		FROM WSA.TownshipTrustee townshipTrustee
		WHERE
			townshipTrustee.TownshipId = township.Id
	) AS TrusteeCount
	,(STUFF((
		SELECT ',' + townshipTrustee.FirstName + ' ' + townshipTrustee.LastName 
		FROM WSA.TownshipTrustee townshipTrustee
		WHERE
			townshipTrustee.TownshipId = township.Id
		GROUP BY townshipTrustee.FirstName, townshipTrustee.LastName
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '')
	) AS Trustees			
INTO #T1
FROM WSA.Township township
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
