﻿CREATE TABLE SIMMESSAGES (SenderID INTEGER, RecipientID INTEGER, Text TEXT, Timestamp DATETIME, ThreadID INTEGER);
CREATE TABLE SIMTABLES (ID INTEGER, Participants TEXT, Messages TEXT, HasUnread BOOLEAN, IsGroup BOOLEAN, GroupName TEXT);