set identity_insert SystemProcess on
INSERT INTO SystemProcess
                         (Id, Symbol, UserId, Name, Description, ParentProcessId)
VALUES        (0, N'NP', 0, N'Null System Process', 'Null System Process', 0)
set identity_insert SystemProcess off