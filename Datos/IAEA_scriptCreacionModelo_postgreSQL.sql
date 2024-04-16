-- Parcial #03 TABD
-- Curso de Tópicos Avanzados de base de datos
-- Miguel Quijano Jaramillo - miguel.quijano@upb.edu.co

-- Proyecto:  IAEA_Reactores_Nucleares_Investigacion
-- Motor de Base de datos: PostgreSQL 16.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull postgres:latest

-- Crear el contenedor
docker run --name postgres-IAEA -e POSTGRES_PASSWORD=unaClav3 -d -p 5432:5432 postgres:latest

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario Root:

-- crear el esquema la base de datos
create database IAEA_db;

-- Conectarse a la base de datos
\c IAEA_db;

-- Creamos un esquema para almacenar todo el modelo de datos del dominio
create schema core;
create schema auth;
create schema localizacion;
create schema auditoria;

-- crear el usuario con el que se implementará la creación del modelo
create user IAEA_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database IAEA_db to IAEA_app;
grant create on database IAEA_db to IAEA_app;
grant create, usage on schema core to IAEA_app;
alter user IAEA_app set search_path to core;


-- crear el usuario con el que se conectará la aplicación
create user IAEA_usr with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database IAEA_db to IAEA_usr;
grant usage on schema core, localizacion to IAEA_usr;
alter default privileges for user IAEA_app in schema core grant insert, update, delete, select on tables to IAEA_usr;
alter default privileges for user IAEA_app in schema core grant execute on routines TO IAEA_usr;
alter user IAEA_usr set search_path to core;

-- ----------------------------------------
-- Script de creación de tablas y vistas
-- ----------------------------------------
