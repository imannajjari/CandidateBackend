USE [CandidateDB]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230727114644_myMigration01', N'7.0.5')
GO
SET IDENTITY_INSERT [dbo].[People] ON 
GO
INSERT [dbo].[People] ([ID], [FirstName], [LastName], [PersonCode], [BasicSalary], [Allowance], [Transportation], [HoursWorked], [TotalSalary], [Date], [IsActive], [IsDeleted], [CreationDateTime], [ModificationDateTime]) VALUES (1, N'ایمان', N'نجاری', N'1234', 15000, 100, 140, 45, 19953, N'14020505', 0, 0, CAST(N'2023-07-27T16:26:58.510' AS DateTime), NULL)
GO
INSERT [dbo].[People] ([ID], [FirstName], [LastName], [PersonCode], [BasicSalary], [Allowance], [Transportation], [HoursWorked], [TotalSalary], [Date], [IsActive], [IsDeleted], [CreationDateTime], [ModificationDateTime]) VALUES (2, N'ایمان', N'نجاری', N'1234', 15000, 100, 140, 45, 19953, N'14020505', 0, 0, CAST(N'2023-07-27T16:30:25.420' AS DateTime), NULL)
GO
INSERT [dbo].[People] ([ID], [FirstName], [LastName], [PersonCode], [BasicSalary], [Allowance], [Transportation], [HoursWorked], [TotalSalary], [Date], [IsActive], [IsDeleted], [CreationDateTime], [ModificationDateTime]) VALUES (3, N'ایمان', N'نجاری', N'1234', 15000, 100, 140, 45, 19953, N'14020505', 0, 0, CAST(N'2023-07-27T16:30:25.420' AS DateTime), NULL)
GO
INSERT [dbo].[People] ([ID], [FirstName], [LastName], [PersonCode], [BasicSalary], [Allowance], [Transportation], [HoursWorked], [TotalSalary], [Date], [IsActive], [IsDeleted], [CreationDateTime], [ModificationDateTime]) VALUES (6, N'Iman', N'Najjari', N'3333', 14000, 120, 160, 42, 21000, N'14020404', 0, 1, CAST(N'2023-07-27T16:50:46.027' AS DateTime), CAST(N'2023-07-27T17:12:55.267' AS DateTime))
GO
INSERT [dbo].[People] ([ID], [FirstName], [LastName], [PersonCode], [BasicSalary], [Allowance], [Transportation], [HoursWorked], [TotalSalary], [Date], [IsActive], [IsDeleted], [CreationDateTime], [ModificationDateTime]) VALUES (7, N'ایمان', N'نجاری', N'1234', 15000, 100, 140, 45, 19953, N'1402/05/05', 0, 0, CAST(N'2023-07-27T16:52:41.787' AS DateTime), CAST(N'2023-07-27T17:23:33.353' AS DateTime))
GO
INSERT [dbo].[People] ([ID], [FirstName], [LastName], [PersonCode], [BasicSalary], [Allowance], [Transportation], [HoursWorked], [TotalSalary], [Date], [IsActive], [IsDeleted], [CreationDateTime], [ModificationDateTime]) VALUES (8, N'Iman', N'Najjari', N'3333', 14000, 120, 160, 47, 17420.4, N'14020509', 0, 0, CAST(N'2023-07-27T16:52:59.950' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[People] OFF
GO
