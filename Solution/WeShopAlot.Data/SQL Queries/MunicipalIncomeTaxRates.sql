-------------------------------------------------------------------------------------------
-- MunicipalIncomeTaxRates
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	municipalIncomeTaxRate.Id
	,(CASE WHEN municipalIncomeTaxRate.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,country.Name AS Country_Name
	,stateProvince.Name AS StateProvince_Name
	,municipalityType.Name AS Municipality_Type
	,municipality.Name AS Municipality_Name
    ,municipalIncomeTaxRate.MunicipalityId
	--,municipalityType.Name AS Municipality_Type
	,(SELECT FORMAT (municipalIncomeTaxRate.StartDate, 'yyyy-MM-dd')) AS StartDate
	,(SELECT FORMAT (municipalIncomeTaxRate.EndDate, 'yyyy-MM-dd')) AS EndDate
    ,municipalIncomeTaxRate.Rate
	,municipality.Telephone
	,municipality.MailingAddressLine1
	,municipality.MailingAddressLine2
	,municipality.ZIPCode
	,municipality.Website
INTO #T1
FROM
	WSA.MunicipalIncomeTaxRate municipalIncomeTaxRate
	LEFT JOIN WSA.Municipality municipality ON municipalIncomeTaxRate.MunicipalityId = municipality.Id
	LEFT JOIN WSA.StateProvince stateProvince ON municipality.StateId = stateProvince.Id
	LEFT JOIN WSA.Country country ON stateProvince.CountryId = country.Id
	LEFT JOIN WSA.MunicipalityType municipalityType ON municipality.MunicipalityTypeId = municipalityType.Id

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	Country_Name, StateProvince_Name, Municipality_Name, StartDate DESC