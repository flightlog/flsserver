USE [master]
GO
/****** Object:  Database [FLSTest]    Script Date: 03/06/2012 00:23:43 ******/
CREATE DATABASE [FLSTest] ON  PRIMARY 
( NAME = N'FLSTest', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\FLSTest.mdf' , SIZE = 6144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FLSTest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\FLSTest_1.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
