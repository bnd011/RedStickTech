drop database if exists RST_DB;
create database RST_DB;
use RST_DB;
drop table if exists user_login_info;
create table user_login_info (
	user_email varchar(50) not null,
	verify varchar(140) not null,
	salt varchar(65) not null,
	primary key (user_email)
	);
    
drop table if exists schedules;
create table schedules (
	schedule_IDN integer not null auto_increment,
	user_email varchar(50) not null,
	archived bit default 0,
	url	varchar(200) not null,
	item varchar(200) not null,
	primary key (schedule_IDN)
	);
    
drop table if exists failed_login;
create table failed_login (
	user_email	varchar(50)	not null,
	failed_num	integer default 1,
	Time_of_try	varchar(20) not null,
	primary key (user_email)
	);

drop table if exists admin_info;
create table admin_info (
	Admin_email	varchar(50)	not null,
	Admin_position	varchar(25)	not null,
	primary key (Admin_email)
	);
	
alter table user_login_info add column emailVerified bool;
