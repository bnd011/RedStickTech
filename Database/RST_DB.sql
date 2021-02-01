/*create database RST_DB;
drop table if exists user_login_info;
create table [user_login_info] (
	User_fname varchar(30) not null,
	User_lname varchar(30) not null,
	Uname	varchar(50)	not null,
	Passwd	varchar(25) not null,
	Salt	varchar(65) not null,
	primary key (Uname)
	);

drop table if exists failed_login;
create table [failed_login] (
	Uname_try	varchar(50)	not null,
	PassWd_try	varchar(25) not null,
	Time_of_try	datetime	not null
	primary key (Uname_try)
	);

drop table if exists admin_info;
create table [admin_info] (
	Admin_fname varchar(25) not null,
	Admin_lname varchar(25) not null,
	Admin_uname	varchar(50)	not null,
	Admin_passwd	varchar(25) not null,
	Admin_position	varchar(25)	not null
	primary key (Admin_uname)
	);

drop table if exists active_schedules;
create table [active_schedules] (
	Item_ID int not null,
	Item_name varchar(30) not null,
	Item_price float not null,
	primary key (Item_ID)
	);

drop table if exists archived_schedules;
create table [archived_schedules] (
	Item_ID int not null,
	Item_name varchar(30) not null,
	Item_price float not null,
	primary key (Item_ID)
	);


drop table if exists user_payment_info;
create table [user_payment_info] (
	Fname varchar(25) not null,
	Lname varchar(25) not null,
	Uname varchar(50) not null,
	User_address varchar(75) not null,
	Name_on_card varchar(50) not null,
	Card_no bigint not null,
	Expir_date date not null,
	CVC int not null
	);


alter table user_login_info add verify varchar(50) not null;

alter table active_schedules add ScheduleIDN int not null;

alter table active_schedules add Archived Bit default 0;

alter table active_schedules add Is_Recurring Bit default 0;

alter table active_schedules add Frequency int default 0;

alter table active_schedules add NeedWant Bit default 0;

alter table active_schedules add PriceLimit int;

alter table active_schedules add ExpDate date;

alter table active_schedules add CurrentPrice float not null;

alter table active_schedules alter column PriceLimit float;

alter table admin_info drop column Admin_passwd;

alter table admin_info add Admin_salt varchar(60) not null;

alter table user_login_info drop column Passwd;*/



