﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT INTO [User] (Id, UserName) VALUES (1, 'chetan')
INSERT INTO [User] (Id, UserName) VALUES (2, 'user2')
INSERT INTO [User] (Id, UserName) VALUES (3, 'user3')
