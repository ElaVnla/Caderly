USE [CaderlyDB]
GO

/****** Object: Table [dbo].[BookInfo] Script Date: 19/1/2022 12:29:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BookInfo] (
    [bookId]       INT            NOT NULL,
    [booktype]     INT            NOT NULL,
    [booktitle]    NVARCHAR (255) NOT NULL,
    [bookvisitors] INT            NOT NULL,
    [bookyear]     INT            NOT NULL,
    [bookmonth]    INT            NOT NULL,
    [bookday]      INT            NOT NULL,
    [booktime]     NVARCHAR (10)   NOT NULL,
    [bookstatus]   NVARCHAR (50)  NOT NULL,
    [bookduration] NVARCHAR (20)  NOT NULL
);


