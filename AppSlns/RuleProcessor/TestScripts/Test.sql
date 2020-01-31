-- Examples for queries that exercise different SQL objects implemented by this assembly

-----------------------------------------------------------------------------------------
-- Stored procedure
-----------------------------------------------------------------------------------------
-- exec StoredProcedureName


-----------------------------------------------------------------------------------------
-- User defined function
-----------------------------------------------------------------------------------------
-- select dbo.FunctionName()


-----------------------------------------------------------------------------------------
-- User defined type
-----------------------------------------------------------------------------------------
-- CREATE TABLE test_table (col1 UserType)
--
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 1'))
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 2'))
-- INSERT INTO test_table VALUES (convert(uri, 'Instantiation String 3'))
--
-- select col1::method1() from test_table



-----------------------------------------------------------------------------------------
-- User defined type
-----------------------------------------------------------------------------------------
-- select dbo.AggregateName(Column1) from Table1

DECLARE @xml AS XML
SET @xml='<Rule><ResultType>BOOL</ResultType><Expressions><Expression><Name>E1</Name><Definition>[Object1] <![CDATA[<=]]> [Object2] </Definition></Expression><Expression><Name>E2</Name><Definition>[Object3] <![CDATA[>=]]> [Object4] </Definition></Expression><Expression><Name>GE</Name><Definition>E1 <![CDATA[&&]]> [Object4] </Definition></Expression></Expressions></Rule>'
SELECT ADB_Dev.[dbo].usp_CLR_ValidateTemplate(@xml)
