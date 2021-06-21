DROP DATABASE IF EXISTS AppBanco_DB;
CREATE DATABASE IF NOT EXISTS AppBanco_DB;

USE AppBanco_DB;

CREATE TABLE IF NOT EXISTS appUser(
	id int primary key auto_increment,
	name varchar(50),
	role varchar(50),
	date date
);

INSERT INTO appUser (name, role, date)
VALUES ('Felipe Testador', 'Desenvolvedor', '2004/08/02');

SELECT * FROM appUser;