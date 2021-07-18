create table if not exists "SSO"."Accounts"
(
	"Id" uuid,
	"RoleId" smallint,
	"Email" varchar(50),
	"FirstName" varchar(50),
	"LastName" varchar(50),
	"PasswordHash" bytea
);
