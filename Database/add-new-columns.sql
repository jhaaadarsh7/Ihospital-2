-- Add new columns to existing RESPONDENT table
-- Run this script to update your existing database schema

USE Ihospital;
GO

-- Add new columns if they don't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RESPONDENT') AND name = 'Has_Private_Insurance')
BEGIN
    ALTER TABLE RESPONDENT ADD Has_Private_Insurance BIT NULL;
    PRINT 'Added Has_Private_Insurance column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RESPONDENT') AND name = 'Insurance_Providers')
BEGIN
    ALTER TABLE RESPONDENT ADD Insurance_Providers NVARCHAR(500) NULL;
    PRINT 'Added Insurance_Providers column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RESPONDENT') AND name = 'Discharge_Plans')
BEGIN
    ALTER TABLE RESPONDENT ADD Discharge_Plans NVARCHAR(500) NULL;
    PRINT 'Added Discharge_Plans column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RESPONDENT') AND name = 'Stay_Period')
BEGIN
    ALTER TABLE RESPONDENT ADD Stay_Period NVARCHAR(100) NULL;
    PRINT 'Added Stay_Period column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RESPONDENT') AND name = 'Wifi_Plan')
BEGIN
    ALTER TABLE RESPONDENT ADD Wifi_Plan NVARCHAR(100) NULL;
    PRINT 'Added Wifi_Plan column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RESPONDENT') AND name = 'Wifi_Satisfaction')
BEGIN
    ALTER TABLE RESPONDENT ADD Wifi_Satisfaction NVARCHAR(100) NULL;
    PRINT 'Added Wifi_Satisfaction column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('RESPONDENT') AND name = 'Wifi_Services_Used')
BEGIN
    ALTER TABLE RESPONDENT ADD Wifi_Services_Used NVARCHAR(MAX) NULL;
    PRINT 'Added Wifi_Services_Used column';
END

PRINT 'Database schema update completed successfully!';
GO
