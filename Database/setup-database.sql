-- Ihospital Survey Database Setup Script
-- Database: Ihospital
-- Server: DESKTOP-BPQ5AR0

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Ihospital')
BEGIN
    CREATE DATABASE Ihospital;
END
GO

USE Ihospital;
GO

-- Drop tables if they exist (for clean setup)
IF OBJECT_ID('RESPONSE_DETAIL', 'U') IS NOT NULL DROP TABLE RESPONSE_DETAIL;
IF OBJECT_ID('OPTION_LIST', 'U') IS NOT NULL DROP TABLE OPTION_LIST;
IF OBJECT_ID('SURVEY_RESPONSE', 'U') IS NOT NULL DROP TABLE SURVEY_RESPONSE;
IF OBJECT_ID('QUESTION', 'U') IS NOT NULL DROP TABLE QUESTION;
IF OBJECT_ID('RESPONDENT', 'U') IS NOT NULL DROP TABLE RESPONDENT;
IF OBJECT_ID('STAFF', 'U') IS NOT NULL DROP TABLE STAFF;
GO

-- Create STAFF table
CREATE TABLE STAFF (
    Staff_ID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL
);
GO

-- Create RESPONDENT table
CREATE TABLE RESPONDENT (
    Respondent_ID INT IDENTITY(1,1) PRIMARY KEY,
    Is_Anonymous BIT DEFAULT 0,
    Title NVARCHAR(20),
    First_Name NVARCHAR(100),
    Last_Name NVARCHAR(100),
    Date_Of_Birth DATE,
    Gender NVARCHAR(20),
    Contact_Phone NVARCHAR(30),
    Email NVARCHAR(150),
    [State] NVARCHAR(100),
    Home_Suburb NVARCHAR(100),
    Home_Postcode NVARCHAR(10),
    Mac_Address NVARCHAR(50),
    Created_DateTime DATETIME DEFAULT GETDATE()
);
GO

-- Create SURVEY_RESPONSE table
CREATE TABLE SURVEY_RESPONSE (
    SurveyResponse_ID INT IDENTITY(1,1) PRIMARY KEY,
    Respondent_ID INT NOT NULL,
    Response_DateTime DATETIME DEFAULT GETDATE(),
    Mac_Address NVARCHAR(50),
    CONSTRAINT FK_SurveyResponse_Respondent
        FOREIGN KEY (Respondent_ID) REFERENCES RESPONDENT(Respondent_ID)
        ON DELETE CASCADE
);
GO

-- Create QUESTION table
CREATE TABLE QUESTION (
    Question_ID INT IDENTITY(1,1) PRIMARY KEY,
    Question_Text NVARCHAR(500) NOT NULL,
    Question_Code NVARCHAR(100) UNIQUE,
    Question_Category NVARCHAR(100),
    Response_Type NVARCHAR(50)
        CHECK (Response_Type IN ('SingleChoice','MultiChoice','Text','Numeric','Date')),
    Is_Active BIT DEFAULT 1,
    Is_Required BIT DEFAULT 0,
    Max_Selections INT NULL,
    Display_Order INT DEFAULT 0
);
GO

-- Create OPTION_LIST table
CREATE TABLE OPTION_LIST (
    Option_ID INT IDENTITY(1,1) PRIMARY KEY,
    Question_ID INT NOT NULL,
    Option_Text NVARCHAR(255) NOT NULL,
    Is_Active BIT DEFAULT 1,
    Option_Order INT DEFAULT 0,
    CONSTRAINT FK_OptionList_Question
        FOREIGN KEY (Question_ID) REFERENCES QUESTION(Question_ID)
        ON DELETE CASCADE
);
GO

-- Create RESPONSE_DETAIL table
CREATE TABLE RESPONSE_DETAIL (
    ResponseDetail_ID INT IDENTITY(1,1) PRIMARY KEY,
    SurveyResponse_ID INT NOT NULL,
    Question_ID INT NOT NULL,
    Option_ID INT NULL,
    Answer_Text NVARCHAR(500) NULL,
    Created_DateTime DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_ResponseDetail_Survey
        FOREIGN KEY (SurveyResponse_ID) REFERENCES SURVEY_RESPONSE(SurveyResponse_ID)
        ON DELETE CASCADE,
    CONSTRAINT FK_ResponseDetail_Question
        FOREIGN KEY (Question_ID) REFERENCES QUESTION(Question_ID),
    CONSTRAINT FK_ResponseDetail_Option
        FOREIGN KEY (Option_ID) REFERENCES OPTION_LIST(Option_ID)
);
GO

-- Insert default admin staff (password is 'admin123' - hashed with BCrypt)
INSERT INTO STAFF (UserName, [Password])
VALUES ('admin', '$2a$11$xKjB5ZK7YGXGJzVZ5z5z5.5ZK7YGXGJzVZ5z5z5.5ZK7YGXGJzVZ5');
GO

-- Insert sample questions
INSERT INTO QUESTION (Question_Text, Question_Code, Question_Category, Response_Type, Is_Active, Is_Required, Display_Order)
VALUES 
    ('What is your age group?', 'Q001', 'Demographics', 'SingleChoice', 1, 1, 1),
    ('How would you rate your overall experience?', 'Q002', 'Satisfaction', 'SingleChoice', 1, 1, 2),
    ('What services did you use? (Select all that apply)', 'Q003', 'Services', 'MultiChoice', 1, 0, 3),
    ('Please provide any additional comments:', 'Q004', 'Feedback', 'Text', 1, 0, 4);
GO

-- Insert options for Question 1 (Age Group)
INSERT INTO OPTION_LIST (Question_ID, Option_Text, Option_Order)
VALUES 
    (1, '18-25', 1),
    (1, '26-35', 2),
    (1, '36-45', 3),
    (1, '46-55', 4),
    (1, '56-65', 5),
    (1, '65+', 6);
GO

-- Insert options for Question 2 (Rating)
INSERT INTO OPTION_LIST (Question_ID, Option_Text, Option_Order)
VALUES 
    (2, 'Excellent', 1),
    (2, 'Very Good', 2),
    (2, 'Good', 3),
    (2, 'Fair', 4),
    (2, 'Poor', 5);
GO

-- Insert options for Question 3 (Services)
INSERT INTO OPTION_LIST (Question_ID, Option_Text, Option_Order)
VALUES 
    (3, 'Emergency Services', 1),
    (3, 'Outpatient Consultation', 2),
    (3, 'Laboratory Tests', 3),
    (3, 'Radiology/Imaging', 4),
    (3, 'Pharmacy', 5),
    (3, 'Inpatient Ward', 6);
GO

PRINT 'Database setup completed successfully!';
PRINT 'Default admin credentials:';
PRINT 'Username: admin';
PRINT 'Password: admin123';
GO
