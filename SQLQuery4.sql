-- Ver los roles del usuario
SELECT u.Email, r.Name 
FROM [dbo].[AspNetUsers] u
INNER JOIN [dbo].[AspNetUserRoles] ur ON u.Id = ur.UserId
INNER JOIN [dbo].[AspNetRoles] r ON ur.RoleId = r.Id
WHERE u.Email = 'admin@todo.com';

-- Eliminar el rol "Usuario" del admin
DELETE FROM [dbo].[AspNetUserRoles] 
WHERE UserId = (SELECT Id FROM [dbo].[AspNetUsers] WHERE Email = 'admin@todo.com')
AND RoleId = (SELECT Id FROM [dbo].[AspNetRoles] WHERE Name = 'Usuario');

-- Verificar que solo tenga el rol Administrador
SELECT u.Email, r.Name 
FROM [dbo].[AspNetUsers] u
INNER JOIN [dbo].[AspNetUserRoles] ur ON u.Id = ur.UserId
INNER JOIN [dbo].[AspNetRoles] r ON ur.RoleId = r.Id
WHERE u.Email = 'admin@todo.com';