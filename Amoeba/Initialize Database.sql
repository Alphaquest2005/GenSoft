

SET IDENTITY_INSERT Agent ON; 
	insert into Agent(id, UserName) values (0, 'System')
	insert into Agent(id, UserName) values (0, 'joe')
SET IDENTITY_INSERT Agent off; 

SET IDENTITY_INSERT [User] ON; 
	insert into [User](id, Password) values (1, 'test')
SET IDENTITY_INSERT [User] off; 

SET IDENTITY_INSERT EntityType ON; 
	insert into EntityType(id, Password) values (1, 'test')
SET IDENTITY_INSERT EntityType off; 