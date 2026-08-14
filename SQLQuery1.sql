-- 1. Eliminar la relación del usuario con roles
DELETE FROM [dbo].[AspNetUserRoles] 
WHERE UserId IN (SELECT Id FROM [dbo].[AspNetUsers] WHERE Email = 'admin@todo.com');

-- 2. Eliminar el usuario
DELETE FROM [dbo].[AspNetUsers] WHERE Email = 'admin@todo.com';

-- 3. Verificar que se eliminó
SELECT * FROM [dbo].[AspNetUsers] WHERE Email = 'admin@todo.com';