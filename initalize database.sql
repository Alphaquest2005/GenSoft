set identity_insert SystemProcess on
INSERT INTO DomainProcess
                         (Id, Symbol, UserId, Name, Description, ParentProcessId)
VALUES        (0, N'NP', 0, N'Null System Process', 'Null System Process', 0)
set identity_insert DomainProcess off

set identity_insert [Application] on
INSERT INTO [Application]
                         (Id, Name)
VALUES        (0, 'System')
set identity_insert [Application] off