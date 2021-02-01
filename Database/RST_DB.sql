/*create database RST_DB;
drop table if exists user_login_info;
create table [user_login_info] (
	user_email varchar(50) not null,
	verify	varchar(12) not null,
	salt	varchar(65) not null,
	primary key (user_email)
	);
    
drop table if exists schedules;
create table [user_login_info] (
	schedule_IDN integer not null,
	user_email varchar(50) not null,
	archieved bool default 0,
	url	varchar(200) not null,
	is_recurring bool default 0,
    frequency integer default 0,
    want_option integer default -1,
    price_limit integer,
    expire_date date,
    current_price float not null,
    item varchar(200) not null,
	primary key (schedule_IDN)
	);
    
drop table if exists failed_login;
create table [failed_login] (
	user_email	varchar(50)	not null,
	failed_num	integer default 1,
	Time_of_try	datetime not null,
	primary key (user_email)
	);

drop table if exists admin_info;
create table [admin_info] (
	Admin_email	varchar(50)	not null,
	Admin_position	varchar(25)	not null
	primary key (Admin_email)
	);
#########################################################################
extra table if we need it in the future.

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

alter table user_login_info drop column Passwd;
################################################################
*/



